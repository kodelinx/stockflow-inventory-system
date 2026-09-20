using StockFlow.Services;
using StockFlow.Models;
using StockFlow.Utilities;
using StockFlow.Data;
using StockFlow.Database;
using StockFlow.Repositories;

ProductManager productManager = new ProductManager();
InputValidationService inputValidationService = new InputValidationService();
BasketService basketService = new BasketService(inputValidationService);
LoggingService loggingService = new LoggingService();
JsonStorageService jsonStorageService = new JsonStorageService(loggingService);
AlertService alertService = new AlertService();
DashboardService dashboardService = new DashboardService(alertService);
SalesReportService salesReportService = new SalesReportService();

// Prepares the SQLite database and creates needed tables.
DatabaseConnectionService databaseConnectionService = new DatabaseConnectionService();
databaseConnectionService.InitializeDatabase();

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

InventoryService inventoryService = new InventoryService(
    inputValidationService, 
    productManager,
    productRepository
);
OrderService orderService = new OrderService(
    orderRepository,
    orderItemRepository
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
        receiptRepository
    );

StockMovementService stockMovementService = new StockMovementService(
    inputValidationService,
    stockMovementRepository,
    productRepository
    
);

NotificationService notificationService = new NotificationService(
    inputValidationService,
    notificationRepository
);



// Loads active products from SQLite instead of JSON.
List<Product> products = productRepository.GetActiveProducts();

// Existing lists can stay for now while we transition.
List<BasketItem> basketItems = new List<BasketItem>();
List<Order> orders = new List<Order>();
List<Payment> payments = new List<Payment>();
List<Receipt> receipts = new List<Receipt>();
List<StockMovement> stockMovements = new List<StockMovement>();
List<Notification> notifications = new List<Notification>();

Console.WriteLine("\nSQLite database initialized successfully.\n");


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
    Console.WriteLine("19. Save Data to JSON");
    Console.WriteLine("20. Reload Data");
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
    Console.WriteLine("31. Exit");

    int option = inputValidationService.GetValidInt("Choose an option: ",  1, 31);
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
            inventoryService.ReactivateProduct(products);
            break;
        case 7:
            inventoryService.DeleteProduct(products);
            break;
        case 8:
            basketService.AddItemToBasket(products, basketItems);
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
            orderService.CheckoutBasket(products, basketItems, orders, stockMovements, stockMovementService);
            break;
        case 13:
            orderService.ViewOrders(orders);
            break;
        case 14:
            paymentService.ProcessPayment(orders, payments);
            break;
        case 15:
            paymentService.ViewPayments(payments);
            break;
        case 16:
            receiptService.GenerateReceipt(receipts);
            break;
        case 17:
            receiptService.ViewReceipts(orders, payments, receipts);
            break;
        case 18:
            dashboardService.ShowDashboard(products, orders, payments);
            break;
        case 19:
             // Transitional only: products are now read from SQLite, but JSON save still exists.
            jsonStorageService.SaveData(products, productsFilePath);
            jsonStorageService.SaveData(orders, ordersFilePath);
            jsonStorageService.SaveData(payments, paymentsFilePath);
            jsonStorageService.SaveData(receipts, receiptsFilePath);
            jsonStorageService.SaveData(stockMovements, stockMovementsFilePath);
            jsonStorageService.SaveData(notifications, notificationFilePath);
            break;
        case 20:
            // Reloads products from SQLite.
            products = productRepository.GetActiveProducts();

            // Orders, payments, and receipts may now also be database-backed.
            orders = orderRepository.GetAllOrders();
            payments = paymentRepository.GetAllPayments();
            receipts = receiptRepository.GetAllReceipts();

            // Reloads stock movements and notifications from SQLite.
            stockMovements = stockMovementRepository.GetAllStockMovements();
            notifications = notificationRepository.GetAllNotifications();
            break;  
        case 21:
            stockMovementService.AddStock(products, stockMovements);
            break;
        case 22:
            stockMovementService.AdjustStock(products, stockMovements);
            break;
        case 23:
            stockMovementService.ViewStockMovements(stockMovements);
            break;
        case 24:
            alertService.ShowLowstockAlers(products);
            break;
        case 25:
            receiptService.ExportReceiptToTextFile(orders, payments, receipts);
            break;
        case 26:
            salesReportService.ShowSalesSummary(orders, payments);
            break;
        case 27:
            notificationService.SimulateLowStockEmail(products, notifications, alertService);
            break;
        case 28:
            orders = orderRepository.GetAllOrders();
            notificationService.SimulateOrderCompletedEmail(orders, notifications);
            break;
        case 29:
            notificationService.SimulateReceiptEmail(receipts,  notifications);
            break;
        case 30:
            notificationService.ViewNotificationEmail(notifications);
            break;
        case 31:
            loggingService.LogInfo("Stockflow application closed.");
            Console.WriteLine("StockFlow has been closed");
            keepRunning = false;
            break;
        default:
            Console.WriteLine("Invalid option.\n");
            continue;
    }
    
}




