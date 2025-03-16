using ErrorOr;
using FDBInsights.Features;
using FDBInsights.Features.Auth;
using FDBInsights.Models;

namespace FDBInsights.Service;

public interface IAuthService
{
    Task<User?> GetByEmailAsync(string email);

    Task<ErrorOr<AuthResponse>> GetByUserNameAsync(string userName, string password,
        CancellationToken cancellationToken = default);
}