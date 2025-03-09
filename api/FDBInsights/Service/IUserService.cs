using FDBInsights.Dto;

namespace FDBInsights.Service;

public interface IUserService
{
    Task<UserInfo?> GetByEmailAsync(string email);
    Task<UserInfo?> GetByUserNameAsync(string userName);
}