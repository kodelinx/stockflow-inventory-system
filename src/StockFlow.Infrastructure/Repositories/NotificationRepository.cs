using Microsoft.Data.Sqlite;
using StockFlow.Database;
using StockFlow.Models;

namespace StockFlow.Repositories;

public class NotificationRepository
{
    private readonly DatabaseConnectionService _databaseConnectionService;

    public NotificationRepository(DatabaseConnectionService databaseConnectionService)
    {
        _databaseConnectionService = databaseConnectionService;
    }

    public void AddNotification(Notification notification)
    {
        // Opens a connection to the SQLite database.
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        // Inserts one notification record.
        string sql = @"
            INSERT INTO Notifications
            (
                NotificationType,
                Recipient,
                Subject,
                Title,
                Message,
                RelatedReference,
                IsRead,
                CreatedAt,
                Status
            )
            VALUES
            (
                @NotificationType,
                @Recipient,
                @Subject,
                @Title,
                @Message,
                @RelatedReference,
                @IsRead,
                @CreatedAt,
                @Status
            );
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@NotificationType", notification.NotificationType);
        command.Parameters.AddWithValue("@Recipient", notification.Recipient);
        command.Parameters.AddWithValue("@Subject", notification.Subject);
        command.Parameters.AddWithValue("@Title", notification.Title);
        command.Parameters.AddWithValue("@Message", notification.Message);
        command.Parameters.AddWithValue("@RelatedReference", notification.RelatedReference);
        command.Parameters.AddWithValue("@IsRead", notification.IsRead);
        command.Parameters.AddWithValue("@CreatedAt", notification.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@Status", notification.Status);

        command.ExecuteNonQuery();
    }

    public List<Notification> GetAllNotifications()
    {
        // Stores all notification records from the database.
        List<Notification> notifications = new List<Notification>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        // Gets all notifications, read and unread.
        string sql = @"
            SELECT
                NotificationId,
                NotificationType,
                Recipient,
                Subject,
                Title,
                Message,
                RelatedReference,
                IsRead,
                CreatedAt,
                Status
            FROM Notifications;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            // Converts each database row into a Notification object.
            Notification notification = MapReaderToNotification(reader);

            notifications.Add(notification);
        }

        return notifications;
    }

    public List<Notification> GetUnreadNotifications()
    {
        // Stores only unread notifications.
        List<Notification> notifications = new List<Notification>();

        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        // Gets notifications that are not yet marked as read.
        string sql = @"
            SELECT
                NotificationId,
                NotificationType,
                Recipient,
                Subject,
                Title,
                Message,
                RelatedReference,
                IsRead,
                CreatedAt,
                Status
            FROM Notifications
            WHERE IsRead = 0;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            // Converts each unread row into a Notification object.
            Notification notification = MapReaderToNotification(reader);

            notifications.Add(notification);
        }

        return notifications;
    }

    public void MarkAsRead(int notificationId)
    {
        using SqliteConnection connection = new SqliteConnection(
            _databaseConnectionService.GetConnectionString()
        );

        connection.Open();

        // Marks one notification as already read.
        string sql = @"
            UPDATE Notifications
            SET IsRead = 1
            WHERE NotificationId = @NotificationId;
        ";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        command.Parameters.AddWithValue("@NotificationId", notificationId);

        command.ExecuteNonQuery();
    }

    private Notification MapReaderToNotification(SqliteDataReader reader)
    {
        // Converts database column values into a Notification model.
        return new Notification
        {
            NotificationId = reader.GetInt32(0),
            NotificationType = reader.GetString(1),
            Recipient = reader.GetString(2),
            Subject = reader.GetString(3),
            Title = reader.GetString(4),
            Message = reader.GetString(5),
            RelatedReference = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
            IsRead = reader.GetInt32(7) == 1,
            CreatedAt = DateTime.Parse(reader.GetString(8)),
            Status = reader.GetString(9)
        };
    }
}