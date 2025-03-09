using System.Security.Cryptography;
using System.Text;
using ErrorOr;
using FastEndpoints;
using FDBInsights.Dto;
using FDBInsights.Service;

//using FDBInsights.Features.Auth.Query;

namespace FDBInsights.Features.Auth;

public class AuthQueryHandler(IAuthService authService) : Endpoint<AuthRequest, ErrorOr<UserInfo>>
{
    private readonly IAuthService _authService = authService;

    public override void Configure()
    {
        Post("/auth/loginsss");
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
        var passCode = OneWayEncryter(req.password);
        var user = await _authService.GetByEmailAsync(req.username);
        // Return response
        await SendAsync(new UserInfo(user.GetUserEmail, user.GetUserFullName, "sdfs"),
            cancellation: ct);
    }

    public static string OneWayEncryter(string textToBeEncrypted)
    {
        if (string.IsNullOrEmpty(textToBeEncrypted))
            return string.Empty;
        var hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(textToBeEncrypted));
        var stringBuilder = new StringBuilder();
        for (var index = 0; index < hash.Length; ++index)
            stringBuilder.Append(hash[index].ToString("X2"));
        return stringBuilder.ToString();
    }
}