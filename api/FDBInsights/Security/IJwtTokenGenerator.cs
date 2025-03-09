using System.Security.Claims;
using FDBInsights.Models;

namespace FDBInsights.Security;

public interface IJwtTokenGenerator
{
    public Task<string> GenerateJwtTokenAsync(User user, CancellationToken cancellationToken);
    public Task<RefreshToken> GenerateRefreshTokenAsync(User user, CancellationToken cancellationToken);
    public ClaimsPrincipal GetClaimsPrincipalFromToken(string token);
    public bool IsValidToken(string token);
}