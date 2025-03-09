using System.Security.Cryptography;
using System.Text;
using FDBInsights.Common;
using FDBInsights.Dto;
using FDBInsights.Service;

namespace FDBInsights.Features.Auth;

public class AuthEndpoint(IUserService userService, BaseEndpointCore baseEndpointCore)
    : BaseEndpoint<AuthRequest, UserInfo>(baseEndpointCore)
{
    private readonly BaseEndpointCore _baseEndpointCore = baseEndpointCore;
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
        try
        {
            var passCode = OneWayEncrypter(req.password);
            var a = await _userService.GetByEmailAsync(req.username);
            var user = await _userService.GetByUserNameAsync(req.username);

            if (user == null)
            {
                await SendNotFoundAsync("User not found", ct);
                return;
            }

            //var token = await _tokenService.GenerateTokenAsync(user);
            await SendAsync(new UserInfo(user.GetUserEmail ?? "", user.GetUserFullName ?? "", "token"),
                cancellation: ct);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // encryption method used to encrypt the password on CCMS
    private static string OneWayEncrypter(string textToBeEncrypted)
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