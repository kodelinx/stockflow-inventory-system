# StockFlow Architecture

## Current Architecture

StockFlow will start as a .NET console application.

Current project:

StockFlow.Console/
├── Models/
│   ├── Product.cs
│   ├── BasketItem.cs
│   ├── Order.cs
│   └── OrderItem.cs
├── Services/
│   ├── InventoryService.cs
│   ├── BasketService.cs
│   └── OrderService.cs
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
