using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FDBInsights.Data;
using FDBInsights.Dto;
using FDBInsights.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FDBInsights.Repositories.Implementation;

public class JwtRepository(ApplicationDbContext dbContext, IOptions<AppSettings> appSettings) : IJwtRepository
{
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task<string> GenerateJwtTokenAsync(UserInfo userInfo, CancellationToken cancellationToken = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var claimList = new List<Claim>
        {
            new(ClaimTypes.Email, userInfo.GetUserEmail),
            new(ClaimTypes.NameIdentifier, userInfo.GetUserId.ToString()),
            new(ClaimTypes.Name, userInfo.GetUserEmail)
        };

        claimList.AddRange(userInfo.GetUserRoles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _appSettings.Audience,
            Issuer = _appSettings.Issuer,
            Expires = DateTime.UtcNow.AddMinutes(_appSettings.TokenExpirationInMinutes),
            NotBefore = DateTime.UtcNow,
            Subject = new ClaimsIdentity(claimList),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }


    public Task<RefreshToken> GenerateRefreshTokenAsync(UserInfo userInfo,
        CancellationToken cancellationToken = default)
    {
        var refreshToken = new RefreshToken
        {
            // token is a cryptographically strong random sequence of values
            Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
            // token is valid for 7 days
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = userInfo.GetUserId.ToString()
        };

        // ensure token is unique by checking against db
        /* To Do */

        return Task.FromResult(refreshToken);
    }

    public ClaimsPrincipal? GetClaimsPrincipalFromToken(string token)
    {
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
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
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            return null;
        return principal;
    }

    public bool IsTokenExpired(string token)
    {
        var jwthandler = new JwtSecurityTokenHandler();
        var jwttoken = jwthandler.ReadToken(token);
        var expDate = jwttoken.ValidTo;
        if (expDate < DateTime.UtcNow)
            return true;
        return false;
    }
}