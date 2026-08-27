# StockFlow Requirements

## Functional Requirements

### Inventory Management

- The system should allow users to add products. (v0.1.0: M02 Implemented - Console Version)
- The system should allow users to view products. (v0.1.0: M01 Implemented - Console Version)
- The system should allow users to search products. (v0.1.0: MO2 Implemented - Console Version)
- The system should allow users to update product details. (v0.1.0: MO2 Implemented - Console Version)
- The system should allow users to deactivate products. (v0.1.0: MO2 Implemented - Console Version)
- The system should allow users to delete products. (v0.1.0: MO2 Implemented - Console Version)
- The system should track product stock quantity. (v0.1.0: MO2 Implemented - Console Version)
- The system should identify low-stock products. (v0.1.0: M12 Implemented - Console Version)

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
- The system should print out receipts as text file (v0.1.0: M13 Implemented - Console Version)

### Dashboard

- The system should show total products. (v0.1.0: MO8 Implemented - Console Version)
- The system should show available stock. (v0.1.0: MO8 Implemented - Console Version)
- The system should show low-stock items. (v0.1.0: MO8 Implemented - Console Version)
- The system should show total sales. (v0.1.0: MO8 Implemented - Console Version)
- The system should show total income. (v0.1.0: MO8 Implemented - Console Version)

### Notifications

- The system should notify the owner when a new order is created.
- The first version may simulate email notification before real email integration. (v0.1.0: M15 Implemented - Console Version - Simulation Only)
- The system creates simulated low-stock notification records. (v0.1.0: M15 Implemented - Console Version - Simulation Only)
- The system creates simulated completed order notification records.  (v0.1.0: M15 Implemented - Console Version - Simulation Only)
- The system creates simulated receipt notification records. (v0.1.0: M15 Implemented - Console Version - Simulation Only)
- The system stores notification type, recipient, subject, message, creation date, and status. (v0.1.0: M15 Implemented - Console Version - Simulation Only)
- The system allows users to view notification history. (v0.1.0: M15 Implemented - Console Version)
- The system saves and loads notification records through JSON persistence. (v0.1.0: M15 Implemented - Console Version)

### Reports

- The system should record and view the increase, adjustments, and reductions of stock quantity of a product.(v0.1.0: M11 Implemented - Console Version)
- The system should present orders and payment summaries for sales report (v0.1.0: M14 Implemented - Console Version)

## Non-Functional Requirements

- The system should validate user input. (v0.1.0: MO3 Implemented - Console Version)
- The system should use clear code structure.
- The system should separate models, services, data, and utilities. (v0.1.0: MO9 Implemented - Console Version)
- The system should save data so it is not lost after closing the app. (v0.1.0: MO9 Implemented - Console Version)
- The system should be documented through README and project docs.
- The system should be version-controlled using Git and GitHub.


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