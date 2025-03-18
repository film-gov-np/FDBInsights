using System.Net;
using System.Reflection;
using FDBInsights.Constants;
using FDBInsights.Features.Auth.Commands;
using FDBInsights.Repositories;
using FDBInsights.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace FDBInsights.Common.Filter;

public class AuthorizedUserFilter(IJwtRepository jwtRepository, IAuthService authService) : IAsyncAuthorizationFilter
{
    private readonly IAuthService _authService = authService;
    private readonly IJwtRepository _jwtRepository = jwtRepository;


    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

        if (controllerActionDescriptor != null)
        {
            var controllerAllowAnonymousAttribute = controllerActionDescriptor.ControllerTypeInfo
                .GetCustomAttribute<AllowAnonymousAttribute>();
            var actionAllowAnonymousAttribute =
                controllerActionDescriptor.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>();

            // Allow anonymous access, proceed with the request without further authorization checks
            if (controllerAllowAnonymousAttribute != null || actionAllowAnonymousAttribute != null) return;
        }

        var accessToken = context.HttpContext.Request.Cookies[TokenConstants.AccessToken];
        var refreshToken = context.HttpContext.Request.Cookies[TokenConstants.RefreshToken];
        var origin = context.HttpContext.Request.Headers.Origin.ToString();

        if (string.IsNullOrEmpty(accessToken))
            accessToken =
                context.HttpContext.Request?.Headers[HeaderNames.Authorization].ToString()
                    .Replace("Bearer ", string.Empty) ?? string.Empty;

        if (!string.IsNullOrEmpty(accessToken))
        {
            if (!_jwtRepository.IsTokenExpired(accessToken))
            {
                var claimPrincipal = _jwtRepository.GetClaimsPrincipalFromToken(accessToken);
                if (claimPrincipal == null)
                    context.Result = new UnauthorizedObjectResult(
                        ApiResponse<string>.ErrorResponse("Invalid Access Token.", HttpStatusCode.NotAcceptable));
                var claimsIdentity = claimPrincipal.Claims.FirstOrDefault().Subject;
                var user = _authService.GetUserFromClaims(claimsIdentity?.Claims);

                context.HttpContext.Items["CurrentUser"] = user;
                context.HttpContext.Items["origin"] = origin;

                if (controllerActionDescriptor != null)
                {
                    // Check if the controller has the RequiredRoles attribute
                    // RequiredRoles is used along with Authorize attribute 
                    var controllerAuthorizeAttribute = controllerActionDescriptor.ControllerTypeInfo
                        .GetCustomAttribute<RequiredRolesAttribute>();

                    // Check if the action method has the CustomAuthorize attribute
                    var actionAuthorizeAttribute =
                        controllerActionDescriptor.MethodInfo.GetCustomAttribute<RequiredRolesAttribute>();

                    if (controllerAuthorizeAttribute != null || actionAuthorizeAttribute != null)
                    {
                        // Combine roles from controller and action level attributes
                        var controllerRoles = controllerAuthorizeAttribute?.Roles ?? [];
                        var actionRoles = actionAuthorizeAttribute?.Roles ?? [];
                        var requiredRoles = controllerRoles.Concat(actionRoles).Distinct().ToArray();

                        var isSuperuser = user.Roles != null &&
                                          user.Roles.Contains(AuthorizationConstants.SuperUserRole);

                        // Check if the user has any of the required roles
                        var hasRequiredRole =
                            requiredRoles.Any(role => user.Roles != null && user.Roles.Contains(role.Trim())) ||
                            isSuperuser;

                        if (!hasRequiredRole)
                            // If user doesn't have the required role, return unauthorized response
                            context.Result = new UnauthorizedObjectResult(
                                ApiResponse<string>.ErrorResponse(
                                    "Unauthorized access. User does not have the required role.",
                                    HttpStatusCode.Forbidden));
                    }
                }
            }
            else // check for refresh token validity and refresh token if valid else remove the cookie
            {
                var sessionExpired = true;
                AuthResponse refreshResp = null;
                if (!string.IsNullOrWhiteSpace(refreshToken))
                    // refreshResp = await _authService.RefreshToken(refreshToken, string.Empty);
                    // sessionExpired = !(refreshResp?.Authenticated ?? true);
                    refreshResp = new AuthResponse(accessToken, refreshToken);

                if (sessionExpired)
                {
                    context.HttpContext.Response.Cookies.Delete(TokenConstants.AccessToken);
                    context.HttpContext.Response.Cookies.Delete(TokenConstants.RefreshToken);
                    context.Result = new UnauthorizedObjectResult(
                        ApiResponse<string>.ErrorResponse("Expired Access Token.", HttpStatusCode.Unauthorized));
                }
                else
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddDays(7),
                        SameSite = SameSiteMode.None,
                        Secure = true
                    };
                    context.HttpContext.Response.Cookies.Append(TokenConstants.AccessToken, refreshResp.AccessToken,
                        cookieOptions);

                    context.HttpContext.Response.Cookies.Append(TokenConstants.RefreshToken, refreshResp.RefreshToken,
                        cookieOptions);
                }
            }
        }
        else
        {
            context.Result = new UnauthorizedObjectResult(
                ApiResponse<string>.ErrorResponse("No access token found. Try loggin in again.",
                    HttpStatusCode.NotFound));
        }
    }
}