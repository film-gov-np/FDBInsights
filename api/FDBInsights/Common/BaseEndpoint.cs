using ErrorOr;
using FastEndpoints;
using FDBInsights.Dto;

namespace FDBInsights.Common;

public class BaseEndpointCore(ILogger<BaseEndpointCore> logger, IHttpContextAccessor httpContextAccessor)
{
    public readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public readonly ILogger _logger = logger;

    public UserInfo? CurrentUser => _httpContextAccessor.HttpContext?.Items["user"] as UserInfo;
    public string CurrentUserEmail => CurrentUser?.GetUserEmail ?? string.Empty;
    public string CurrentUserFullName => CurrentUser?.GetUserFullName ?? string.Empty;
    public string? CurrentUserJwtToken => CurrentUser?.GetUserJwtToken ?? string.Empty;

    // get the current user from the http context
    public string HostUrl
    {
        get
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return string.Empty;

            var host = request.Host;
            var scheme = request.Scheme;
            return $"{scheme}://{host}{request.PathBase}";
        }
    }
}

public abstract class BaseEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse>, IBaseEndpoint
    where TRequest : notnull where TResponse : notnull
{
    private readonly BaseEndpointCore _baseEndpointCore;

    protected BaseEndpoint(BaseEndpointCore baseEndpointCore)
    {
        _baseEndpointCore = baseEndpointCore;
    }

    protected IHttpContextAccessor _httpContextAccessor => _baseEndpointCore._httpContextAccessor;
    private ILogger _logger => _baseEndpointCore._logger;


    // check if the response is an ErrorOr<T> or just a T
    protected bool IsErrorOrResponse => typeof(TResponse).IsGenericType &&
                                        typeof(TResponse).GetGenericTypeDefinition() == typeof(ErrorOr<>);

    public UserInfo? CurrentUser => _baseEndpointCore.CurrentUser;
    public string? CurrentUserEmail => _baseEndpointCore.CurrentUserEmail;
    public string? CurrentUserFullName => _baseEndpointCore.CurrentUserFullName;
    public string? CurrentUserToken => _baseEndpointCore.CurrentUserJwtToken;
    public string HostUrl => _baseEndpointCore.HostUrl;


    protected void LogError(Exception ex, string message)
    {
        _logger.LogError(ex, message);
    }

    // for both ErrorOr<T> and T responses, return the response
    protected async Task SendResultAsync(TResponse result, int statusCode = 200, CancellationToken ct = default)
    {
        // for ErrorOr<T> responses, return the response
        if (IsErrorOrResponse && result != null)
        {
            // Get the IsError property via reflection (since we can't cast to ErrorOr<> directly)
            var isErrorProperty = result.GetType().GetProperty("IsError");
            var isError = (bool)(isErrorProperty?.GetValue(result) ?? false);

            if (isError)
            {
                // Get FirstError via reflection
                var firstErrorProperty = result.GetType().GetProperty("FirstError");
                var firstError = firstErrorProperty?.GetValue(result);
                var errorType = firstError?.GetType().GetProperty("Type")?.GetValue(firstError);
                var errorDescription = firstError?.GetType().GetProperty("Description")?.GetValue(firstError) as string;

                // Determine status code based on error type
                statusCode = errorType?.ToString() switch
                {
                    "NotFound" => 404,
                    "Validation" => 400,
                    "Unauthorized" => 401,
                    "Forbidden" => 403,
                    _ => 500
                };

                Logger.LogWarning("Error in endpoint {EndpointName}: {ErrorType} - {ErrorDescription}",
                    GetType().Name, errorType, errorDescription);

                await SendErrorsAsync(statusCode, ct);
                return;
            }
        }

        // for T responses, return the response
        if (result == null)
        {
            await SendErrorsAsync(500, ct);
            return;
        }

        await SendAsync(result, statusCode, ct);
    }

    private async Task SendErrorAsync(string message, int statusCode = 400, CancellationToken ct = default)
    {
        Logger.LogWarning("Error in endpoint {EndpointName}: {ErrorMessage}",
            GetType().Name, message);

        if (IsErrorOrResponse)
        {
            await SendErrorsAsync(statusCode, ct);
            return;
        }

        ThrowError(message, statusCode);
    }

    protected async Task SendNotFoundAsync(string message = "Resource not found", CancellationToken ct = default)
    {
        await SendErrorAsync(message, 404, ct);
    }

    protected async Task SendBadRequestAsync(string message = "Bad request", CancellationToken ct = default)
    {
        await SendErrorAsync(message, 400, ct);
    }
}