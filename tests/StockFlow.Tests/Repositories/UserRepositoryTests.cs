using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Tests.Helpers;

namespace StockFlow.Tests.Repositories;

public class UserRepositoryTests
{
    [Fact]
    public void AddUser_ValidUser_PersistsUser()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        UserRepository userRepository =
            new UserRepository(
                testDatabase.DatabaseConnectionService
            );

        User user = new User
        {
            Username = "admin",
            PasswordHash = "test-hash",
            FullName = "StockFlow Administrator",
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        int userId =
            userRepository.AddUser(user);

        User? savedUser =
            userRepository.FindUserByUsername(
                "admin"
            );

        Assert.NotNull(savedUser);

        Assert.Equal(
            userId,
            savedUser.UserId
        );

        Assert.Equal(
            "admin",
            savedUser.Username
        );

        Assert.Equal(
            "test-hash",
            savedUser.PasswordHash
        );

        Assert.Equal(
            "StockFlow Administrator",
            savedUser.FullName
        );

        Assert.Equal(
            "Admin",
            savedUser.Role
        );

        Assert.True(
            savedUser.IsActive
        );
    }

    [Fact]
    public void GetAllUsers_MultipleUsers_ReturnsAllUsers()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        UserRepository userRepository =
            new UserRepository(
                testDatabase.DatabaseConnectionService
            );

        User admin = new User
        {
            Username = "admin",
            PasswordHash = "admin-hash",
            FullName = "Administrator",
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        User staff = new User
        {
            Username = "staff",
            PasswordHash = "staff-hash",
            FullName = "Staff User",
            Role = "Staff",
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        userRepository.AddUser(admin);
        userRepository.AddUser(staff);

        List<User> users =
            userRepository.GetAllUsers();

        Assert.Equal(
            2,
            users.Count
        );
    }

    [Fact]
    public void FindUserByUsername_ExistingUsername_ReturnsUser()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        UserRepository userRepository =
            new UserRepository(
                testDatabase.DatabaseConnectionService
            );

        User user = new User
        {
            Username = "staff",
            PasswordHash = "staff-hash",
            FullName = "Staff User",
            Role = "Staff",
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        userRepository.AddUser(user);

        User? result =
            userRepository.FindUserByUsername(
                "staff"
            );

        Assert.NotNull(result);

        Assert.Equal(
            "staff",
            result.Username
        );
    }

    [Fact]
    public void FindUserByUsername_MissingUsername_ReturnsNull()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        UserRepository userRepository =
            new UserRepository(
                testDatabase.DatabaseConnectionService
            );

        User? result =
            userRepository.FindUserByUsername(
                "missing-user"
            );

        Assert.Null(result);
    }

    [Fact]
    public void UpdateUserRole_ExistingUser_UpdatesRole()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        UserRepository userRepository =
            new UserRepository(
                testDatabase.DatabaseConnectionService
            );

        User user = new User
        {
            Username = "staff",
            PasswordHash = "staff-hash",
            FullName = "Staff User",
            Role = "Staff",
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        userRepository.AddUser(user);

        userRepository.UpdateUserRole(
            "staff",
            "Admin"
        );

        User? updatedUser =
            userRepository.FindUserByUsername(
                "staff"
            );

        Assert.NotNull(updatedUser);

        Assert.Equal(
            "Admin",
            updatedUser.Role
        );
    }

    [Fact]
    public void UpdateUserActiveStatus_ExistingUser_DeactivatesUser()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        UserRepository userRepository =
            new UserRepository(
                testDatabase.DatabaseConnectionService
            );

        User user = new User
        {
            Username = "staff",
            PasswordHash = "staff-hash",
            FullName = "Staff User",
            Role = "Staff",
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        userRepository.AddUser(user);

        userRepository.UpdateUserActiveStatus(
            "staff",
            false
        );

        User? updatedUser =
            userRepository.FindUserByUsername(
                "staff"
            );

        Assert.NotNull(updatedUser);

        Assert.False(
            updatedUser.IsActive
        );
    }
}