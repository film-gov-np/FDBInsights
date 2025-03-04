using FDBInsights.Common;

namespace FDBInsights.Models;

public class User : BaseEntity
{
    public User(string username, string email, string passwordHash, string role, DateTime? lastLogin)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        LastLogin = lastLogin;
    }

    private string Username { get; }
    private string Email { get; }
    private string PasswordHash { get; }
    private string Role { get; }
    private DateTime? LastLogin { get; }
}