# StockFlow Architecture

## Current Architecture

StockFlow will start as a .NET console application.

Current project:

StockFlow.Console/
├── Models/
│   ├── Product.cs
│   ├── BasketItem.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   ├── Payment.cs
│   ├── Receipt.cs
│   └── StockMovement.cs
├── Services/
│   ├── InventoryService.cs
│   ├── BasketService.cs
│   ├── OrderService.cs
│   ├── PaymentService.cs
│   ├── ReceiptService.cs
│   ├── DashboardService.cs
│   ├── StockMovementService.cs
│   ├── SalesReportService.cs
│   └── AlertService.cs
├── Data/
│   └── JsonStorageService.cs
├── Utilities/
│   └── InputValidationService.cs
└── Program.cs

### Folder Responsibilities
- Models/ Contains business data classes such as Product, Order, OrderItem, Payment, and Receipt.
- Services/ Contains business actions such as inventory management, basket management, checkout, payment processing, receipt generation, dashboard summary, and notifications.
- Data/ Contains storage-related logic such as JSON save and load services. Later, this may contain database access logic.
- Utilities/ Contains reusable helper classes such as input validation.

- Product.cs stores product data.
- InventoryService.cs handles inventory actions.
- InputValidationService.cs handles reusable user input validation.
- Program.cs controls the console menu flow.
- BasketItem.cs represents selected products before checkout.
- BasketService.cs handles basket/cart actions.
- Order.cs represents a completed checkout transaction.
- OrderItem.cs represents products recorded inside an order.
- OrderService.cs manages checkout and order viewing.
- Payment.cs represents a payment record.
- PaymentService.cs handles payment processing and payment viewing.
- Receipt.cs represents proof/output of a completed paid transaction.
- ReceiptServices.cs handles receipt generation, viewing, and printing.
- DashboardServices.cs calculates and displays inventory, order, payment, income, and low-stock summaries. 
- JsonStorageService saves and loads records.
- StockMovement.cs represents one inventory quantity change
- StockMovementService.cs records and display stock movement history
- AlertService.cs identifies and displays low-stock products.
- SalesReportService.cs calculates and displays sales-related summaries.

### Architecture Principle

StockFlow follows separation of concerns.

Models represent data.
Services perform business actions.
Data classes handle storage.
Utilities provide reusable helpers.
Program.cs controls the application flow.

### Future Architecture

Later versions may evolve into:

src/
├── StockFlow.Domain/
├── StockFlow.Application/
├── StockFlow.Infrastructure/
├── StockFlow.Api/
└── StockFlow.Console/

This structure will support database integration, API development, and deployment.


## v0.1.0 Architecture Summary

StockFlow v0.1.0 uses a simple console application architecture.

Current structure:

StockFlow.Console/
├── Models/
├── Services/
├── Data/
├── Utilities/
└── Program.cs

### Layers
- Models contain data structures.
- Services contain business operations.
- Utilities contain reusable helper logic.
- Data contains persistence logic.
- Program.cs coordinates the console menu and service calls.

### Architecture Style

The project currently follows a simple service-based console architecture.

This is not yet a full layered enterprise architecture, but it prepares the project for future separation into:

- Domain layer
- Application layer
- Infrastructure layer
- API layer
- Test project

### Known Architecture Limitations
- Program.cs still coordinates many lists and services.
- Data is stored in JSON files instead of a database.
- No repository pattern yet.
- No dependency injection container yet.
- No automated tests yet
