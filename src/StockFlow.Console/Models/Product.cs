namespace StockFlow.Models;

public class Product
{
    public int ProductId { get; set;}
    public string ProductCode { get; set;  } = String.Empty; 
    public string Name { get; set; } = String.Empty; 
    public string Category { get; set;} = String.Empty; 
    public decimal UnitPrice { get; set; }
    public int QuantityInStock { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; }

    public Product (
        int productId,
        string productCode,
        string name,
        string category,
        decimal unitPrice,
        int quantityInStock,
        int reorderLevel,
        bool  isActive
    )
    {
        ProductId = productId;
        ProductCode = productCode;
        Name = name;
        Category = category;
        UnitPrice = unitPrice;
        QuantityInStock = quantityInStock;
        ReorderLevel = reorderLevel;
        IsActive = isActive;
    }
    //Constructor to be utilized by JsonSerializer.Deserialize()
    public Product()
    {
        
    }

}