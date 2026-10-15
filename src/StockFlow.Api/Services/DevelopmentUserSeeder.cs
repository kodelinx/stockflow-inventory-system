using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Security;

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
        // Reads the development admin username from configuration.
        string username =
            _configuration[
                "SeedAdmin:Username"
            ]
            ?? "admin";

        // Prevents creating the same admin account more than once.
        User? existingUser =
            _userRepository
                .FindUserByUsername(username);

        if (existingUser != null)
        {
            return;
        }

        // The development password is stored in User Secrets, not in source code.
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

        // Creates the initial development administrator account.
        User admin = new User
        {
            Username = username,
            FullName = fullName,
            // Uses the centralized role constant instead of a raw string.
            Role = UserRoles.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Hashes the password before the user is stored in SQLite.
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

    public void SeedStaff()
    {
        // Reads the development Staff username from configuration.
        string username =
            _configuration[
                "SeedStaff:Username"
            ]
            ?? "staff";

        // Prevents the same Staff account from being created again.
        User? existingUser =
            _userRepository
                .FindUserByUsername(username);

        if (existingUser != null)
        {
            return;
        }

        // The Staff password comes from User Secrets,
        // so it is never committed into Git.
        string? password =
            _configuration[
                "SeedStaff:Password"
            ];

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine(
                "Development staff user was not seeded because SeedStaff:Password is not configured."
            );

            return;
        }

        string fullName =
            _configuration[
                "SeedStaff:FullName"
            ]
            ?? "StockFlow Staff";

        User staff = new User
        {
            Username = username,
            FullName = fullName,

            // Uses the centralized Staff role constant.
            Role = UserRoles.Staff,

            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // The plain-text password exists only long enough
        // to create the secure password hash.
        staff.PasswordHash =
            _passwordService.HashPassword(
                staff,
                password
            );

        _userRepository.AddUser(staff);

        Console.WriteLine(
            $"Development staff '{username}' created."
        );
    }
}