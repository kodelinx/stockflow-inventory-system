# StockFlow Release Notes

Last updated: 2026-09-05

## Purpose

This document records what changed in each released version of StockFlow.

Milestone tracking belongs in `milestone-plan.md`.  
Requirements belong in `requirements.md`.

---

# v0.1.0 - Console Inventory and Sales MVP

Release Date: 2026-08-23

## Summary

StockFlow v0.1.0 is the first working console-based MVP of the inventory and sales management system.

This release includes inventory management, basket management, checkout, payment processing, receipt generation, dashboard summaries, and JSON file persistence.

## Completed Features

- Product model and inventory listing
- Product CRUD operations
- Product search by name or product code
- Product update
- Product soft delete through deactivation
- Product hard delete for cleanup/testing
- Input validation service
- Basket item management
- Basket total calculation
- Checkout and order creation
- Order item records
- Payment processing
- Payment method selection
- Change calculation
- Receipt generation
- Dashboard summary
- JSON save and load support

## Technical Improvements

- Separated models, services, utilities, and data storage
- Used service classes for business operations
- Used JSON serialization and deserialization
- Added reusable generic storage methods
- Added validation for common invalid inputs
- Added basic error handling for file operations

## Known Limitations

- Console application only
- Data is stored in local JSON files
- No database yet
- No user login or role-based access yet
- No automated tests yet
- No web API yet
- No receipt export to PDF/text yet
- ID generation is still based on list counts
- No date-based reports yet

---

# v0.2.0 - Inventory Rules and Reporting

Release Date: 2026-08-29

## Summary

StockFlow v0.2.0 improves the console MVP by adding inventory traceability, low-stock alerting, receipt export, sales reporting, notification simulation, and basic logging preparation.

## Completed Features

- Stock movement tracking
- Stock-in records
- Stock adjustment records
- Automatic stock-out records during checkout
- Low-stock alert viewing
- Receipt text file export
- Sales summary reports
- Sales by payment method
- Completed order details report
- Simulated low-stock email notifications
- Simulated order completed email notifications
- Simulated receipt email notifications
- Notification history
- Notification JSON persistence
- Basic logging service
- JSON save/load event logging
- JSON save/load error logging

## Technical Improvements

- Added StockMovement model
- Added Notification model
- Added StockMovementService
- Added AlertService
- Added SalesReportService
- Added NotificationService
- Added LoggingService
- Improved separation of concerns
- Added basic application log file output
- Improved troubleshooting support for JSON storage

## Known Limitations

- Console application only
- Local JSON file storage only
- No database yet
- No real email sending yet
- No PDF receipt export yet
- No authentication or user roles yet
- No automated tests yet
- No web API yet
- No advanced reporting filters yet
- No structured logging or log rotation yet

---

# v0.3.0 - Database-Ready Inventory System

Release Date: 2026-09-02

## Summary

StockFlow v0.3.0 prepares the application for database-backed storage. This version adds database requirements, database table design, SQL CRUD scripts, SQLite initialization, and the first repository class for product database access.

## Completed Features

- Defined database requirements
- Designed planned database tables
- Created SQL CRUD scripts
- Added SQLite package support
- Created DatabaseConnectionService
- Added SQLite database initialization
- Added automatic Products table creation
- Created ProductRepository
- Added product insert database method
- Added product read database methods
- Added product search by product code
- Added SQL parameters for safer database commands
- Added SQLite database file ignore rules

## Technical Improvements

- Introduced database-ready project direction
- Introduced repository pattern
- Separated database connection setup
- Separated product database access logic
- Prepared future repository-based storage
- Preserved JSON storage temporarily during migration

## Known Limitations

- The full app is not yet database-backed.
- Product menu operations are not fully using SQLite yet.
- JSON persistence still exists.
- Only Products table is initialized from C#.
- Only ProductRepository has been started.
- Other repositories are not implemented yet.
- No Web API yet.
- No automated tests yet.

---

# v0.4.0 - StockFlow Web API

