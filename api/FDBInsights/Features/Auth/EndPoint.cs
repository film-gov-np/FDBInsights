using FDBInsights.Common;
using FDBInsights.Dto;
using FDBInsights.Service;

namespace FDBInsights.Features.Auth;

public class AuthEndpoint(IAuthService authService, BaseEndpointCore baseEndpointCore)
    : BaseEndpoint<AuthRequest, UserInfo>(baseEndpointCore)
{
    private readonly IAuthService _authService = authService;
    private readonly BaseEndpointCore _baseEndpointCore = baseEndpointCore;

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
            var user = await _authService.GetByUserNameAsync(req.username, req.password, ct);
            if (user == null) await SendNotFoundAsync("User not found", ct);

            // await SendAsync(new UserInfo(user.GetUserEmail ?? "", user.GetUserFullName ?? "", "token"),
            //     cancellation: ct);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}