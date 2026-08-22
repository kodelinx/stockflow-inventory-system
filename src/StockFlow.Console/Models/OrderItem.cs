namespace StockFlow.Models;

public class OrderItem
{
    public int ProductId { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }

    public OrderItem(
        int productId, 
        string productCode, 
        string productName, 
        int quantity, 
        decimal unitPrice)
    {
        ProductId = productId;
        ProductCode = productCode;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        LineTotal = UnitPrice * Quantity;
    }
}