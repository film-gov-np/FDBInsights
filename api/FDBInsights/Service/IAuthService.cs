using FDBInsights.Dto;
using FDBInsights.Features.Auth;

namespace FDBInsights.Service;

public interface IAuthService
{
    Task<UserInfo?> GetByEmailAsync(string email);
    Task<AuthResponse?> GetByUserNameAsync(string userName, string password, CancellationToken cancellationToken);
}