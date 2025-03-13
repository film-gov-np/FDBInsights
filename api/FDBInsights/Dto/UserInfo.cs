namespace FDBInsights.Dto;

public class UserInfo(
    int id,
    string email,
    string fullName,
    string password,
    string? rolesString = null,
    List<UserRoleInfo>? roles = null,
    string? jwtToken = null
)
{
    private int Id { get; } = id;
    private string Email { get; } = email;
    private string FullName { get; } = fullName;
    private string Password { get; } = password;
    private List<UserRoleInfo> Roles { get; set; } = roles ?? new List<UserRoleInfo>();
    private string? JwtToken { get; set; } = jwtToken;
    private string? RolesString { get; } = rolesString;
    public string GetUserEmail => Email;

    public string GetUserFullName => FullName;
    public List<UserRoleInfo> GetUserRoles => Roles;
    public string GetUserPassword => Password;
    public string? GetUserJwtToken => JwtToken;
    public int GetUserId => Id;
    public string? GetUserRolesString => RolesString;

    public void UpdateJwtToken(string token)
    {
        JwtToken = token;
    }

    public void UpdateRoles(List<UserRoleInfo> roles)
    {
        Roles = roles;
    }
}