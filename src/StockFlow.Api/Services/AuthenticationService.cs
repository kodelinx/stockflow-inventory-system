using StockFlow.Api.Models.Auth;
using StockFlow.Models;
using StockFlow.Repositories;

namespace StockFlow.Api.Services;

public class AuthenticationService
{
    // Reads persisted user accounts from SQLite.
    private readonly UserRepository _userRepository;

    // Hashes and verifies user passwords.
    private readonly PasswordService _passwordService;

    // Creates JWT tokens after successful authentication.
    private readonly JwtTokenService _jwtTokenService;

    public AuthenticationService(
        UserRepository userRepository,
        PasswordService passwordService,
        JwtTokenService jwtTokenService
    )
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
    }

    public LoginResponse? Login(
        string username,
        string password
    )
    {
        if (
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password)
        )
        {
            return null;
        }

        username = username.Trim();

        User? user =
            _userRepository
                .FindUserByUsername(username);

        if (user == null)
        {
            return null;
        }

        if (!user.IsActive)
        {
            return null;
        }

        bool passwordIsValid =
            _passwordService.VerifyPassword(
                user,
                password
            );

        if (!passwordIsValid)
        {
            return null;
        }
        // Creates the JWT only after all authentication checks succeed.
        (
            string token,
            DateTime expiresAt
        ) = _jwtTokenService.CreateToken(user);

        // Returns only safe identity information and the JWT.
        return new LoginResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role,
            Token = token,
            ExpiresAt = expiresAt
        };
    }
}