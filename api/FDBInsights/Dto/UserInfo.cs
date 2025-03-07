namespace FDBInsights.Dto;

public class UserInfo
{
    public UserInfo(string email, string fullName)
    {
        Email = email;
        FullName = fullName;
    }

    private string Email { get; }
    private string FullName { get; }
    public string GetUserEmail => Email;

    public string GetUserFullName => FullName;
}