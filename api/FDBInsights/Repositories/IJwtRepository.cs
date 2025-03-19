using System.Security.Claims;
using FDBInsights.Dto;
using FDBInsights.Models;

namespace FDBInsights.Repositories;

public interface IJwtRepository
{
    Task<string> GenerateJwtTokenAsync(UserInfo userInfo, CancellationToken cancellationToken = default);
    Task<RefreshToken> GenerateRefreshTokenAsync(UserInfo userInfo, CancellationToken cancellationToken = default);
    ClaimsPrincipal? GetClaimsPrincipalFromToken(string token);
    bool IsTokenExpired(string token);
}