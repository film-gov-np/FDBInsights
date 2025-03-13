using System.Linq.Expressions;
using FDBInsights.Common.Helper;
using FDBInsights.Data;
using FDBInsights.Dto;
using FDBInsights.Features;
using FDBInsights.Models;
using FDBInsights.Repositories;
using LinqKit;

namespace FDBInsights.Service.Implementation;

public class AuthService(
    IJwtRepository jwtRepository,
    ApplicationDbContext dbContext,
    IUserRepository userRepository,
    IRoleRepository roleRepository)
    : IAuthService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IJwtRepository _jwtRepository = jwtRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<AuthResponse?> GetByUserNameAsync(string userName, string password,
        CancellationToken cancellationToken = default)
    {
        Expression<Func<User, bool>> predicate = user => user.UserName == userName;
        var a = PredicateBuilder.New<User>().And(predicate);
        Expression<Func<User, UserInfo>> selector = user => new UserInfo(
            user.UserID,
            user.Email,
            user.UserName,
            user.Password,
            user.RoleID, null, null);
        var user = await _userRepository.GetByPropertyAsync(a, selector, cancellationToken);
        if (user == null || user.GetUserPassword != Encrypter.OneWayEncrypter(password)) return null;
        if (string.IsNullOrEmpty(user.GetUserRolesString)) return new AuthResponse("", "");
        var roleIds = user.GetUserRolesString.Split(',')
            .Select(int.Parse)
            .ToList();
        Expression<Func<UserRole, bool>> rolePredicate = r => roleIds.Contains(r.RoleID);
        Expression<Func<UserRole, UserRoleInfo>> roleSelector = r => new UserRoleInfo
        {
            ID = r.RoleID,
            Name = r.Name
        };
        var userRoles = await _roleRepository.GetAllAsync(rolePredicate, roleSelector, null, null, cancellationToken);
        user.UpdateRoles(userRoles);
        var jwtToken = await _jwtRepository.GenerateJwtTokenAsync(user, cancellationToken);
        var refreshToken = await _jwtRepository.GenerateRefreshTokenAsync(user, cancellationToken);
        return new AuthResponse(jwtToken, refreshToken.Token);
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return _userRepository.GetByEmailAsync(email);
    }
}