Release Date: 2026-09-11

## Summary

StockFlow v0.4.0 introduces ASP.NET Core Web API support. The goal is to expose core StockFlow features through HTTP endpoints.

## Progress

### M24 - ASP.NET Core Web API Setup

Status: Completed

Completed:

- Created StockFlow.Api project
- Added API project to the StockFlow solution
- Confirmed API project builds
- Confirmed API project runs locally
- Tested sample `/weatherforecast` endpoint
- Tested OpenAPI document access through `/openapi/v1.json`
- Reviewed basic ASP.NET Core Web API startup flow

Known limitations:

- Swagger UI is not configured yet.
- Custom StockFlow endpoints were not part of M24.

### M25 - Product API Endpoints

Status: Completed

Completed:

- Created `ProductsController`
- Added `GET /api/products`
- Added `GET /api/products/{productCode}`
- Connected product endpoints to `ProductRepository`
- Retrieved active products through repository-based data access
- Returned product data as JSON
- Added `404 Not Found` response for missing product codes
- Confirmed product API routes work locally

Known limitations:

- Product API currently supports read operations only
- Create, update, delete, and deactivate product endpoints are not yet implemented
- API authentication and authorization are not yet implemented
- Shared architecture cleanup is planned for a future version

### M26 - Order API Endpoints

Status: Completed

Completed:

- Created `OrdersController`
- Added `GET /api/orders`
- Added `GET /api/orders/{orderNumber}`
- Returned order data as JSON
- Added `404 Not Found` response for missing order numbers
- Used typed temporary sample order data for API testing
- Confirmed order API routes work locally

Known limitations:

- Order API currently supports read operations only
- Order endpoints use temporary sample data
- Order endpoints are not yet connected to SQLite
- Create, update, and checkout order API endpoints are not yet implemented
- API authentication and authorization are not yet implemented

## Expected v0.4.0 Outcome

By the end of v0.4.0, StockFlow should have:

- Working API project
- Product API endpoints
- Order API endpoints
- Payment API endpoints
- Dashboard API endpoints
- Basic API validation
- Basic API error responses
- OpenAPI document access

## Expected v0.4.0 Limitations

- API may still use temporary sample data.
- Full SQLite-backed API integration may not be complete.
- Console app may still contain the main business flow.
- Authentication is not yet implemented.
- Frontend UI is not yet implemented.
- Swagger UI or Scalar UI is not yet configured unless added later.

### M27 - Payment API Endpoints

Status: Completed

Completed:

- Created PaymentsController
- Added GET /api/payments
- Added GET /api/payments/{paymentNumber}
- Returned payment data as JSON
- Added 404 Not Found response for missing payment numbers
- Used typed temporary sample payment data for API testing

Pending verification:

- Confirm GET /api/payments returns payment records
- Confirm GET /api/payments/PAY-001 returns one payment
- Confirm GET /api/payments/PAY-999 returns 404 Not Found

Known limitations:

- Payment API currently supports read operations only
- Payment endpoints use temporary sample data
- Payment endpoints are not yet connected to SQLite
- Payment processing through API is not yet implemented
- API authentication and authorization are not yet implemented

### M28 - Dashboard API Endpoints

Status: Completed

Completed:

- Created DashboardController
- Added GET /api/dashboard/summary
- Returned dashboard summary data as JSON
- Used typed temporary sample dashboard data for API testing

Pending verification:

- Confirm GET /api/dashboard/summary returns dashboard summary data
- Confirm response includes inventory summary values
- Confirm response includes order summary values
- Confirm response includes payment and income summary values

Known limitations:

- Dashboard API currently supports read-only summary data
- Dashboard endpoint uses temporary sample data
- Dashboard endpoint is not yet connected to DashboardService
- Dashboard endpoint is not yet connected to SQLite
- API authentication and authorization are not yet implemented

### M29 - API Validation and Error Responses

Status: Completed

Completed:

