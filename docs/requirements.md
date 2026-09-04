# StockFlow Requirements

## Functional Requirements

### Inventory Management

- The system should allow users to add products. (v0.1.0: M02 Implemented - Console Version)
- The system should allow users to view products. (v0.1.0: M01 Implemented - Console Version)
- The system should allow users to search products. (v0.1.0: MO2 Implemented - Console Version)
- The system should allow users to update product details. (v0.1.0: MO2 Implemented - Console Version)
- The system should allow users to deactivate products. (v0.1.0: MO2 Implemented - Console Version)
- The system should allow users to reactivate products. (v0.2.0: m17 Implemented - Console Version)
- The system should allow users to delete products. (v0.1.0: MO2 Implemented - Console Version)
- The system should track product stock quantity. (v0.1.0: MO2 Implemented - Console Version)
- The system should identify low-stock products. (v0.2.0: M12 Implemented - Console Version)

### Basket and Orders

- The system should allow users to add products to a basket. (v0.1.0: MO4 Implemented - Console Version)
- The system should allow users to remove products from the basket. (v0.1.0: MO4 Implemented - Console Version)
- The system should calculate basket totals. (v0.1.0: MO4 Implemented - Console Version)
- The system should convert a basket into an order. (v0.1.0: MO5 Implemented - Console Version)
- The system should reduce stock after checkout. (v0.1.0: MO5 Implemented - Console Version)
- The system should prevent checkout when stock is insufficient. (v0.1.0: MO5 Implemented - Console Version)

### Payments

- The system should record payment method. (v0.1.0: MO6 Implemented - Console Version)
- The system should validate payment amount. (v0.1.0: MO6 Implemented - Console Version)
- The system should calculate change for cash payments. (v0.1.0: MO6 Implemented - Console Version)
- The system should track paid and unpaid orders. (v0.1.0: MO6 Implemented - Console Version)

### Receipts

- The system should generate receipts after checkout. (v0.1.0: MO7 Implemented - Console Version)
- The receipt should display ordered items, quantities, prices, total, payment method, amount paid, and change. (v0.1.0: MO7 Implemented - Console Version)
- The system should print out receipts as text file (v0.2.0: M13 Implemented - Console Version)

### Dashboard

- The system should show total products. (v0.1.0: MO8 Implemented - Console Version)
- The system should show available stock. (v0.1.0: MO8 Implemented - Console Version)
- The system should show low-stock items. (v0.1.0: MO8 Implemented - Console Version)
- The system should show total sales. (v0.1.0: MO8 Implemented - Console Version)
- The system should show total income. (v0.1.0: MO8 Implemented - Console Version)

### Notifications

- The system should notify the owner when a new order is created.
- The first version may simulate email notification before real email integration. (v0.2.0: M15 Implemented - Console Version - Simulation Only)
- The system creates simulated low-stock notification records. (v0.2.0: M15 Implemented - Console Version - Simulation Only)
- The system creates simulated completed order notification records.  (v0.2.0: M15 Implemented - Console Version - Simulation Only)
- The system creates simulated receipt notification records. (v0.2.0: M15 Implemented - Console Version - Simulation Only)
- The system stores notification type, recipient, subject, message, creation date, and status. (v0.2.0: M15 Implemented - Console Version - Simulation Only)
- The system allows users to view notification history. (v0.2.0: M15 Implemented - Console Version)
- The system saves and loads notification records through JSON persistence. (v0.2.0: M15 Implemented - Console Version)

### Reports

- The system should record and view the increase, adjustments, and reductions of stock quantity of a product.(v0.2.0: M11 Implemented - Console Version)
- The system should present orders and payment summaries for sales report (v0.2.0: M14 Implemented - Console Version)

## Non-Functional Requirements

- The system should validate user input. (v0.1.0: MO3 Implemented - Console Version)
- The system records informational log entries. (v0.2.0: M16 Implemented - Console Version)
- The system records error log entries. (v0.2.0: M16 Implemented - Console Version)
- The system writes logs to a text file. (v0.2.0: M16 Implemented - Console Version)
- The system creates the log folder when needed. (v0.2.0: M16 Implemented - Console Version)
- The system logs JSON save success events. (v0.2.0: M16 Implemented - Console Version)
- The system logs JSON load success events. (v0.2.0: M16 Implemented - Console Version)
- The system logs JSON save/load failures. (v0.2.0: M16 Implemented - Console Version)
- The system continues running when logging fails. (v0.2.0: M16 Implemented - Console Version)
- The system should use clear code structure.
- The system should separate models, services, data, and utilities. (v0.1.0: MO9 Implemented - Console Version)
- The system should save data so it is not lost after closing the app. (v0.1.0: MO9 Implemented - Console Version)
- The system should be documented through README and project docs.
- The system should be version-controlled using Git and GitHub.

