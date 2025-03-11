using FDBInsights.Common;
using FDBInsights.Service;

namespace FDBInsights.Features.Auth;

public class AuthEndpoint(IAuthService userService, BaseEndpointCore baseEndpointCore)
    : BaseEndpoint<AuthRequest, AuthResponse>(baseEndpointCore)
{
    private readonly BaseEndpointCore _baseEndpointCore = baseEndpointCore;
    private readonly IAuthService _userService = userService;

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
        try
        {
            var authResponse = await _userService.GetByUserNameAsync(req.username, req.password, ct);

            if (authResponse == null)
            {
                await SendNotFoundAsync("User not found", ct);
                return;
            }

            await SendAsync(authResponse,
                cancellation: ct);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}