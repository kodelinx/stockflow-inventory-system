namespace StockFlow.Api.Security;

// Centralizes policy names used by [Authorize] attributes
// and ASP.NET Core authorization configuration.
public static class AuthorizationPolicies
{
    public const string AdminOnly = "AdminOnly";

    public const string StaffOrAdmin = "StaffOrAdmin";
}