using Microsoft.AspNetCore.Identity;
using StockFlow.Models;

namespace StockFlow.Api.Services;

public class PasswordService
{
    // ASP.NET Core helper that securely hashes and verifies passwords.
    private readonly PasswordHasher<User> _passwordHasher;

    public PasswordService()
    {
        _passwordHasher = new PasswordHasher<User>();
    }

    public string HashPassword(
        User user,
        string password
    )
    {
        return _passwordHasher.HashPassword(
            user,
            password
        );
    }

    public bool VerifyPassword(
        User user,
        string password
    )
    {
        // Compares the entered password against the saved password hash.
        PasswordVerificationResult result =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password
            );

        // Any result other than Failed means the password is valid.
        return result !=
            PasswordVerificationResult.Failed;
    }
}