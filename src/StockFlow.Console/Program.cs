using StockFlow.Services;
using StockFlow.Models;
using StockFlow.Utilities;
using StockFlow.Data;


List<Product> products = new List<Product>();
List<BasketItem> basketItems = new List<BasketItem>();
List<Order> orders = new List<Order>();
List<Payment> payments = new List<Payment>();
List<Receipt> receipts = new List<Receipt>();
List<StockMovement> stockMovements = new List<StockMovement>();
List<Notification> notifications = new List<Notification>();
InputValidationService inputValidationService = new InputValidationService();
InventoryService inventoryService = new InventoryService(inputValidationService);
BasketService basketService = new BasketService(inputValidationService);
OrderService orderService = new OrderService();
PaymentService paymentService = new PaymentService(inputValidationService);
ReceiptService receiptService = new ReceiptService(inputValidationService);
LoggingService loggingService = new LoggingService();
JsonStorageService jsonStorageService = new JsonStorageService(loggingService);
StockMovementService stockMovementService = new StockMovementService(inputValidationService);
AlertService alertService = new AlertService();
DashboardService dashboardService = new DashboardService(alertService);
SalesReportService salesReportService = new SalesReportService();
NotificationService notificationService = new NotificationService(inputValidationService);


string productsFilePath = "Data/products.json";
string ordersFilePath ="Data/orders.json";
string paymentsFilePath = "Data/payments.json";
string receiptsFilePath = "Data/receipts.json";
string stockMovementsFilePath = "Data/stock-movements.json";
string notificationFilePath = "Data/notifications.json";

bool keepRunning = true;

loggingService.LogInfo("StockFlow application started.");

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
    Console.WriteLine("15. Generate Receipt");
    Console.WriteLine("16. View Receipts");
    Console.WriteLine("17. Show Dashboard");
    Console.WriteLine("18. Save Data to JSON");
    Console.WriteLine("19. Load Data from JSON");
    Console.WriteLine("20. Add Stock");
    Console.WriteLine("21. Adjust Stock");
    Console.WriteLine("22. View Stock movements");
    Console.WriteLine("23. View Low Stock Products");
    Console.WriteLine("24. Export Receipt to Text File");
    Console.WriteLine("25. View Sales Summary Report");
    Console.WriteLine("26. Simulate Low Stock Email");
    Console.WriteLine("27. Simulate Order Completed Email");
    Console.WriteLine("28. Simulate Receipt Email");
    Console.WriteLine("29. View Notifications");
    Console.WriteLine("30. Exit");

    int option = inputValidationService.GetValidInt("Choose an option: ",  1, 30);
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
            orderService.CheckoutBasket(products, basketItems, orders, stockMovements, stockMovementService);
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
            receiptService.GenerateReceipt(orders, payments, receipts);
            break;
        case 16:
            receiptService.ViewReceipts(orders, payments, receipts);
            break;
        case 17:
            dashboardService.ShowDashboard(products, orders, payments);
            break;
        case 18:
            jsonStorageService.SaveData(products, productsFilePath);
            jsonStorageService.SaveData(orders, ordersFilePath);
            jsonStorageService.SaveData(payments, paymentsFilePath);
            jsonStorageService.SaveData(receipts, receiptsFilePath);
            jsonStorageService.SaveData(stockMovements, stockMovementsFilePath);
            jsonStorageService.SaveData(notifications, notificationFilePath);
            break;
        case 19:
            products = jsonStorageService.LoadData<Product>(productsFilePath);
            orders = jsonStorageService.LoadData<Order>(ordersFilePath);
            payments = jsonStorageService.LoadData<Payment>(paymentsFilePath);
            receipts = jsonStorageService.LoadData<Receipt>(receiptsFilePath);
            stockMovements = jsonStorageService.LoadData<StockMovement>(stockMovementsFilePath);
            notifications = jsonStorageService.LoadData<Notification>(notificationFilePath);
            break;    
        case 20:
            stockMovementService.AddStock(products, stockMovements);
            break;
        case 21:
            stockMovementService.AdjustStock(products, stockMovements);
            break;
        case 22:
            stockMovementService.ViewStockMovements(stockMovements);
            break;
        case 23:
            alertService.ShowLowstockAlers(products);
            break;
        case 24:
            receiptService.ExportReceiptToTextFile(orders, payments, receipts);
            break;
        case 25:
            salesReportService.ShowSalesSummary(orders, payments);
            break;
        case 26:
            notificationService.SimulateLowStockEmail(products, notifications, alertService);
            break;
        case 27:
            notificationService.SimulateOrderCompletedEmail(orders, notifications);
            break;
        case 28:
            notificationService.SimulateReceiptEmail(receipts,  notifications);
            break;
        case 29:
            notificationService.ViewNotificationEmail(notifications);
            break;
        case 30:
            loggingService.LogInfo("Stockflow application closed.");
            Console.WriteLine("StockFlow has been closed");
            keepRunning = false;
            break;
        default:
            Console.WriteLine("Invalid option.\n");
            continue;
    }
    
}




