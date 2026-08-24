using System.Security.Cryptography.X509Certificates;
using StockFlow.Models;

namespace StockFlow.Services;

public class AlertService
{
    public void ShowLowstockAlers(List<Product> products)
    {
        List<Product> lowStockProducts = GetLowStockProducts(products);

        if (lowStockProducts.Count == 0)
        {
            Console.WriteLine("No low-stock products.\n");
            return;
        }

        foreach (Product product in lowStockProducts)
        {
            Console.WriteLine("----------------");
            Console.WriteLine($"ALERT: {product.Name} is low in stock.");
            Console.WriteLine($"Product Code: {product.ProductCode}");
            Console.WriteLine($"Current Stock: {product.QuantityInStock}");
            Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
            Console.WriteLine("----------------");
        }

        Console.WriteLine();

    }
    public int CountLowStockProducts(List<Product> products)
    {
        return  GetLowStockProducts(products).Count;
    }

    public List<Product> GetLowStockProducts(List<Product> products)
    {
        return products
        .Where(product => 
            product.IsActive && 
            product.QuantityInStock <= product.ReorderLevel)
        .ToList();
    }
}