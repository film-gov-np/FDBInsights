using System.Net;
using ErrorOr;
using FDBInsights.Common;
using FDBInsights.Constants;
using FDBInsights.Service;

namespace FDBInsights.Features.Auth;

public class AuthEndpoint(IAuthService userService, BaseEndpointCore baseEndpointCore)
    : BaseEndpoint<AuthRequest, ApiResponse<AuthResponse>>(baseEndpointCore)
{
    private readonly BaseEndpointCore _baseEndpointCore = baseEndpointCore;
    private readonly IAuthService _userService = userService;

    public override void Configure()
    {
        Post("/auth/login");
        //Options(x => x.CacheOutput(p => p.Expire(TimeSpan.FromSeconds(60))));
        ResponseCache(60);
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Authenticates a user";
            s.Description = "Authenticates a user with username and password and returns a JWT token";
            s.Response<AuthResponse>(200, "User authenticated successfully");
            s.Response(401, "Invalid credentials");
        });
    }

    public override async Task HandleAsync(AuthRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _userService.GetByUserNameAsync(req.Username, req.Password, ct);

            if (result.IsError)
            {
                var httpStatusCode = HttpStatusCode.Unauthorized;
                switch (result.FirstError.Type)
                {
                    case ErrorType.NotFound:
                        httpStatusCode = HttpStatusCode.NotFound;
                        break;
                    case ErrorType.Validation:
                        httpStatusCode = HttpStatusCode.BadRequest;
                        break;
                    default:
                        await SendErrorsAsync((int)HttpStatusCode.InternalServerError, ct);
                        return;
                }

                await SendAsync(ApiResponse<AuthResponse>.ErrorResponse(
                    result.FirstError.Description), (int)httpStatusCode, ct);
                return;
            }

            GetSetTokenCookie(result.Value.JwtToken, result.Value.RefreshToken);
            await SendAsync(ApiResponse<AuthResponse>.SuccessResponse(
                result.Value), (int)HttpStatusCode.OK, ct);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void GetSetTokenCookie(string accessToken, string refreshToken = "")
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
            SameSite = SameSiteMode.None,
            Secure = true
        };
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(TokenConstants.RefreshToken, refreshToken,
            cookieOptions);

        _httpContextAccessor.HttpContext!.Response.Cookies.Append(TokenConstants.AccessToken, accessToken,
            cookieOptions);
    }
}