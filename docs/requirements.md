# StockFlow Requirements

## Functional Requirements

### Inventory Management

- The system should allow users to add products. (v0.1.0: M02 Implemented - Console Version)
- The system should allow users to view products. (v0.1.0: M01 Implemented - Console Version)
- The system should allow users to search products. (v0.1.0: MO2 Implemented - Console Version)
- The system should allow users to update product details. (v0.1.0: MO2 Implemented - Console Version)
- The system should allow users to deactivate products. (v0.1.0: MO2 Implemented - Console Version)
- The system should allow users to delete products. (v0.1.0: MO2 Implemented - Console Version)
- The system should track product stock quantity.
- The system should identify low-stock products.

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

- The system should generate receipts after checkout.
- The receipt should display ordered items, quantities, prices, total, payment method, amount paid, and change.

### Dashboard

- The system should show total products.
- The system should show available stock.
- The system should show low-stock items.
- The system should show total sales.
- The system should show total income.

### Notifications

- The system should notify the owner when a new order is created.
- The first version may simulate email notification before real email integration.

## Non-Functional Requirements

- The system should validate user input. (v0.1.0: MO3 Implemented - Console Version)
- The system should use clear code structure.
- The system should separate models, services, data, and utilities.
- The system should save data so it is not lost after closing the app.
- The system should be documented through README and project docs.
- The system should be version-controlled using Git and GitHub.