- Added basic route parameter validation
- Added `400 Bad Request` response for invalid required values
- Confirmed `404 Not Found` behavior for missing products
- Confirmed `404 Not Found` behavior for missing orders
- Confirmed `404 Not Found` behavior for missing payments
- Clarified that empty list responses should return `200 OK`

Pending verification:

- Confirm product search response behavior
- Confirm order search response behavior
- Confirm payment search response behavior
- Confirm dashboard summary still returns `200 OK`

Known limitations:

- Validation is still basic
- Full request body validation is not yet implemented
- Create, update, and delete endpoints are not yet implemented
- Authentication and authorization are not yet implemented

## v0.4.0 - StockFlow Web API

Release date: 2026-09-10

Status: Released

### Summary

v0.4.0 introduced the ASP.NET Core Web API layer for StockFlow.

This release added API endpoints for products, orders, payments, and dashboard summary data. It also introduced basic API response handling using standard HTTP responses such as 200 OK, 400 Bad Request, and 404 Not Found.

### Completed Milestones

- M24 - ASP.NET Core Web API Setup
- M25 - Product API Endpoints
- M26 - Order API Endpoints
- M27 - Payment API Endpoints
- M28 - Dashboard API Endpoints
- M29 - API Validation and Error Responses
- M30 - v0.4.0 Release

### Added

- Added `StockFlow.Api` ASP.NET Core Web API project
- Added OpenAPI JSON support through `/openapi/v1.json`
- Added `ProductsController`
- Added `OrdersController`
- Added `PaymentsController`
- Added `DashboardController`
- Added `GET /api/products`
- Added `GET /api/products/{productCode}`
- Added `GET /api/orders`
- Added `GET /api/orders/{orderNumber}`
- Added `GET /api/payments`
- Added `GET /api/payments/{paymentNumber}`
- Added `GET /api/dashboard/summary`

### Improved

- Added basic route parameter validation
- Added clearer API error responses
- Used `200 OK` for successful requests
- Used `400 Bad Request` for invalid input where applicable
- Used `404 Not Found` for missing records
- Clarified that empty list responses are successful responses

### Current Limitations

- The API is not production-ready yet
- Product endpoints use repository-backed SQLite access
- Order endpoints use typed temporary sample data
- Payment endpoints use typed temporary sample data
- Dashboard endpoint uses typed temporary sample data
- Create, update, and delete API endpoints are not yet implemented
- Authentication and authorization are not yet implemented
- Full shared architecture cleanup is planned for v0.5.0
- Full database-backed flow is planned for v0.6.0

## v0.5.0 - Shared Architecture and Full API Integration

Status: Released

### M31 - Create StockFlow.Core Class Library

Status: Completed

Completed:

- Created `StockFlow.Core` class library project
- Added `StockFlow.Core` to the solution
- Added project reference from `StockFlow.Console` to `StockFlow.Core`
- Added project reference from `StockFlow.Api` to `StockFlow.Core`
- Created initial Core folders for Models, Services, and Interfaces

Known limitations:

- Shared models have not been moved yet
- Shared services have not been moved yet
- Repository and database logic are still outside Infrastructure

### M32 - Move Models to StockFlow.Core

Status: Completed

Completed:

- Moved shared model classes to StockFlow.Core
- Kept model namespaces stable under StockFlow.Models
- Confirmed shared models can be used by StockFlow.Api and StockFlow.Console
- Removed duplicate model definitions from StockFlow.Console

Known limitations:

- Shared services have not been extracted yet
- Some business logic remains inside console workflow services

### M33 - Service Layer Assessment

Status: Completed

Completed:

- Reviewed the current service layer
- Identified that existing services depend heavily on InputValidationService
- Confirmed that current services are still console workflow services
- Decided not to move services directly into StockFlow.Core
- Planned future extraction of pure business logic into Core services

Outcome:

- Services remain in StockFlow.Console for now
- InputValidationService remains in StockFlow.Console
- Pure business service extraction is planned for M36

Known limitations:

