using FDBInsights.Features;
using FDBInsights.Models;

namespace FDBInsights.Service;

public interface IAuthService
{
    Task<User?> GetByEmailAsync(string email);

    Task<AuthResponse?> GetByUserNameAsync(string userName, string password,
        CancellationToken cancellationToken = default);
}