## Database Storage

- The system should define database tables for products, orders, payments, receipts, stock movements, and notifications.
- The system should support database-backed save and load operations.
- The system should replace local JSON storage with database persistence.
- The system should use safer ID generation through database identifiers.
- The system should support SQL CRUD operations.

### Database planning
Before creating tables or writing SQL, we  should understand what business data needs to be stored and why. Database design should come from business requirements, not just from copying current C# classes.
- Store Products
- Store Orders
- Store Order Items
- Store Payments
- Store Receipts
- Store Stcok Movements
- Store Notifications
- Support Active and Inactive Products
- Preseerve Historical Transaction data
- Support Future Reports

#### Database Entities Needed
Main Tables:
- Products
- Orders
- OrderItems
- Payments
- Receipts
- StockMovements
- Notifications

Later Future Tables:
- Users
- Roles
- Customers
- Suppliers
- Categories
- AuditLogs

#### Relationship Planning
- One Product can appear in many OrderItems.
- One Order has many OrderItems.
- One Order can have one Payment.
- One Payment can have one Receipt.
- One Product can have many StockMovements.
- One Product can trigger many Notifications indirectly.

#### Database Requirement Rules
- Define database tables. (v0.3.0: M19 Implemented - Console Version)
- Define primary keys. (v0.3.0: M19 Implemented - Console Version)
- Define foreign keys. (v0.3.0: M19 Implemented - Console Version)
- Define relationships between tables. (v0.3.0: M19 Implemented - Console Version)
- Write SQL CRUD scripts. (v0.3.0: M20 Implemented - Console Version)
- Integrate SQLite. (v0.3.0: M21 Implemented - Console Version)
- Create an actual SQLite database. (v0.3.0: M22 Implemented - Console Version)
- Run the SQL scripts against SQLite. (v0.3.0: M22 Implemented - Console Version)
- Connect the C# application to the database. (v0.3.0: M22 Implemented - Console Version)
- Replace JSON persistence with repository-based database access. (v0.3.0: M22 Implemented - Console Version)
- Introduce repository pattern. (v0.3.0: M22 Implemented - Console Version)

## v0.3.0 Requirements Status

The following requirements are implemented or planned in v0.3.0:

### Database Planning

- The system identifies the main data entities needed for database storage.
- The system documents planned database tables.
- The system documents primary keys and foreign keys.
- The system documents relationships between products, orders, order items, payments, receipts, stock movements, and notifications.
- The system preserves historical transaction data through order item snapshot fields.

### SQL Preparation

- The system includes SQL scripts for creating planned tables.
- The system includes SQL examples for create, read, update, and delete operations.
- The system includes SQL examples for product deactivation and reactivation.
- The system includes SQL examples for basic reporting queries.

### SQLite Integration

- The system includes SQLite package support.
- The system includes a database connection service.
- The system can initialize a local SQLite database file.
- The system can create the Products table if it does not exist.
- The system excludes generated SQLite database files from Git tracking.

### Repository Pattern

- The system introduces a repository layer for database access.
- The system includes `ProductRepository`.
- The system can insert product records through the repository.
- The system can read product records through the repository.
- The system can search products by product code through the repository.
- SQL commands use parameters instead of direct string interpolation.

Remaining planned work:

- Convert full product CRUD menu flow to SQLite.
- Add update, deactivate, reactivate, and delete methods in `ProductRepository`.
- Add repositories for other models.
- Reduce dependency on JSON persistence.
- Add automated tests.

## Web API Requirements

Implemented setup in M24:

- The system should include an ASP.NET Core Web API project.
- The API project should be part of the StockFlow solution.
- The API should run locally during development.
- The API should expose a sample endpoint for initial testing.
- The API should expose an OpenAPI document for endpoint discovery.
- The API should prepare the project for future product, order, payment, and dashboard endpoints.

