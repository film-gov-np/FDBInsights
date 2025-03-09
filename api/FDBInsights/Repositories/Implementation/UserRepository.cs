using FDBInsights.Data;
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

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserName == userName)
            .FirstOrDefaultAsync();
        if (user == null) return null;
        return user;
    }
}