# StockFlow Milestone Plan

## Version Plan

- v0.1.0 - Console Inventory and Sales MVP
- v0.2.0 - Inventory Rules and Reporting
- v0.3.0 - Database-Ready Inventory System
- v0.4.0 - StockFlow Web API
- v1.0.0 - Business MVP Release

## v0.1.0 - Console Inventory and Sales MVP

Goal:

Build the first usable console version of StockFlow.

Milestones:

### M00 - Project Initialization - Completed
### M01 - Product Model and Inventory Basics - Completed
#### Completed:
- Created `Product` model
- Added product properties such as product code, name, category, price, stock quantity, reorder level, and active status
- Created `InventoryService`
- Added `ViewProducts()` method

#### Known limitations:
- Pre-filled data, no input prompts

### M02 - Inventory CRUD Operations - Completed
#### Completed:
- Add product
- View active products
- Search product by name or product code
- Update product details
- Deactivate product using soft delete

#### Known limitations:
- Input validation is still basic.
- Data is still stored in memory.
- Product ID generation is temporary.

### M03 - Service Structure and Input Validation - Completed
#### Completed:
- Created `InputValidationService`
- Added `GetRequiredText()`
- Added `GetValidInt()`
- Added `GetValidDecimal()`
- Added `GetValidMenuOption()`
- Updated inventory CRUD methods and main program menu option to use validation helpers
- Prevented invalid numeric inputs from crashing the app

#### Known limitations:
- Product ID generation is still based on list count.
- Data is still stored only in memory.
- Hard delete should be used only for exceptional cleanup scenarios.

### M04 - Basket Management - Completed
#### Completed:
- Created `BasketItem` model
- Created `BasketService`
- Added products to basket using product code
- Viewed basket contents
- Removed basket items
- Cleared basket
- Calculated basket total
- Prevented adding inactive products
- Prevented adding quantities greater than available stock

#### Known limitations:
- Basket data is temporary and stored in memory.
- Checkout has not been implemented yet.
- Stock is not reduced until the checkout milestone.

### M05 - Checkout and Order Creation
#### Completed:
- Created `Order` model
- Created `OrderItem` model
- Created `OrderService`
- Added basket checkout
- Generated order numbers
- Calculated order totals
- Reduced product stock after successful checkout
- Cleared basket after checkout
- Added order viewing

#### Known limitations:
- Orders are stored in memory only.
- Payment processing is not yet implemented.
- Order ID generation is temporary.

### M06 - Payment Processing
#### Completed:
- Created `Payment` model
- Created `PaymentService`
- Added payment processing by order number
- Added payment method selection
- Validated amount paid
- Calculated change amount
- Stored payment records in memory
- Updated order payment status after payment
- Prevented duplicate payments

#### Known limitations:
- Payment data is stored in memory only.
- Payment gateway integration is not implemented.
- Receipt generation is not yet implemented.

### M07 - Receipt Generation
#### Completed:
- Created `Receipt` model
- Created `ReceiptService`
- Generated receipt records by order number
- Printed receipt details in the console
- Displayed purchased items, total amount, payment method, amount paid, and change
- Linked receipts to order and payment records
- Prevented receipt generation for unpaid orders
- Prevented duplicate receipt generation


#### Known limitations:
- Receipts are stored in memory only.
- Receipt file export is not yet implemented.

### M08 - Dashboard Summary
#### Completed:
- Created `DashboardService`
- Displayed total products
- Displayed active and inactive products
- Displayed total available stock
- Displayed total orders
- Displayed completed and pending orders
- Displayed total payments
- Displayed total income
- Displayed low-stock products


#### Known limitations:
- Dashboard is based on in-memory data only.
- No date filters or charts are available yet.

### M09 - JSON Persistence
#### Completed:
- Created `JsonStorageService`
- Added generic save and load methods
- Saved products, orders, payments, and receipts to JSON files
- Loaded products, orders, payments, and receipts from JSON files
- Added save and load menu options
- Added empty constructors to models for JSON deserialization


#### Known limitations:
- Storage is local JSON file storage only.
- Database storage is not yet implemented.
- IDs are still generated using list counts.

### M10 - v0.1.0 Release
#### Completed:
- Reviewed features from M00 to M09
- Tested inventory, basket, order, payment, receipt, dashboard, and JSON persistence flows
- Documented v0.1.0 release
- Added release notes
- Updated project documentation
- Created Git tag for v0.1.0

#### Release:
- Version: v0.1.0
- Name: Console Inventory and Sales MVP

#### Known limitations:
- Console app only
- JSON file storage only
- No database yet
- No authentication yet
- No automated tests yet
- No web API yet

## v0.2.0 - Inventory Rules and Reporting

Goal: Add inventory traceability, low-stock alerting, receipt export, sales reporting, notification simulation, and basic logging preparation.

