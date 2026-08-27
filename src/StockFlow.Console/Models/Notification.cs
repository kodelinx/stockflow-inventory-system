namespace StockFlow.Models;

public class Notification
{
    public int NotificationId { get; set; }
    public string NotificationType { get;  set; } = String.Empty;
    public string Recipient { get; set; } = String.Empty;
    public string Subject { get; set; } = String.Empty;
    public string Message { get; set; } = String.Empty;
    public DateTime CreatedAt {get; set; }
    public string Status { get; set; } = String.Empty;

    public Notification()
    {
        
    }

    public Notification(
        int notificationId,
        string notificationType,
        string recipient,
        string subject,
        string message,
        DateTime createdAt,
        string status)
    {
        NotificationId = notificationId;
        NotificationType = notificationType;
        Recipient = recipient;
        Subject = subject;
        Message = message;
        CreatedAt = createdAt;
        Status = status;
    }


}