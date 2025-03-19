using System.Security.Claims;
using ErrorOr;
using FDBInsights.Dto;
using FDBInsights.Features.Auth.Commands;
using FDBInsights.Models;

namespace FDBInsights.Service;

public interface IAuthService
{
    Task<User?> GetByEmailAsync(string email);

    Task<ErrorOr<AuthResponse>> GetByUserNameAsync(string userName, string password,
        CancellationToken cancellationToken = default);

    CurrentUser GetUserFromClaims(IEnumerable<Claim> claims);
}