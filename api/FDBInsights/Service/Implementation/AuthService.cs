using System.Text;
using FDBInsights.Dto;
using FDBInsights.Features.Auth;
using FDBInsights.Models;
using FDBInsights.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FDBInsights.Service.Implementation;

public class AuthService : IAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;

    public AuthService(
        IUserRepository userRepository,
        SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<UserInfo?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return new UserInfo(user?.Email ?? "", user?.UserName ?? "", string.Empty);
    }

    public async Task<AuthResponse?> GetByUserNameAsync(string userName, string password,
        CancellationToken cancellationToken)
    {
        // Find the user by username
        var user = await _userRepository.GetByUserNameAsync(userName);
        if (user == null) return null;

        // Use SignInManager to check password (it handles all password validation and hashing)
        var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

        if (result.Succeeded)
        {
            // For now, return a simple token. In a real app, you'd generate a proper JWT
            var token = GenerateSimpleToken(user.UserID.ToString());
            return new AuthResponse(token, token);
        }

        return null;
    }

    private string GenerateSimpleToken(string userId)
    {
        // This is just a placeholder - not for production use
        var tokenData = Encoding.UTF8.GetBytes($"{userId}:{DateTime.UtcNow.Ticks}");
        return Convert.ToBase64String(tokenData);
    }
}