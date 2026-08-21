namespace StockFlow.Models;

public class BasketItem
{
    public int ProductId { get; set;}
    public string ProductCode { get; set; }
    public string ProductName {get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public BasketItem(
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
        TotalPrice = UnitPrice * Quantity;
    }

}