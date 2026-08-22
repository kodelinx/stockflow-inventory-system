namespace StockFlow.Models;

public class Order
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public string OrderStatus { get; set; }

    public Order(
        int orderId,
        string orderNumber,
        DateTime orderDate,
        List<OrderItem> items,
        decimal totalAmount,
        string orderStatus
    )
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        Items = items;
        TotalAmount = totalAmount;
        OrderStatus = orderStatus;
    }
}