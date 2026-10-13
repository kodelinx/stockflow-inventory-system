namespace StockFlow.Security;

// Centralizes valid StockFlow role names so the same values
// are reused throughout authentication and authorization.
public static class UserRoles
{
    public const string Admin = "Admin";

    public const string Staff = "Staff";
}