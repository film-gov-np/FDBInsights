using FDBInsights.Common.Helper;
using FDBInsights.Features;
using FDBInsights.Models;
using FDBInsights.Repositories;

namespace FDBInsights.Service.Implementation;

public class AuthService(IUserRepository userRepository, IJwtRepository jwtRepository) : IAuthService
{
    private readonly IJwtRepository _jwtRepository = jwtRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<AuthResponse?> GetByUserNameAsync(string userName, string password,
        CancellationToken cancellationToken = default)
    {
        var userInfo = await _userRepository.GetByUserNameAsync(userName);
        if (userInfo == null)
            return null;
        var jwtToken = await _jwtRepository.GenerateJwtTokenAsync(userInfo, cancellationToken);
        userInfo.UpdateJwtToken(jwtToken);
        var refreshToken = await _jwtRepository.GenerateRefreshTokenAsync(userInfo, cancellationToken);
        if (userInfo.GetUserPassword != Encrypter.OneWayEncrypter(password))
            return null;
        return new AuthResponse(jwtToken, refreshToken.Token);
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return _userRepository.GetByEmailAsync(email);
    }
}