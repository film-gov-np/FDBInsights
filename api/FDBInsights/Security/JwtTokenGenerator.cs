using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FDBInsights.Data;
using FDBInsights.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FDBInsights.Security;

public class JwtTokenGenerator(ApplicationDbContext dbContext, JwtSettings jwtSettings, UserManager<User> userManager)
    : IJwtTokenGenerator
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly JwtSettings _jwtSettings = jwtSettings;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<string> GenerateJwtTokenAsync(User user, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var claimList = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new(ClaimTypes.Name, user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(user);
        claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDecriptor = new SecurityTokenDescriptor
        {
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            Subject = new ClaimsIdentity(claimList),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDecriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(User user, CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken
        {
            UserId = user.UserID.ToString(),
            Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationInDays),
            Created = DateTime.UtcNow
        };
        //ensure token is unique by checking against existing tokens
        // var tokenIsUnique = !_context.Users.Any(a => a.RefreshTokens.Any(t => t.Token == refreshToken.Token));
        //
        // if (!tokenIsUnique)
        //     return await GenerateRefreshToken(ipAddress);

        return refreshToken;
    }

    public ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
    {
        try
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience =
                    false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                return null;
            return principal;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool IsValidToken(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;
        var expiry = jwtToken?.ValidTo;
        if (expiry < DateTime.UtcNow) return true;
        return false;
    }
}