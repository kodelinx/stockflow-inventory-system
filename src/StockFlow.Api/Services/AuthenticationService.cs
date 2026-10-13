using StockFlow.Api.Models.Auth;
using StockFlow.Models;
using StockFlow.Repositories;

namespace StockFlow.Api.Services;

public class AuthenticationService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordService _passwordService;
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

        (
            string token,
            DateTime expiresAt
        ) = _jwtTokenService.CreateToken(user);

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