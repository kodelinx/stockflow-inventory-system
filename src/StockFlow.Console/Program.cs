using StockFlow.Services;
using StockFlow.Models;
using StockFlow.Utilities;
using StockFlow.Data;
using StockFlow.Database;
using StockFlow.Repositories;

ProductManager productManager = new ProductManager();

LoggingService loggingService = new LoggingService();
JsonStorageService jsonStorageService = new JsonStorageService(loggingService);


// Prepares the SQLite database and creates needed tables.
DatabaseConnectionService databaseConnectionService = new DatabaseConnectionService();
databaseConnectionService.InitializeDatabase();

Console.WriteLine(
    $"Database location: {databaseConnectionService.GetDatabaseFilePath()}"
);

// Temporary migration: fixes old typo in existing OrderItems table.
//databaseConnectionService.RenameLineTotaColumnIfNeeded();

// Creates repositories used by the Console app.
ProductRepository productRepository = new ProductRepository(databaseConnectionService);
OrderRepository orderRepository = new OrderRepository(databaseConnectionService);
OrderItemRepository orderItemRepository = new OrderItemRepository(databaseConnectionService);
PaymentRepository paymentRepository = new PaymentRepository(databaseConnectionService);
ReceiptRepository receiptRepository = new ReceiptRepository(databaseConnectionService);
StockMovementRepository stockMovementRepository = new StockMovementRepository(databaseConnectionService);
NotificationRepository notificationRepository = new NotificationRepository(databaseConnectionService);

InputValidationService inputValidationService = new InputValidationService();

BasketService basketService = new BasketService(
    inputValidationService,
    productRepository
);

InventoryService inventoryService = new InventoryService(
    inputValidationService, 
    productManager,
    productRepository
);

PaymentService paymentService = new PaymentService(
        inputValidationService,
        paymentRepository,
        orderRepository
    );
ReceiptService receiptService = new ReceiptService(
        inputValidationService,
        paymentRepository,
        orderRepository,
        orderItemRepository,
        receiptRepository
    );

StockMovementService stockMovementService = new StockMovementService(
    inputValidationService,
    stockMovementRepository,
    productRepository
    
);

OrderService orderService = new OrderService(
    productRepository,
    orderRepository,
    orderItemRepository,
    stockMovementService
);

AlertService alertService = new AlertService(
    productRepository
);

DashboardService dashboardService = new DashboardService(
    alertService,
    productRepository,
    orderRepository,
    paymentRepository
);

NotificationService notificationService = new NotificationService(
    inputValidationService,
    notificationRepository,
    productRepository,
    orderRepository,
    receiptRepository,
    alertService
);

SalesReportService salesReportService = new SalesReportService(
    orderRepository,
    paymentRepository
);

// Temporary lists
List<BasketItem> basketItems = new List<BasketItem>();


Console.WriteLine("\nSQLite database initialized successfully.\n");