Planned for future milestones:

- Add product API endpoints.
- Add order API endpoints.
- Add payment API endpoints.
- Add dashboard API endpoints.
- Add API validation and error responses.
- Connect API endpoints to services and repositories.
- Add Swagger UI or Scalar UI later for interactive browser testing.

## v0.1.0 Requirements Status

The following requirements are implemented in v0.1.0:

### Inventory
- Add products
- View products
- Search products
- Update products
- Deactivate products
- Delete products

### Basket
- Add items to basket
- View basket
- Remove items from basket
- Clear basket
- Calculate basket total

### Orders
- Checkout basket
- Create order records
- Create order item records
- View orders
- Reduce stock after checkout

### Payments
- Process payment
- Select payment method
- Validate amount paid
- Calculate change
- View payments

### Receipts
- Generate receipts
- View receipts
- Prevent duplicate receipt generation

### Dashboard
- Show inventory summary
- Show order summary
- Show payment summary
- Show income summary
- Show low-stock products

### Storage
- Save products/orders/payments/receipts to JSON
- Load products/orders/payments/receipts from JSON


## v0.2.0 Requirements Status

The following requirements are implemented in v0.2.0:

### Stock Movement Tracking

- The system records stock increases.
- The system records stock adjustments.
- The system records stock reductions caused by checkout.
- The system stores stock before and stock after values.
- The system records reasons for stock movements.
- The system allows users to view stock movement history.

### Low-Stock Alerts

- The system identifies active products with low stock.
- The system treats a product as low stock when `QuantityInStock <= ReorderLevel`.
- The system excludes inactive products from low-stock alerts.
- The system allows users to view low-stock alerts.

### Receipt Export

- The system exports generated receipts to text files.
- The system creates a receipt folder when needed.
- The system uses receipt data, order data, and payment data for export.
- The system handles export errors safely.

### Sales Reports

- The system displays total orders.
- The system displays completed and pending orders.
- The system calculates total sales income.
- The system calculates total cash received.
- The system calculates total change given.
- The system displays sales by payment method.

### Notifications

- The system creates simulated low-stock notification records.
- The system creates simulated completed order notification records.
- The system creates simulated receipt notification records.
- The system allows users to view notification history.
- The system saves and loads notifications through JSON persistence.

### Logging

- The system records informational log entries.
- The system records error log entries.
- The system writes logs to a text file.
- The system logs JSON save/load events.
- The system logs JSON save/load failures.

### Inventory
- Reactivate products


## v0.3.0 Requirements Status

The following requirements are implemented or planned in v0.3.0:

### Database Planning

- The system identifies the main data entities needed for database storage.
- The system documents planned database tables.
- The system documents primary keys and foreign keys.
- The system documents relationships between products, orders, order items, payments, receipts, stock movements, and notifications.
- The system preserves historical transaction data through order item snapshot fields.

### SQL Preparation

- The system includes SQL scripts for creating planned tables.
- The system includes SQL examples for create, read, update, and delete operations.
- The system includes SQL examples for product deactivation and reactivation.
- The system includes SQL examples for basic reporting queries.

### SQLite Integration

- The system includes SQLite package support.
- The system includes a database connection service.
- The system can initialize a local SQLite database file.
- The system can create the Products table if it does not exist.
- The system excludes generated SQLite database files from Git tracking.

### Repository Pattern

- The system introduces a repository layer for database access.
- The system includes `ProductRepository`.
- The system can insert product records through the repository.
- The system can read product records through the repository.
- The system can search products by product code through the repository.
- SQL commands use parameters instead of direct string interpolation.

Remaining planned work:

- Convert full product CRUD menu flow to SQLite.
- Add update, deactivate, reactivate, and delete methods in `ProductRepository`.
- Add repositories for other models.
- Reduce dependency on JSON persistence.
- Add automated tests.

## Product API Requirements

Implemented in M25:

- The API should allow clients to view products.
- The API should allow clients to search a product by product code.
- The API should return product data as JSON.
- The API should return 404 when a product code does not exist.

Planned:

- Connect product endpoints to SQLite.
- Add create product endpoint.
- Add update product endpoint.
- Add deactivate/reactivate product endpoints.
- Add API validation and standardized error responses.