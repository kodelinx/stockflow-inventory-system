using StockFlow.Models;
using StockFlow.Repositories;

namespace StockFlow.Api.Services;

public class DevelopmentUserSeeder
{
    private readonly UserRepository _userRepository;

    private readonly PasswordService _passwordService;

    private readonly IConfiguration _configuration;

    public DevelopmentUserSeeder(
        UserRepository userRepository,
        PasswordService passwordService,
        IConfiguration configuration
    )
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _configuration = configuration;
    }

    public void SeedAdmin()
    {
        string username =
            _configuration[
                "SeedAdmin:Username"
            ]
            ?? "admin";

        User? existingUser =
            _userRepository
                .FindUserByUsername(username);

        if (existingUser != null)
        {
            return;
        }

        string? password =
            _configuration[
                "SeedAdmin:Password"
            ];

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine(
                "Development admin was not seeded because SeedAdmin:Password is not configured."
            );

            return;
        }

        string fullName =
            _configuration[
                "SeedAdmin:FullName"
            ]
            ?? "StockFlow Administrator";

        User admin = new User
        {
            Username = username,
            FullName = fullName,
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        admin.PasswordHash =
            _passwordService.HashPassword(
                admin,
                password
            );

        _userRepository.AddUser(admin);

        Console.WriteLine(
            $"Development admin '{username}' created."
        );
    }
}