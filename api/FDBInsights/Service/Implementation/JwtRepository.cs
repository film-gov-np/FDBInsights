using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FDBInsights.Data;
using FDBInsights.Dto;
using FDBInsights.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FDBInsights.Service.Implementation;

public class JwtRepository(ApplicationDbContext dbContext, IOptions<AppSettings> appSettings) : IJwtRepository
{
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<string> GenerateJwtTokenAsync(UserInfo userInfo, CancellationToken cancellationToken = default)
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
            Subject = new ClaimsIdentity(claimList),
            Expires = DateTime.Now.AddMinutes(_appSettings.ExpiresInMinutes),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    public async Task<RefreshToken> GenerateRefreshTokenAsync(UserInfo userInfo,
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

        return refreshToken;
    }
}