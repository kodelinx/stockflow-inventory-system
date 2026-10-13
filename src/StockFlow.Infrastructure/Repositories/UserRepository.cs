using Microsoft.Data.Sqlite;
using StockFlow.Database;
using StockFlow.Models;

namespace StockFlow.Repositories;

public class UserRepository
{
    private readonly DatabaseConnectionService _databaseConnectionService;

    public UserRepository(DatabaseConnectionService databaseConnectionService)
    {
        _databaseConnectionService = databaseConnectionService;
    }

    public int AddUser(User user)
    {
        using SqliteConnection connection = new SqliteConnection
        (
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            INSERT INTO Users
            (
                Username,
                PasswordHash,
                FullName,
                Role,
                IsActive,
                CreatedAt
            )
            VALUES
            (
                @Username,
                @PasswordHash,
                @FullName,
                @Role,
                @IsActive,
                @CreatedAt
            )
            RETURNING UserId;
        ";

        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.AddWithValue("@Username",user.Username);
        command.Parameters.AddWithValue("@PasswordHash",user.PasswordHash);
        command.Parameters.AddWithValue("@FullName",user.FullName);
        command.Parameters.AddWithValue("@Role",user.Role);
        command.Parameters.AddWithValue("@IsActive", user.IsActive ? 1 : 0);
        command.Parameters.AddWithValue("@CreatedAt",user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));

        object? result = command.ExecuteScalar();

        if (result == null)
        {
            throw new InvalidOperationException(
                "User was inserted but no UserId was returned."
            );
        }

        return Convert.ToInt32(result);
    }

    public List<User> GetAllUsers()
    {
        List<User> users =
            new List<User>();

        using SqliteConnection connection = new SqliteConnection
        (
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            SELECT
                UserId,
                Username,
                PasswordHash,
                FullName,
                Role,
                IsActive,
                CreatedAt
            FROM Users;
        ";

        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = sql;

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            users.Add(MapReaderToUser(reader));
        }

        return users;
    }

    public User? FindUserByUsername(string username)
    {
        using SqliteConnection connection = new SqliteConnection
        (
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            SELECT
                UserId,
                Username,
                PasswordHash,
                FullName,
                Role,
                IsActive,
                CreatedAt
            FROM Users
            WHERE Username = @Username;
        ";

        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.AddWithValue("@Username", username);

        using SqliteDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return MapReaderToUser(reader);
        }

        return null;
    }

    public void UpdateUserRole(string username,string role)
    {
        using SqliteConnection connection = new SqliteConnection(_databaseConnectionService.GetConnectionString());

        connection.Open();

        string sql = @"
            UPDATE Users
            SET Role = @Role
            WHERE Username = @Username;
        ";

        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.AddWithValue("@Role", role);
        command.Parameters.AddWithValue("@Username", username);

        command.ExecuteNonQuery();
    }

    public void UpdateUserActiveStatus( string username, bool isActive)
    {
        using SqliteConnection connection =new SqliteConnection
        (
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        string sql = @"
            UPDATE Users
            SET IsActive = @IsActive
            WHERE Username = @Username;
        ";

        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);

        command.ExecuteNonQuery();
    }

    private User MapReaderToUser(SqliteDataReader reader)
    {
        return new User
        {
            UserId = reader.GetInt32(0),
            Username = reader.GetString(1),
            PasswordHash = reader.GetString(2),
            FullName = reader.GetString(3),
            Role = reader.GetString(4),
            IsActive = reader.GetInt32(5) == 1,
            CreatedAt = DateTime.Parse(reader.GetString(6))
        };
    }
}