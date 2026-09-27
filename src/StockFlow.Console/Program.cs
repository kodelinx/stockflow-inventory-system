using StockFlow.Services;
using StockFlow.Models;
using StockFlow.Utilities;
using StockFlow.Database;
using StockFlow.Repositories;

ProductManager productManager = new ProductManager();

LoggingService loggingService = new LoggingService();



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
    Console.WriteLine("19. Add Stock");
    Console.WriteLine("20. Adjust Stock");
    Console.WriteLine("21. View Stock movements");
    Console.WriteLine("22. View Low Stock Products");
    Console.WriteLine("23. Export Receipt to Text File");
    Console.WriteLine("24. View Sales Summary Report");
    Console.WriteLine("25. Simulate Low Stock Email");
    Console.WriteLine("26. Simulate Order Completed Email");
    Console.WriteLine("27. Simulate Receipt Email");
    Console.WriteLine("28. View Notifications");
    Console.WriteLine("29. Reset Database");
    Console.WriteLine("30. Exit");

    int option = inputValidationService.GetValidInt("Choose an option: ",  1, 30);
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
            stockMovementService.AddStock();
            break;
        case 20:
            stockMovementService.AdjustStock();
            break;
        case 21:
            stockMovementService.ViewStockMovements();
            break;
        case 22:
            alertService.ShowLowstockAlerts();
            break;
        case 23:
            receiptService.ExportReceiptToTextFile();
            break;
        case 24:
            salesReportService.ShowSalesSummary();
            break;
        case 25:
            notificationService.SimulateLowStockEmail();
            break;
        case 26:
            notificationService.SimulateOrderCompletedEmail();
            break;
        case 27:
            notificationService.SimulateReceiptEmail();
            break;
        case 28:
            notificationService.ViewNotificationEmail();
            break;
        case 29:
            databaseConnectionService.ResetDatabase();
            basketItems.Clear();

            Console.WriteLine( $"Current database location: {databaseConnectionService.GetDatabaseFilePath()}");
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




