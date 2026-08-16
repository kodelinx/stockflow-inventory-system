using StockFlow.Services;
using StockFlow.Models;

List<Product> products = new List<Product>();
InventoryService inventoryService = new InventoryService();

products.Add(new Product(
    1,
    "PRD-001",
    "Bottled Water",
    "Drinks",
    20.00m,
    50,
    10,
    true
));

products.Add(new Product(
    2,
    "PRD-002",
    "Notebook",
    "School Supplies",
    35.00m,
    25,
    5,
    true
));

products.Add(new Product(
    3,
    "PRD-003",
    "Ballpen",
    "School Supplies",
    10.00m,
    100,
    20,
    true
));

Console.WriteLine("-------------------------------");
Console.WriteLine("\n\bStockFlow Inventory and Sales Management System\n");
Console.WriteLine("-------------------------------");

inventoryService.ViewProducts(products);





