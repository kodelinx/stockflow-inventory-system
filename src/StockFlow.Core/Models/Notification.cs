namespace StockFlow.Models;

public class Notification
{
    public int NotificationId { get; set; }
    public string NotificationType { get;  set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Title  { get;  set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt {get; set; }
    public string RelatedReference { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public string Status { get; set; } = string.Empty;

    public Notification()
    {
        
    }

    public Notification(
        int notificationId,
        string notificationType,
        string recipient,
        string subject,
        string title,
        string message,
        string relatedReference,
        bool isRead,
        DateTime createdAt,
        string status)
    {
        NotificationId = notificationId;
        NotificationType = notificationType;
        Recipient = recipient;
        Subject = subject;
        Title = title;
        Message = message;
        RelatedReference = relatedReference;
        IsRead = isRead;
        CreatedAt = createdAt;
        Status = status;
    }


}