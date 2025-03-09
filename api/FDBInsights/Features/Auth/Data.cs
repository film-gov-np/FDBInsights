using FDBInsights.Data;
using FDBInsights.Models;
using Microsoft.EntityFrameworkCore;

public class UserData
{
    private readonly ApplicationDbContext _dbContext;

    internal Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    internal Task<User?> GetUserByUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}