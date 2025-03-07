using FDBInsights.Dto;

namespace FDBInsights.Repositories;

public interface IUserRepository
{
    Task<UserInfo?> GetByEmailAsync(string email);
}