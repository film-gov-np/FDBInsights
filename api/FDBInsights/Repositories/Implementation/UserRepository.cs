using FDBInsights.Data;
using FDBInsights.Dto;
using Microsoft.EntityFrameworkCore;

namespace FDBInsights.Repositories.Implementation;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<UserInfo?> GetByEmailAsync(string email)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return null;
        var userInfo = new UserInfo(user.Email, user.FullName);
        return userInfo;
    }
}