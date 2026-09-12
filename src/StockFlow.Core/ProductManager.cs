using StockFlow.Models;

namespace StockFlow.Services;

public class ProductManager
{
    public Product CreateProduct(
        string productCode,
        string name,
        string category,
        decimal unitPrice,
        int quantityInStock,
        int reorderLevel,
        bool isActive
    )
    {
        return new Product
        {
            ProductCode = productCode.Trim(),
            Name = name.Trim(),
            Category = category.Trim(),
            UnitPrice = unitPrice,
            QuantityInStock = quantityInStock,
            ReorderLevel = reorderLevel,
            IsActive = true
        };
    }

    public bool IsLowStock(Product product)
    {
        return product.QuantityInStock <= product.ReorderLevel;
    }

    public bool HasEnoughStock(Product product, int requestedQuantity)
    {
        return product.QuantityInStock >= requestedQuantity;
    }

    public void DeactiveProduct(Product product)
    {
        product.IsActive = false;
    }
}