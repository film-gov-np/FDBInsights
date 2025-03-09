using FDBInsights.Dto;
using FDBInsights.Repositories;

namespace FDBInsights.Service.Implementation;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public Task<UserInfo?> GetByEmailAsync(string email)
    {
        return _userRepository.GetByEmailAsync(email);
    }

    public Task<UserInfo?> GetByUserNameAsync(string userName)
    {
        return _userRepository.GetByUserNameAsync(userName);
    }
}