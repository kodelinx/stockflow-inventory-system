# StockFlow Architecture

## Current Architecture

StockFlow will start as a .NET console application.

Current project:

src/StockFlow.Console
Models/Product.cs
Services/InventoryService.cs
Program.cs

### Folder Responsibilities
- Models/ Contains business data classes such as Product, Order, OrderItem, Payment, and Receipt.

- Services/ Contains business actions such as inventory management, basket management, checkout, payment processing, receipt generation, dashboard summary, and notifications.

- Data/ Contains storage-related logic such as JSON save and load services. Later, this may contain database access logic.

- Utilities/ Contains reusable helper classes such as input validation.

- Program.cs Controls the console application flow and calls the appropriate services.

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
