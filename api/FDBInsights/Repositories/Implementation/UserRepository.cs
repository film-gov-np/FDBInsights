using FDBInsights.Data;
using FDBInsights.Dto;
using FDBInsights.Models;
using Microsoft.EntityFrameworkCore;

namespace FDBInsights.Repositories.Implementation;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return null;
        return user;
    }

    public async Task<UserInfo?> GetByUserNameAsync(string userName)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null) return null;
        var userRoles = new List<string>();
        if (!string.IsNullOrEmpty(user.RoleID))
        {
            var roleIds = user.RoleID.Split(',').Select(int.Parse).ToList();
            userRoles = await _dbContext.UserRole
                .AsNoTracking()
                .Where(role => roleIds.Contains(role.RoleID))
                .Select(role => role.Name)
                .ToListAsync();
        }

        return new UserInfo(user.UserID, user.Email, user.FullName, user.Password, userRoles);
    }
}