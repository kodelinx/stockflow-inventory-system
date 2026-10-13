using Microsoft.AspNetCore.Identity;
using StockFlow.Models;

namespace StockFlow.Api.Services;

public class PasswordService
{
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
        PasswordVerificationResult result =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password
            );

        return result !=
            PasswordVerificationResult.Failed;
    }
}