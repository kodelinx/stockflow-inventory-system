using StockFlow.Models;
using StockFlow.Repositories;
using StockFlow.Tests.Helpers;

namespace StockFlow.Tests.Repositories;

public class NotificationRepositoryTests
{
    [Fact]
    public void AddNotification_ValidNotification_PersistsNotification()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        NotificationRepository repository =
            new NotificationRepository(
                testDatabase.DatabaseConnectionService
            );

        Notification notification =
            new Notification
            {
                NotificationType = "Order Completed",
                Recipient = "test@example.com",
                Subject = "Order Completed",
                Title = "Order Completed",
                Message = "Order TEST-ORD-001 was completed.",
                RelatedReference = "TEST-ORD-001",
                IsRead = false,
                CreatedAt = DateTime.Now,
                Status = "Sent"
            };

        repository.AddNotification(
            notification
        );

        List<Notification> notifications =
            repository.GetAllNotifications();

        Assert.Single(notifications);

        Notification savedNotification =
            notifications[0];

        Assert.True(
            savedNotification.NotificationId > 0
        );

        Assert.Equal(
            "Order Completed",
            savedNotification.NotificationType
        );

        Assert.Equal(
            "test@example.com",
            savedNotification.Recipient
        );

        Assert.Equal(
            "TEST-ORD-001",
            savedNotification.RelatedReference
        );

        Assert.False(
            savedNotification.IsRead
        );

        Assert.Equal(
            "Sent",
            savedNotification.Status
        );
    }

    [Fact]
    public void GetUnreadNotifications_ReadAndUnread_ReturnsOnlyUnread()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        NotificationRepository repository =
            new NotificationRepository(
                testDatabase.DatabaseConnectionService
            );

        Notification unread =
            new Notification
            {
                NotificationType = "Low Stock",
                Recipient = "inventory@example.com",
                Subject = "Low Stock",
                Title = "Low Stock Alert",
                Message = "Mouse is low in stock.",
                RelatedReference = string.Empty,
                IsRead = false,
                CreatedAt = DateTime.Now,
                Status = "Sent"
            };

        Notification read =
            new Notification
            {
                NotificationType = "Receipt",
                Recipient = "customer@example.com",
                Subject = "Receipt",
                Title = "Receipt Available",
                Message = "Receipt TEST-REC-001 is available.",
                RelatedReference = "TEST-REC-001",
                IsRead = true,
                CreatedAt = DateTime.Now,
                Status = "Sent"
            };

        repository.AddNotification(unread);
        repository.AddNotification(read);

        List<Notification> unreadNotifications =
            repository.GetUnreadNotifications();

        Assert.Single(unreadNotifications);

        Assert.Equal(
            "Low Stock",
            unreadNotifications[0].NotificationType
        );

        Assert.False(
            unreadNotifications[0].IsRead
        );

        Assert.DoesNotContain(
            unreadNotifications,
            notification =>
                notification.NotificationType ==
                "Receipt"
        );
    }

    [Fact]
    public void MarkAsRead_UnreadNotification_SetsIsReadTrue()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        NotificationRepository repository =
            new NotificationRepository(
                testDatabase.DatabaseConnectionService
            );

        Notification notification =
            new Notification
            {
                NotificationType = "Order Completed",
                Recipient = "test@example.com",
                Subject = "Order Completed",
                Title = "Order Completed",
                Message = "Order completed.",
                RelatedReference = "TEST-ORD-002",
                IsRead = false,
                CreatedAt = DateTime.Now,
                Status = "Sent"
            };

        repository.AddNotification(
            notification
        );

        List<Notification> notifications =
            repository.GetAllNotifications();

        Assert.Single(notifications);

        int notificationId =
            notifications[0].NotificationId;

        repository.MarkAsRead(
            notificationId
        );

        List<Notification> updatedNotifications =
            repository.GetAllNotifications();

        Notification? updatedNotification =
            updatedNotifications.FirstOrDefault(
                item =>
                    item.NotificationId ==
                    notificationId
            );

        Assert.NotNull(
            updatedNotification
        );

        Assert.True(
            updatedNotification.IsRead
        );
    }

    [Fact]
    public void MarkAsRead_UnreadNotification_RemovesFromUnreadResults()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        NotificationRepository repository =
            new NotificationRepository(
                testDatabase.DatabaseConnectionService
            );

        Notification notification =
            new Notification
            {
                NotificationType = "Receipt",
                Recipient = "customer@example.com",
                Subject = "Receipt",
                Title = "Receipt Generated",
                Message = "Receipt generated.",
                RelatedReference = "TEST-REC-002",
                IsRead = false,
                CreatedAt = DateTime.Now,
                Status = "Sent"
            };

        repository.AddNotification(
            notification
        );

        List<Notification> notifications =
            repository.GetAllNotifications();

        Assert.Single(notifications);

        int notificationId =
            notifications[0].NotificationId;

        Assert.Single(
            repository.GetUnreadNotifications()
        );

        repository.MarkAsRead(
            notificationId
        );

        List<Notification> unreadNotifications =
            repository.GetUnreadNotifications();

        Assert.Empty(
            unreadNotifications
        );
    }

    [Fact]
    public void AddNotification_NoRelatedReference_PersistsEmptyReferenceAfterRead()
    {
        using TestDatabase testDatabase =
            new TestDatabase();

        NotificationRepository repository =
            new NotificationRepository(
                testDatabase.DatabaseConnectionService
            );

        Notification notification =
            new Notification
            {
                NotificationType = "Low Stock",
                Recipient = "inventory@example.com",
                Subject = "Low Stock",
                Title = "Low Stock Alert",
                Message = "Product is low in stock.",
                RelatedReference = string.Empty,
                IsRead = false,
                CreatedAt = DateTime.Now,
                Status = "Sent"
            };

        repository.AddNotification(
            notification
        );

        List<Notification> notifications =
            repository.GetAllNotifications();

        Assert.Single(notifications);

        Assert.Equal(
            string.Empty,
            notifications[0].RelatedReference
        );
    }
}