- Current services still contain console input/output logic
- API endpoints are not yet connected to reusable Core business services

### M34 - Create StockFlow.Infrastructure Class Library

Status: Completed

Completed:

- Created StockFlow.Infrastructure class library project
- Added StockFlow.Infrastructure to the solution
- Added reference from StockFlow.Infrastructure to StockFlow.Core
- Added references from StockFlow.Api and StockFlow.Console to StockFlow.Infrastructure
- Installed Microsoft.Data.Sqlite in StockFlow.Infrastructure

Known limitations:

- Only current database/repository logic has been moved so far
- Other repositories are not yet implemented

### M35 - Move Database and Repository Logic to StockFlow.Infrastructure

Status: Completed

Completed:

- Moved DatabaseConnectionService to StockFlow.Infrastructure
- Moved ProductRepository to StockFlow.Infrastructure
- Removed StockFlow.Api dependency on StockFlow.Console
- Resolved duplicate Product type issue
- Resolved circular project reference issue
- Confirmed the project builds after architecture cleanup

Known limitations:

- Services remain in StockFlow.Console because they are console-heavy
- OrderRepository, PaymentRepository, ReceiptRepository, and other repositories are not yet implemented

### M36 - Extract Product Business Service to StockFlow.Core

Status: Completed

Completed:

- Created `ProductManager` in `StockFlow.Core`
- Added reusable product business logic without console input/output
- Added product creation logic through clean parameters
- Added low-stock checking logic
- Added enough-stock checking logic
- Added product deactivation logic
- Kept console-heavy services inside `StockFlow.Console`

Pending verification:

- Run `dotnet build`
- Confirm console project still builds
- Confirm API project still builds
- Optionally connect `InventoryService` to `ProductManager`

Known limitations:

- Not all service logic has been extracted yet
- Existing console services still contain input/output workflow
- API endpoints are not yet fully connected to Core business services

### M37 - Connect Product API to Core and Infrastructure

Status: Completed

Completed:

- Registered `ProductManager` in the API dependency injection container
- Injected `ProductManager` into `ProductsController`
- Kept `ProductRepository` as the product database access class
- Used `ProductManager.IsLowStock()` inside the Product API response
- Improved the Product API flow to use both Core and Infrastructure layers

Pending verification:

- Run `dotnet build`
- Confirm `GET /api/products` still works
- Confirm `GET /api/products/P001` returns one product
- Confirm `GET /api/products/P001` includes `isLowStock`
- Confirm `GET /api/products/P999` returns `404 Not Found`

Known limitations:

- Only product endpoints are connected to Core and Infrastructure
- Order, payment, and dashboard endpoints still use temporary sample data
- Create, update, and delete API endpoints are not yet implemented

## v0.5.0 - Shared Architecture and Full API Integration

Release date: 2026-09-13

Status: Released

### Summary

v0.5.0 improved StockFlow’s architecture by separating the project into clearer layers.

This release introduced `StockFlow.Core` for shared business models and pure business logic, and `StockFlow.Infrastructure` for database and repository implementation. It also removed the API dependency on the Console project and connected the Product API to both Core and Infrastructure.

### Added

- Added `StockFlow.Core`
- Added `StockFlow.Infrastructure`
- Added `ProductManager`
- Added Core project references where needed
- Added Infrastructure project references where needed

### Changed

- Moved shared models to `StockFlow.Core`
- Moved `DatabaseConnectionService` to `StockFlow.Infrastructure`
- Moved `ProductRepository` to `StockFlow.Infrastructure`
- Updated Product API flow to use Core business logic and Infrastructure repository access
- Removed the API dependency on `StockFlow.Console`

### Fixed

- Fixed duplicate `Product` type issue
- Fixed circular project reference issue
- Fixed incorrect dependency direction between projects
- Fixed product API architecture by removing Console dependency

### Service Layer Decision

Current console services remain in `StockFlow.Console` because they depend heavily on `InputValidationService`, `Console.ReadLine`, and `Console.WriteLine`.

