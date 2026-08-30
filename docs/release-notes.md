## v0.1.0 - Console Inventory and Sales MVP

Release Date: 2026-08-23

### Summary

StockFlow v0.1.0 is the first working console-based MVP of the inventory and sales management system.

This release includes inventory management, basket management, checkout, payment processing, receipt generation, dashboard summaries, and JSON file persistence.

### Completed Features

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

### Technical Improvements

- Separated models, services, utilities, and data storage
- Used service classes for business operations
- Used JSON serialization and deserialization
- Added reusable generic storage methods
- Added validation for common invalid inputs
- Added basic error handling for file operations

### Known Limitations

- Console application only
- Data is stored in local JSON files
- No database yet
- No user login or role-based access yet
- No automated tests yet
- No web API yet
- No receipt export to PDF/text yet
- ID generation is still based on list counts
- No date-based reports yet

### Next Version

v0.2.0 will focus on inventory rules, reporting improvements, stock movement tracking, receipt file export, and stronger error handling.

## v0.2.0 - Inventory Rules and Reporting

Release Date: 2026-08-29

### Summary

StockFlow v0.2.0 improves the console MVP by adding inventory traceability, low-stock alerting, receipt export, sales reporting, notification simulation, and basic logging preparation.

### Completed Features

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

### Technical Improvements

- Added `StockMovement` model
- Added `Notification` model
- Added `StockMovementService`
- Added `AlertService`
- Added `SalesReportService`
- Added `NotificationService`
- Added `LoggingService`
- Improved separation of concerns
- Added basic application log file output
- Improved troubleshooting support for JSON storage

### Known Limitations

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

### Next Version

v0.3.0 will focus on preparing StockFlow for database-backed storage, SQL design, CRUD scripts, SQLite integration, and repository pattern introduction.