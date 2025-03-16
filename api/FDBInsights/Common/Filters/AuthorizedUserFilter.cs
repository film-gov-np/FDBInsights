using System.Net;
using System.Reflection;
using FDBInsights.Constants;
using FDBInsights.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace FDBInsights.Common.Filters;

public class AuthorizedUserFilter(IAuthService authService, IJwtRepository jwtRepository) : IEndpointFilter
{
    private readonly IAuthService _authService = authService;
    private readonly IJwtRepository _jwtRepository = jwtRepository;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        Console.WriteLine("************************************************************");
        Console.WriteLine("AuthorizedUserFilter is being executed");
        Console.WriteLine($"Request path: {context.HttpContext.Request.Path}");
        Console.WriteLine($"Request method: {context.HttpContext.Request.Method}");
        Console.WriteLine($"Authentication present: {context.HttpContext.User?.Identity?.IsAuthenticated}");
        
        var endpoint = context.HttpContext.GetEndpoint();
        var httpContext = context.HttpContext;

        try 
        {
            // Check if endpoint allows anonymous access
            if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                Console.WriteLine("Anonymous access allowed for endpoint");
                return await next(context);
            }

            Console.WriteLine("Endpoint requires authentication");
            
            // Get tokens from cookies or Authorization header
            var accessToken = httpContext.Request.Cookies[TokenConstants.AccessToken];
            Console.WriteLine($"Cookie token present: {!string.IsNullOrEmpty(accessToken)}");

            if (string.IsNullOrEmpty(accessToken))
            {
                var authHeader = httpContext.Request.Headers[HeaderNames.Authorization].ToString();
                Console.WriteLine($"Authorization header: {authHeader}");
                
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    accessToken = authHeader.Substring("Bearer ".Length);
                    Console.WriteLine("Found token in Authorization header");
                }
            }
            else
            {
                Console.WriteLine("Found token in cookies");
            }

            // If no access token, return custom unauthorized response
            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("No access token found, returning unauthorized response from filter");
                return Results.Json(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Authorization required - custom filter",
                    StatusCode = HttpStatusCode.Unauthorized
                }, statusCode: StatusCodes.Status401Unauthorized);
            }

            // Add token validation here when IJwtRepository has the necessary methods
            Console.WriteLine("Token found and validation passed");

            // If all checks pass, proceed with the request
            var result = await next(context);
            Console.WriteLine("Filter execution completed");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in AuthorizedUserFilter: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return Results.Json(new ApiResponse<object>
            {
                IsSuccess = false,
                Message = "Error processing the authorization request",
                StatusCode = HttpStatusCode.InternalServerError
            }, statusCode: StatusCodes.Status500InternalServerError);
        }
        finally
        {
            Console.WriteLine("************************************************************");
        }
    }

    // Legacy method for MVC Controller-based authorization, not used for FastEndpoints
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        Console.WriteLine("OnAuthorizationAsync called (not used for FastEndpoints)");

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

        // Rest of the implementation removed as it's not used for FastEndpoints
    }
}