Future Core services should be extracted gradually. They should receive clean parameters, apply business rules, and return results without directly depending on console input/output.

### Current Limitations

- Existing console services still contain console workflow logic
- Only product business logic has been extracted into Core
- Only ProductRepository currently exists
- Other repositories are not yet implemented
- Full database-backed business flow is planned for v0.6.0

## v0.6.0 - Full Database-Backed StockFlow

Status: In Progress

### M39 - Complete Product Repository CRUD

Status: Completed

Completed:

- Reviewed existing ProductRepository database methods
- Added or confirmed product create/read methods
- Added product update method
- Added product deactivate method for soft delete
- Added product delete method for hard delete
- Kept ProductRepository inside StockFlow.Infrastructure

Pending verification:

- Run dotnet build
- Confirm existing product API endpoints still work
- Confirm product repository methods compile successfully

Known limitations:

- Product CRUD is repository-level only for now
- Full API create/update/delete endpoints are not yet implemented
- Console product flow is not yet fully database-backed
- Other repositories are not yet implemented

### M40 - Add Order Repository

Status: Completed

Completed:

- Added Orders table initialization
- Created OrderRepository in StockFlow.Infrastructure
- Added AddOrder method
- Added GetAllOrders method
- Added FindOrderByNumber method
- Added UpdateOrderStatus method
- Added UpdatePaymentStatus method
- Added MapReaderToOrder helper method
- Registered OrderRepository for dependency injection

Pending verification:

- Run dotnet build
- Confirm the API project still builds
- Confirm the Console project still builds

Known limitations:

- OrderRepository currently handles order summary records only
- OrderItems are not yet saved through this repository
- Console order flow is not yet fully connected to SQLite
- API order endpoints may still use temporary sample data

### M41 - Add OrderItem Repository

Status: Completed

Completed:

- Added OrderItems table initialization
- Created OrderItemRepository in StockFlow.Infrastructure
- Added AddOrderItem method
- Added GetOrderItemsByOrderId method
- Added MapReaderToOrderItem helper method
- Registered OrderItemRepository for dependency injection

Known limitations:

- Order items are not yet connected to the full checkout flow
- Console order flow is not yet fully SQLite-backed
- API order endpoints may still use temporary sample data

### M42 - Add Payment Repository

Status: Completed

Completed:

- Added Payments table initialization
- Created PaymentRepository in StockFlow.Infrastructure
- Added AddPayment method
- Added GetAllPayments method
- Added FindPaymentByNumber method
- Added GetPaymentsByOrderNumber method
- Added MapReaderToPayment helper method
- Registered PaymentRepository for dependency injection

Business notes:

- FindPaymentByNumber is used to retrieve one exact payment transaction.
- GetPaymentsByOrderNumber is used to retrieve all payment records connected to one order.
- This design supports future partial payments, split payments, and payment history tracking.

Known limitations:

- PaymentRepository is repository-level only for now
- Payment processing is not yet fully connected to the checkout flow
- API payment endpoints may still use temporary sample data
- Full SQLite checkout/payment flow is planned for a later milestone

### M43 - Add Receipt Repository

Status: In Progress

Completed:

- Added Receipts table initialization
- Created ReceiptRepository in StockFlow.Infrastructure
- Added AddReceipt method
- Added GetAllReceipts method
- Added FindReceiptByNumber method
- Added GetReceiptsByOrderNumber method
- Added MapReaderToReceipt helper method
- Registered ReceiptRepository for dependency injection

Business notes:

- ReceiptRepository stores transaction proof after payment.
- Receipts are linked to both orders and payments.
- ReceiptNumber identifies one issued receipt.
- OrderNumber can be used to retrieve receipts connected to one order.

Known limitations:

- ReceiptRepository is repository-level only for now
- Receipt generation is not yet fully connected to the checkout flow
- API receipt endpoints may still use temporary sample data
- Full SQLite checkout/payment/receipt flow is planned for a later milestone

### M44 - Add StockMovement Repository

