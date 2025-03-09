namespace FDBInsights.Dto;

public class UserInfo(string email, string fullName, string jwtToken)
{
    private string Email { get; } = email;
    private string FullName { get; } = fullName;
    private string JwtToken { get; } = jwtToken;
    public string GetUserEmail => Email;

    public string GetUserFullName => FullName;
    public string? GetJwtToken => JwtToken;
}