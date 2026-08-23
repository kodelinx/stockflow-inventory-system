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