/*
string productsFilePath = "Data/products.json";
string ordersFilePath ="Data/orders.json";
string paymentsFilePath = "Data/payments.json";
string receiptsFilePath = "Data/receipts.json";
string stockMovementsFilePath = "Data/stock-movements.json";
string notificationFilePath = "Data/notifications.json";
*/

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
    Console.WriteLine("6. Reactivate Product");
    Console.WriteLine("7. Delete Product");
    Console.WriteLine("8. Add Product to Basket");
    Console.WriteLine("9. View Items in Basket");
    Console.WriteLine("10. Delete an Item in Basket");
    Console.WriteLine("11. Clear all items in Basket");
    Console.WriteLine("12. Checkout Basket");
    Console.WriteLine("13. View all orders");
    Console.WriteLine("14. Process Payments");
    Console.WriteLine("15. View Payments");
    Console.WriteLine("16. Generate Receipt");
    Console.WriteLine("17. View Receipts");
    Console.WriteLine("18. Show Dashboard");
    Console.WriteLine("19. Save Data to JSON (Legacy - Disabled)");
    Console.WriteLine("20. Reload Data(Legacy - Disabled)");
    Console.WriteLine("21. Add Stock");
    Console.WriteLine("22. Adjust Stock");
    Console.WriteLine("23. View Stock movements");
    Console.WriteLine("24. View Low Stock Products");
    Console.WriteLine("25. Export Receipt to Text File");
    Console.WriteLine("26. View Sales Summary Report");
    Console.WriteLine("27. Simulate Low Stock Email");
    Console.WriteLine("28. Simulate Order Completed Email");
    Console.WriteLine("29. Simulate Receipt Email");
    Console.WriteLine("30. View Notifications");
    Console.WriteLine("31. Reset Database");
    Console.WriteLine("32. Exit");

    int option = inputValidationService.GetValidInt("Choose an option: ",  1, 32);
    Console.WriteLine("");

    switch(option)
    {
        case 1:
            inventoryService.AddProduct();
            break;
        case 2:
            inventoryService.ViewProducts();
            break;
        case 3:
            inventoryService.SearchProduct();
            break;
        case 4:
            inventoryService.UpdateProduct();
            break;
        case 5:
            inventoryService.DeactivateProduct();
            break;
        case 6:
            inventoryService.ReactivateProduct();
            break;
        case 7:
            inventoryService.DeleteProduct();
            break;
        case 8:
            basketService.AddItemToBasket(basketItems);
            break;
        case 9:
            basketService.ViewBasket(basketItems);
            break;
        case 10:
            basketService.RemoveIteminBasket(basketItems);
            break;
        case 11:
            basketService.ClearBasket(basketItems);
            break;
        case 12:
            orderService.CheckoutBasket(basketItems);
            break;
        case 13:
            orderService.ViewOrders();
            break;
        case 14:
            paymentService.ProcessPayment();
            break;
        case 15:
            paymentService.ViewPayments();
            break;
        case 16:
            receiptService.GenerateReceipt();
            break;
        case 17:
            receiptService.ViewReceipts();
            break;
        case 18:
            dashboardService.ShowDashboard();
            break;
        case 19:
             // Transitional only: products are now read from SQLite, but JSON save still exists.
            /*jsonStorageService.SaveData(products, productsFilePath);
            jsonStorageService.SaveData(orders, ordersFilePath);
            jsonStorageService.SaveData(payments, paymentsFilePath);
            jsonStorageService.SaveData(receipts, receiptsFilePath);
            jsonStorageService.SaveData(stockMovements, stockMovementsFilePath);
            jsonStorageService.SaveData(notifications, notificationFilePath);**/

            Console.WriteLine("JSON saving is disabled because SQLite is now the main data source.\n");

            break;
        case 20:
            /* Reloads products from SQLite.
            products = productRepository.GetActiveProducts();

            // Orders, payments, and receipts may now also be database-backed.
            orders = orderRepository.GetAllOrders();
            payments = paymentRepository.GetAllPayments();
            receipts = receiptRepository.GetAllReceipts();

            // Reloads stock movements and notifications from SQLite.
            stockMovements = stockMovementRepository.GetAllStockMovements();
            notifications = notificationRepository.GetAllNotifications();

            Console.WriteLine("Data reloaded from SQLite successfully.\n");
            */
            Console.WriteLine("Functionality Disabled (Legacy)");
            break;  
        case 21:
            stockMovementService.AddStock();
            break;
        case 22:
            stockMovementService.AdjustStock();
            break;
        case 23:
            stockMovementService.ViewStockMovements();
            break;
        case 24:
            alertService.ShowLowstockAlerts();
            break;
        case 25:
            receiptService.ExportReceiptToTextFile();
            break;
        case 26:
            salesReportService.ShowSalesSummary();
            break;
        case 27:
            notificationService.SimulateLowStockEmail();
            break;
        case 28:
            notificationService.SimulateOrderCompletedEmail();
            break;
        case 29:
            notificationService.SimulateReceiptEmail();
            break;
        case 30:
            notificationService.ViewNotificationEmail();
            break;
        case 31:
            databaseConnectionService.ResetDatabase();
            basketItems.Clear();

            Console.WriteLine( $"Current database location: {databaseConnectionService.GetDatabaseFilePath()}");
            break;
        case 32:
            loggingService.LogInfo("Stockflow application closed.");
            Console.WriteLine("StockFlow has been closed");
            keepRunning = false;
            break;
        default:
            Console.WriteLine("Invalid option.\n");
            continue;
    }
    
}




