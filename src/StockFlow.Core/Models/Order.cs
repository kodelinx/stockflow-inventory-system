using System.Runtime.CompilerServices;

namespace StockFlow.Models;

public class Order
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = String.Empty; 
    public DateTime OrderDate { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public decimal TotalAmount { get; set; }
    public string OrderStatus { get; set; } = String.Empty; 
    public string PaymentStatus { get; set; } = String.Empty; 

    public Order(
        int orderId,
        string orderNumber,
        DateTime orderDate,
        List<OrderItem> items,
        decimal totalAmount,
        string orderStatus,
        string paymentStatus
    )
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        Items = items;
        TotalAmount = totalAmount;
        OrderStatus = orderStatus;
        PaymentStatus = paymentStatus;
    }
    //Constructor to be utilized by JsonSerializer.Deserialize()
    public Order()
    {
        
    }
}