using StockFlow.Models;
using StockFlow.Repositories;

namespace StockFlow.Services;

public class AlertService
{
    private readonly ProductRepository _productRepository;
    public AlertService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public void ShowLowstockAlerts()
    {
        List<Product> lowStockProducts = GetLowStockProducts();

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
    public int CountLowStockProducts()
    {
        return  GetLowStockProducts().Count;
    }

    public List<Product> GetLowStockProducts()
    {
        List<Product> activeProducts = _productRepository.GetActiveProducts();

        return activeProducts
        .Where(product =>
            product.QuantityInStock <= product.ReorderLevel)
        .ToList();
    }
}