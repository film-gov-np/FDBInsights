using System.Security.Claims;
using FDBInsights.Common.Filter;
using FDBInsights.Constants;
using FDBInsights.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FDBInsights.Controllers;

[TypeFilter(typeof(AuthorizedUserFilter))]
public class AuthorizedController : ControllerBase
{
    private string? GetUserRoles =>
        //Since we are using ASP.NET Identity,
        //user claims are typically stored in the authentication cookie,
        //and ASP.NET Identity middleware automatically populates HttpContext.
        //User with these claims during the authentication process. 
        User.FindFirst(ClaimTypes.Role)?.Value;

    public bool IsAdmin
    {
        get
        {
            var usersRoles = GetUserRoles?.Split(',').ToArray();
            return usersRoles != null && (usersRoles.Contains(AuthorizationConstants.SuperUserRole) ||
                                          usersRoles.Contains(AuthorizationConstants.AdminRole));
        }
    }

    public string? GetUserName
    {
        get
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            return userName;
        }
    }

    public string? GetUserEmail
    {
        get
        {
            var userName = User.FindFirst(ClaimTypes.Email)?.Value;
            return userName;
        }
    }

    public string? GetUserId
    {
        get
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
    }

    public string HostUrl => $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

    public CurrentUser? CurrentUser => (CurrentUser)HttpContext.Items["CurrentUser"]!;
}