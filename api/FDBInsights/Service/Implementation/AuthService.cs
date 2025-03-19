using System.Linq.Expressions;
using System.Security.Claims;
using ErrorOr;
using FDBInsights.Common.Helper;
using FDBInsights.Data;
using FDBInsights.Dto;
using FDBInsights.Features.Auth.Commands;
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

    public Task<User?> GetByEmailAsync(string email)
    {
        return _userRepository.GetByEmailAsync(email);
    }

    public async Task<ErrorOr<AuthResponse>> GetByUserNameAsync(string userName, string password,
        CancellationToken cancellationToken = default)
    {
        Expression<Func<User, bool>> predicate = user => user.UserName == userName;
        var userPredicate = PredicateBuilder.New<User>().And(predicate);
        Expression<Func<User, UserInfo>> selector = user => new UserInfo(
            user.UserID,
            user.Email,
            user.UserName,
            user.Password,
            user.RoleID, null, null);
        var user = await _userRepository.GetByPropertyAsync(userPredicate, selector, cancellationToken);
        if (user == null) return Error.NotFound("UserNotFound", "User not found");
        if (user.GetUserPassword != Encrypter.OneWayEncrypter(password))
            return Error.Validation("UserPasswordMismatch", "User password mismatch");
        if (string.IsNullOrEmpty(user.GetUserRolesString)) return Error.Forbidden("NoRoles", "User has no roles");
        var roleIds = user.GetUserRolesString.Split(',')
            .Select(int.Parse)
            .ToList();
        Expression<Func<UserRole, bool>> rolePredicate = r => roleIds.Contains(r.RoleID);
        Expression<Func<UserRole, UserRoleInfo>> roleSelector = r => new UserRoleInfo
        {
            ID = r.RoleID,
            Name = r.Name
        };
        var userRoles =
            await _roleRepository.GetAllAsync(rolePredicate, roleSelector,
                cancellationToken: cancellationToken);
        user.UpdateRoles(userRoles);
        var jwtToken = await _jwtRepository.GenerateJwtTokenAsync(user, cancellationToken);
        var refreshToken = await _jwtRepository.GenerateRefreshTokenAsync(user, cancellationToken);
        return new AuthResponse(jwtToken, refreshToken.Token);
    }

    public CurrentUser GetUserFromClaims(IEnumerable<Claim> claims)
    {
        var user = new CurrentUser();
        var roles = new List<string>();
        foreach (var c in claims)
            switch (c.Type)
            {
                case ClaimTypes.NameIdentifier:
                    user.ID = c.Value;
                    break;
                case ClaimTypes.Name:
                    user.UserName = c.Value;
                    break;
                case ClaimTypes.Role:
                    roles.Add(c.Value);
                    break;
                case ClaimTypes.Email:
                    user.Email = c.Value;
                    break;
            }

        user.Roles = roles;
        return user;
    }

    // public async Task<AuthResponse> RefreshToken(string token, string ipAddress)
    // {
    //     var applicationUser = await getAccountByRefreshToken(token);
    //     var refreshToken = applicationUser.RefreshTokens.Single(x => x.Token == token);
    //     // replace old refresh token with a new one (rotate token)
    //     var newRefreshToken = await rotateRefreshToken(refreshToken, ipAddress);
    //     applicationUser.RefreshTokens.Add(newRefreshToken);
    //
    //     // remove old refresh tokens from account
    //     await removeOldRefreshTokens(applicationUser);
    //
    //     // save changes to db
    //     _context.Update(applicationUser);
    //     await _context.SaveChangesAsync();
    //
    //     // generate new jwt
    //     var roles = await _userManager.GetRolesAsync(applicationUser);
    //     var jwtToken = _jwtTokenGenerator.GenerateJwtToken(applicationUser, roles);
    //
    //     // return data in authenticate response object
    //     var response = _mapper.Map<AuthResponse>(applicationUser);
    //     response.JwtToken = jwtToken;
    //     response.RefreshToken = newRefreshToken.Token;
    //     response.Authenticated = true;
    //     return response;
    // }
}