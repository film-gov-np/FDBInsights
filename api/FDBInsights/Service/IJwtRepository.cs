using FDBInsights.Dto;
using FDBInsights.Models;

namespace FDBInsights.Service;

public interface IJwtRepository
{
    Task<string> GenerateJwtTokenAsync(UserInfo userInfo, CancellationToken cancellationToken = default);
    Task<RefreshToken> GenerateRefreshTokenAsync(UserInfo userInfo, CancellationToken cancellationToken = default);
}