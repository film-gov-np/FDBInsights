namespace FDBInsights.Dto;

public class UserInfo(
    int id,
    string email,
    string fullName,
    string password,
    List<string>? roles = null,
    string? jwtToken = null)
{
    private int Id { get; } = id;
    private string Email { get; } = email;
    private string FullName { get; } = fullName;
    private string Password { get; } = password;
    private List<string> Roles { get; } = roles ?? new List<string>();
    private string? JwtToken { get; set; } = jwtToken;
    public string GetUserEmail => Email;

    public string GetUserFullName => FullName;
    public List<string> GetUserRoles => Roles;
    public string GetUserPassword => Password;
    public string? GetUserJwtToken => JwtToken;
    public int GetUserId => Id;

    public void UpdateJwtToken(string token)
    {
        JwtToken = token;
    }
}