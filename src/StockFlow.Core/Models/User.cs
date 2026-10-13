namespace StockFlow.Models;

public class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public User()
    {
    }

    public User(
        int userId,
        string username,
        string passwordHash,
        string fullName,
        string role,
        bool isActive,
        DateTime createdAt
    )
    {
        UserId = userId;
        Username = username;
        PasswordHash = passwordHash;
        FullName = fullName;
        Role = role;
        IsActive = isActive;
        CreatedAt = createdAt;
    }
}