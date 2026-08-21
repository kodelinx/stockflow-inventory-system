using StockFlow.Services;
using StockFlow.Models;
using StockFlow.Utilities;

List<Product> products = new List<Product>();
InputValidationService inputValidationService = new InputValidationService();
InventoryService inventoryService = new InventoryService(inputValidationService);

bool keepRunning = true;

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


while (keepRunning)
{
    Console.WriteLine("-------------------------------");
    Console.WriteLine("\n\bStockFlow Inventory and Sales Management System\n");
    Console.WriteLine("-------------------------------");

    Console.WriteLine("1. Add Product");
    Console.WriteLine("2. View Products");
    Console.WriteLine("3. Search Product");
    Console.WriteLine("4. Update Product");
    Console.WriteLine("5. Deactivate Product");
    Console.WriteLine("6. Delete Product");
    Console.WriteLine("7. Exit");

    int option = inputValidationService.GetValidInt("Choose an option: ",  1, 7);

    switch(option)
    {
        case 1:
            inventoryService.AddProduct(products);
            break;
        case 2:
            inventoryService.ViewProducts(products);
            break;
        case 3:
            inventoryService.SearchProduct(products);
            break;
        case 4:
            inventoryService.UpdateProduct(products);
            break;
        case 5:
            inventoryService.DeactivateProduct(products);
            break;
        case 6:
            inventoryService.DeleteProduct(products);
            break;
        case 7:
            Console.WriteLine("StockFlow has been closed");
            keepRunning = false;
            break;
        default:
            Console.WriteLine("Invalid option.\n");
            continue;
    }
    
}




