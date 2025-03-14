using FDBInsights.Models;

namespace FDBInsights.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}