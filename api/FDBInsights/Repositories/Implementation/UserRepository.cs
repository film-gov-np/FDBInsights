using FDBInsights.Data;
using FDBInsights.Models;
using Microsoft.EntityFrameworkCore;

namespace FDBInsights.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    protected readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext _dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.GetEmail.ToLower() == "ccmsfdb@gmail.com".ToLower());
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.GetUsername.ToLower() == username.ToLower());
    }
}