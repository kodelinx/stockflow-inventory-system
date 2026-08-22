using StockFlow.Services;
using StockFlow.Models;
using StockFlow.Utilities;
using System.Reflection.Metadata;
using StockFlow.Service;

List<Product> products = new List<Product>();
List<BasketItem> basketItems = new List<BasketItem>();
List<Order> orders = new List<Order>();
List<Payment> payments = new List<Payment>();
InputValidationService inputValidationService = new InputValidationService();
InventoryService inventoryService = new InventoryService(inputValidationService);
BasketService basketService = new BasketService(inputValidationService);
OrderService orderService = new OrderService();
PaymentService paymentService = new PaymentService(inputValidationService);

bool keepRunning = true;

products.Add(new Product(
    1,
    "PRD-001",
    "Bottled Water",
    "Drinks",
    20.00m,
    100,
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

basketItems.Add(new BasketItem(
    3,
    "PRD-003",
    "Ballpen",
    10,
    10.00m
));

basketItems.Add(new BasketItem(
    1,
    "PRD-001",
    "Bottled Water",
    2,
    20.00m
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
    Console.WriteLine("7. Add Product to Basket");
    Console.WriteLine("8. View Items in Basket");
    Console.WriteLine("9. Delete an Item in Basket");
    Console.WriteLine("10. Clear all items in Basket");
    Console.WriteLine("11. Checkout Basket");
    Console.WriteLine("12. View all orders");
    Console.WriteLine("13. Process Payments");
    Console.WriteLine("14. View Payments");
    Console.WriteLine("15. Exit");

    int option = inputValidationService.GetValidInt("Choose an option: ",  1, 15);
    Console.WriteLine("");

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
            basketService.AddItemToBasket(products, basketItems);
            break;
        case 8:
            basketService.ViewBasket(basketItems);
            break;
        case 9:
            basketService.RemoveIteminBasket(basketItems);
            break;
        case 10:
            basketService.ClearBasket(basketItems);
            break;
        case 11:
            orderService.CheckoutBasket(products, basketItems, orders);
            break;
        case 12:
            orderService.ViewOrders(orders);
            break;
        case 13:
            paymentService.ProcessPayment(orders, payments);
            break;
        case 14:
            paymentService.ViewPayments(payments);
            break;
        case 15:
            Console.WriteLine("StockFlow has been closed");
            keepRunning = false;
            break;
        default:
            Console.WriteLine("Invalid option.\n");
            continue;
    }
    
}




