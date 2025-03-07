using FastEndpoints;
using FDBInsights.Dto;
using FDBInsights.Features.Auth.Query;
using FDBInsights.Service;

namespace FDBInsights.Features.Auth;

public class AuthQueryHandler(IUserService userService) : Endpoint<AuthRequest, UserInfo>
{
    private readonly IUserService _userService = userService;

    public override void Configure()
    {
        Post("/auth/login");
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
        var user = await _userService.GetByEmailAsync(req.username);
        // Return response
        await SendAsync(new UserInfo(user.GetUserEmail, user.GetUserFullName),
            cancellation: ct);
    }
}