Status: In Progress

Completed:

- Added StockMovements table initialization
- Created StockMovementRepository in StockFlow.Infrastructure
- Added AddStockMovement method
- Added GetAllStockMovements method
- Added GetStockMovementsByProductCode method
- Added GetStockMovementsByReferenceNumber method
- Added MapReaderToStockMovement helper method
- Registered StockMovementRepository for dependency injection

Business notes:

- StockMovementRepository stores inventory movement history.
- QuantityChanged uses positive values for stock increases.
- QuantityChanged uses negative values for stock decreases.
- ReferenceNumber can connect a movement to an order, receipt, or adjustment.
- Stock movement history helps explain why product quantity changed.

Known limitations:

- StockMovementRepository is repository-level only for now
- Product stock updates are not yet fully connected to SQLite
- Checkout stock-out flow is not yet fully database-backed
- Full SQLite inventory flow is planned for a later milestone

### M45 - Add Notification Repository

Status: In Progress

Completed:

- Added Notifications table initialization
- Created or confirmed Notification model in StockFlow.Core
- Created NotificationRepository in StockFlow.Infrastructure
- Added AddNotification method
- Added GetAllNotifications method
- Added GetUnreadNotifications method
- Added MarkAsRead method
- Added MapReaderToNotification helper method
- Registered NotificationRepository for dependency injection

Business notes:

- NotificationRepository stores system alerts and business messages.
- Notifications can support low stock warnings, payment reminders, order updates, and other system messages.
- IsRead tracks whether the notification has already been viewed.
- RelatedReference can optionally connect a notification to a product, order, payment, receipt, or stock movement.

Known limitations:

- NotificationRepository is repository-level only for now
- Automatic notification generation is not yet connected
- API notification endpoints are not yet implemented
- Full SQLite business flow is planned for a later milestone

### M46 - Replace JSON Flow with SQLite Flow

Status: In Progress

Completed:

- Started migration from JSON/list storage to SQLite-backed repository flow
- Fixed SQLite table creation syntax by using CREATE TABLE IF NOT EXISTS
- Added ExecuteNonQuery helper method for repeated non-query SQL commands
- Confirmed database initialization should run before repository usage
- Began preparing Console app to use SQLite repositories

Business notes:

- SQLite is becoming the main storage system for StockFlow.
- JSON storage is being phased out gradually.
- Database tables must be initialized before product, order, payment, receipt, stock movement, or notification data can be used.

Technical notes:

- CREATE TABLE IF NOT EXISTS is the correct SQLite syntax.
- CREATE TABLE IF NOT EXISTING is invalid and causes a SQLite syntax error.
- dotnet build compiles the project but does not normally create the database.
- dotnet run starts the app and executes InitializeDatabase().

Known limitations:

- Full JSON-to-SQLite flow replacement is still in progress.
- Product add/update/deactivate flow may still use old list or JSON logic.
- Orders, payments, receipts, stock movements, and notifications are not yet fully connected to SQLite flow.
- Database path is still relative and may depend on where the app is run from.

### M46.1 - Connect Product Read Flow to SQLite

Status: Completed

Completed:

- Began connecting the Console app to SQLite repository flow
- Initialized DatabaseConnectionService from the Console app
- Created ProductRepository in the Console app
- Replaced initial product loading with ProductRepository.GetActiveProducts()
- Updated reload flow so products are reloaded from SQLite
- Removed incorrect ProductHeaderValue usage from product list setup
- Kept JSON flow temporarily for non-product records during transition

Business notes:

- Product records are now read from SQLite for the Console product display flow.
- SQLite is becoming the main product data source.
- List<Product> still exists as a temporary in-memory working copy after reading from the database.

Known limitations:

- Product add/update/deactivate flow may still use older list logic
- Orders, payments, receipts, stock movements, and notifications are not yet fully connected to SQLite flow
- JSON services may still exist during the transition
- Product reload may discard unsaved list-only product changes until product write operations are connected to SQLite