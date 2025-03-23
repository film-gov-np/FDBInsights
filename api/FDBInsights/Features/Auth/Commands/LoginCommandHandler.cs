using ErrorOr;
using FDBInsights.Common;
using FDBInsights.Constants;
using FDBInsights.Service;
using MediatR;

namespace FDBInsights.Features.Auth.Commands;

public class LoginCommandHandler(IAuthService authService, IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<AuthCommand, ErrorOr<ApiResponse<AuthResponse>>>
{
    private readonly IAuthService _authService = authService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<ErrorOr<ApiResponse<AuthResponse>>> Handle(AuthCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.GetByUserNameAsync(request.Username, request.Password, cancellationToken);
        if (result.IsError) return ApiResponse<AuthResponse>.ErrorResponse(result.Errors.First().Description);
        GetSetTokenCookie(result.Value);
        return ApiResponse<AuthResponse>.SuccessResponse(result.Value);
    }

    private void GetSetTokenCookie(AuthResponse response)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
            SameSite = SameSiteMode.None,
            Secure = true
        };
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(TokenConstants.RefreshToken, response.RefreshToken,
            cookieOptions);

        _httpContextAccessor.HttpContext!.Response.Cookies.Append(TokenConstants.AccessToken, response.AccessToken,
            cookieOptions);
    }
}