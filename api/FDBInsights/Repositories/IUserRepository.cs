using FDBInsights.Dto;
using FDBInsights.Models;

namespace FDBInsights.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<UserInfo?> GetByUserNameAsync(string userName);
}