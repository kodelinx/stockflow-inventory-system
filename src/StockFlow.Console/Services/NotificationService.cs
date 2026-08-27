using StockFlow.Models;
using StockFlow.Services;
using StockFlow.Utilities;

namespace StockFlow.Models;

public class NotificationService
{
    private readonly InputValidationService _inputValidationService = new InputValidationService();

    public NotificationService(InputValidationService inputValidationService)
    {
        _inputValidationService = inputValidationService;
    }
    public void SimulateLowStockEmail(
        List<Product> products, 
        List<Notification> notifications, 
        AlertService alertService)
    {
        List<Product> lowStockProducts = alertService.GetLowStockProducts(products);

        if (lowStockProducts.Count == 0)
        {
            Console.WriteLine("No low-stock products found. No notification created.\n");
            return;
        }

        string subject = "Low Stock Alert";
        string message = "The following products are low in stock: \n\n";

        foreach (Product product in lowStockProducts)
        {
            message += $"- {product.ProductCode} - {product.Name}\n";
            message += $"- Current Stock: {product.QuantityInStock}\n";
            message += $"- ReorderLevel: {product.ReorderLevel}\n\n";
        }

        CreateNotification(
            notifications,
            "Low Stock",
            "business-owner@example.com",
            subject,
            message
        );

        Console.WriteLine("Low-stock email notification simulated successfully");
    }

    public void SimulateOrderCompletedEmail(List<Order> orders, List<Notification> notifications)
    {
        if (orders.Count == 0)
        {
            Console.WriteLine("No order available for notification\n");
            return;
        }

        string orderNumber = _inputValidationService.GetRequiredText("Enter completed order name: ");

        Order? order = orders.FirstOrDefault(order => 
            order.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase)
        );

        if (order == null)
        {
            Console.WriteLine("Order not found.\n");
            return;
        }

        if (!order.OrderStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Order is not completed. Notification was not created.\n");
            return;
        }

        string subject = $"Order Completed - {order.OrderNumber}";
        string message = "";

        message += $"Order Number: {order.OrderNumber}\n";
        message += $"Order Date: {order.OrderDate}\n";
        message += $"Total Amount: {order.TotalAmount:C}\n";
        message += $"Payment Status: {order.PaymentStatus}\n";

        CreateNotification(
            notifications,
            "Order Completed",
            "business-owner@example.com",
            subject,
            message
        );

        Console.WriteLine("Order completed email notification simulated successfully.\n");

    }

    public void SimulateReceiptEmail(List<Receipt> receipts, List<Notification> notifications)
    {
        if (receipts.Count == 0)
        {
            Console.WriteLine("No receipts available for notification.\n");
            return;
        }

        string receiptNumber = _inputValidationService.GetRequiredText("Enter receipt number: ");

        Receipt? receipt = receipts.FirstOrDefault(receipt =>
            receipt.ReceiptNumber.Equals(receiptNumber, StringComparison.OrdinalIgnoreCase)
        );

        if (receipt == null)
        {
            Console.WriteLine("Receipt not found.\n");
            return;
        }

        string subject = $"Receipt Generated - {receipt.ReceiptNumber}";
        string message = "";

        message += $"Receipt Number: {receipt.ReceiptNumber}\n";
        message += $"Order Number: {receipt.OrderNumber}\n";
        message += $"Payment Number: {receipt.PaymentNumber}\n";
        message += $"Receipt Date: {receipt.ReceiptDate}\n";
        message += $"Total Amount: {receipt.TotalAmount:C}\n";
        message += $"Payment Method: {receipt.PaymentMethod}\n";
        message += $"Amount Paid: {receipt.AmountPaid:C}\n";
        message += $"Change: {receipt.ChangeAmount:C}\n";

        CreateNotification(
            notifications,
            "Receipt",
            "customer@example.com",
            subject,
            message
        );

        Console.WriteLine("Receipt email notification simulated successfully.\n");
    }

    public void ViewNotificationEmail(List<Notification> notifications)
    {
        if (notifications.Count == 0)
        {
            Console.WriteLine("No notifications available.\n");
            return;
        }

        Console.WriteLine("\nNotification History");
        Console.WriteLine("--------------------");

        foreach (Notification notification in notifications)
        {
            Console.WriteLine($"Notification ID: {notification.NotificationId}");
            Console.WriteLine($"Type: {notification.NotificationType}");
            Console.WriteLine($"Recipient: {notification.Recipient}");
            Console.WriteLine($"Subject: {notification.Subject}");
            Console.WriteLine($"Message:\n{notification.Message}");
            Console.WriteLine($"Created At: {notification.CreatedAt}");
            Console.WriteLine($"Status: {notification.Status}");
            Console.WriteLine("--------------------");
        }
    }

    public void CreateNotification(
        List<Notification> notifications,
        string notificationType,
        string recipient,
        string subject,
        string message
    )
    {
        int notificationId = notifications.Count + 1;

        Notification notification = new Notification(
            notificationId,
            notificationType,
            recipient,
            subject,
            message,
            DateTime.Now,
            "Simulated"
        );

        notifications.Add(notification);
    }
}