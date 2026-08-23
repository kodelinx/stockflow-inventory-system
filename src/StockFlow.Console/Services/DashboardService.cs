using StockFlow.Models;

namespace StockFlow.Services;

public class DashboardService
{
    public void ShowDashboard(List<Product> products, List<Order> orders, List<Payment> payments)
    {
        Console.WriteLine("\nStockFlow Dashboard");
        Console.WriteLine("================");
        ShowInventorySummary(products);
        ShowOrderSummary(orders);
        ShowPaymentSummary(payments);
        ShowLowStockProducts(products);

        Console.WriteLine();
    }
    public void ShowInventorySummary(List<Product> products)
    {
        int totalProducts = products.Count;
        int activeProducts = products.Count(product => product.IsActive);
        int inactiveProducts = products.Count(product => !product.IsActive);
        int totalStockQuantity = products
            .Where(product => product.IsActive)
            .Sum(product => product.QuantityInStock);

        Console.WriteLine("\nInventory Summary");
        Console.WriteLine("-----------------");
        Console.WriteLine($"Total Products: {totalProducts}");
        Console.WriteLine($"Active Products: {activeProducts}");
        Console.WriteLine($"Inactive Products: {inactiveProducts}");
        Console.WriteLine("Quantity of Stocks per Product");
        foreach(Product product in products)
        {
            Console.WriteLine($" - {product.Name}: {product.QuantityInStock}");
        }
        Console.WriteLine($"Total Stock Quantity: {totalStockQuantity}");
        
    }
    public void ShowOrderSummary(List<Order> orders)
    {
        int totalOrders = orders.Count;
        int completedOrders = orders.Count(order => 
            order.OrderStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase)
        );
        int pendingOrders = orders.Count(order => 
            !order.OrderStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase)
        );
        
        Console.WriteLine("\nOrder Summary");
        Console.WriteLine("-------------");
        Console.WriteLine($"Total Orders: {totalOrders}");
        Console.WriteLine($"Completed Orders: {completedOrders}");
        Console.WriteLine($"Pending Orders: {pendingOrders}");
    }
    public void ShowPaymentSummary(List<Payment> payments)
    {
        int totalPayments = payments.Count;
        decimal totalIncome = payments
        .Where(payment => payment.PaymentStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase))
        .Sum(payment => payment.AmountDue);

        Console.WriteLine("\nPayment Summary");
        Console.WriteLine("---------------");
        Console.WriteLine($"Total Payments: {totalPayments}");
        Console.WriteLine($"Total Income: {totalIncome:C}");
    }
    public void ShowLowStockProducts(List<Product> products)
    {
        List<Product> lowStockProducts = products
            .Where(product => 
                product.IsActive && 
                product.QuantityInStock <= product.ReorderLevel)
            .ToList();

        Console.WriteLine("\nLow Stock Products");
        Console.WriteLine("------------------");

        if (lowStockProducts.Count == 0)
        {
            Console.WriteLine("No low-stock products.");
            return;
        }

        foreach (Product product in lowStockProducts)
        {
            Console.WriteLine($"{product.ProductCode} - {product.Name}");
            Console.WriteLine($"Stock: {product.QuantityInStock}");
            Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
            Console.WriteLine("------------------");
        }
        
    }
}