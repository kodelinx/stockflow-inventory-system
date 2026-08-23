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