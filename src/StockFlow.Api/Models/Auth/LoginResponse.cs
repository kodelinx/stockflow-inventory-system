namespace StockFlow.Api.Models.Auth;

public class LoginResponse
{
    public int UserId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    // JWT token used by the client for authenticated API requests.
    public string Token { get; set; } = string.Empty;
    
    // Indicates when the JWT token is no longer valid.
    public DateTime ExpiresAt { get; set; }
}