using FDBInsights.Dto;

namespace FDBInsights.Common;

public interface IBaseEndpoint
{
    UserInfo? CurrentUser { get; }
    string? CurrentUserEmail { get; }
    string? CurrentUserFullName { get; }
    string? CurrentUserToken { get; }
    string HostUrl { get; }
}