using StockFlow.Models;

namespace StockFlow.Services;

public class InventoryService
{
    public void ViewProducts(List<Product> products)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("No products to view.");
            return;
        }

        Console.WriteLine("\n\b Product Inventory");
        Console.WriteLine("------------------------");

        foreach (Product product in products)
        {
            Console.WriteLine($"\nProduct ID: {product.ProductId}");
            Console.WriteLine($"Product Code: {product.ProductCode}");
            Console.WriteLine($"Name: {product.Name}");
            Console.WriteLine($"Category: {product.Category}");
            Console.WriteLine($"Unit Price: {product.UnitPrice}");
            Console.WriteLine($"Quantity in Stock: {product.QuantityInStock}");
            Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
            Console.WriteLine($"Active:  {product.IsActive}\n");
            Console.WriteLine("------------------------"); 
        }
        Console.WriteLine("");
    }
}