Milestones:

### M11 - Stock Movement Tracking
#### Completed:
- Created `StockMovement` model
- Created `StockMovementService`
- Recorded stock-in movements
- Recorded stock adjustment movements
- Recorded stock-out movements from sales checkout
- Added stock movement history viewing
- Added JSON persistence for stock movement records

#### Known limitations:
- No filtering by date, product, or movement type yet.
- No user tracking for who performed the movement.
- No approval process for stock adjustments.

### M12 - Low Stock Alerts
#### Completed:
- Created `AlertService`
- Added low-stock alert menu option
- Displayed active products with low stock
- Used `QuantityInStock <= ReorderLevel` as the alert rule
- Excluded inactive products from alert results

#### Known limitations:
- Alerts are manually viewed from the menu.
- No automatic notification system yet.
- No supplier restocking workflow yet.

### M13 - Receipt File Export
#### Completed:
- Added receipt export method
- Added receipt content builder
- Created receipt files inside a `Receipts` folder
- Exported receipt details as readable `.txt` files
- Added receipt export menu option
- Added error handling for receipt export

#### Known limitations:
- Text export only.
- No PDF export yet.
- No receipt reprint history yet.

### M14 - Sales Summary Reports
#### Completed:
- Created `SalesReportService`
- Displayed order sales summary
- Displayed payment sales summary
- Calculated total sales income
- Calculated total cash received
- Calculated total change given
- Displayed sales by payment method
- Displayed completed order details

#### Known limitations:
- No date-based filtering yet.
- No product-level sales ranking yet.
- No exported sales report yet.

### M15 - Email Notification Simulation
#### Completed:
- Created `Notification` model
- Created `NotificationService`
- Simulated low-stock notification messages
- Simulated completed order notification messages
- Simulated receipt notification messages
- Added notification history viewing
- Added notification JSON persistence

#### Known limitations:
- Notifications are simulated only.
- No real email provider integration yet.
- Recipient addresses are placeholders.
- No retry or delivery failure handling yet.

### M16 - Error Handling and Logging Preparation
#### Completed:
- Created `LoggingService`
- Added INFO log support
- Added ERROR log support
- Added text file log output
- Automatically created `Logs` folder
- Updated `JsonStorageService` to log save and load events
- Logged JSON save and load errors
- Added application start and close logs

#### Known limitations:
- Basic file-based logging only.
- No log rotation yet.
- No structured JSON logs yet.
- No external logging library yet.

### M17 - v0.2.0 Release
#### Completed:
- Reviewed v0.2.0 features from M11 to M16
- Added product reactivation feature
- Tested stock movement tracking
- Tested low-stock alerts
- Tested receipt text file export
- Tested sales summary reports
- Tested notification simulation
- Tested basic logging
- Updated project documentation
- Updated release notes
- Created Git tag for v0.2.0

#### Release:

- Version: v0.2.0
- Name: Inventory Rules and Reporting

#### Known limitations:

- Console app only
- JSON file storage only
- No database yet
- No real email provider yet
- No automated tests yet
- No web API yet

## v0.2.0 - Inventory Rules and Reporting

Goal: Add inventory traceability, low-stock alerting, receipt export, sales reporting, notification simulation, and basic logging preparation.

Milestones:

### M18 - Database Requirements - Completed
#### Completed:
- Identified product data requirements
- Identified order data requirements
- Identified order item data requirements
- Identified payment data requirements
- Identified receipt data requirements
- Identified stock movement data requirements
- Identified notification data requirements
- Identified active/inactive product state requirements
- Identified historical transaction preservation requirements
- Identified future reporting requirements

#### Known limitations:
- No database tables designed yet
- No SQL scripts yet
- No database integration yet

### M19 - Database Table Design
#### Completed:
- Designed the Products table
- Designed the Orders table
- Designed the OrderItems table
- Designed the Payments table
- Designed the Receipts table
- Designed the StockMovements table
- Designed the Notifications table
- Identified primary keys
- Identified foreign keys
- Identified table relationships
- Documented the detailed design in `docs/database-design.md`

#### Known limitations:
- SQL scripts are not created yet
- SQLite database is not created yet
- Application still uses JSON storage
- Repository pattern is not implemented yet

### M20 - SQL CRUD Scripts 
#### Completed:
- Created `docs/sql/stockflow-crud-scripts.sql`
- Added create table scripts
- Added sample insert scripts
- Added select/read scripts
- Added update scripts
- Added deactivate/reactivate scripts
- Added delete examples
- Added reporting query examples

#### Known limitations:
- Scripts are not integrated with the C# application yet.
- SQLite database is not created yet.
- Repository pattern is not implemented yet.

### M21 - SQLite Integration
### M22 - Repository Pattern Introduction
### M23 - v0.3.0 Release 