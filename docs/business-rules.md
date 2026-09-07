# StockFlow Business Rules

Last updated: 2026-09-05

## Purpose

This document defines rules that StockFlow must follow to keep business data correct and reliable.

Requirements describe what the system should do. Business rules describe the conditions that must always be respected.

---

# Product Rules

- Product code should be unique.
- Product name should not be empty.
- Product category should not be empty when required by the product flow.
- Unit price should not be negative.
- Quantity in stock should not be negative.
- Reorder level should not be negative.
- Active products are available for normal inventory and sales operations.
- Inactive products should not be available for normal checkout.
- Products with transaction history should be deactivated instead of hard deleted.
- Reactivated products should become available again for normal inventory operations.

# Basket Rules

- A basket can contain one or more products.
- A product can only be added to the basket if it is active.
- A product can only be added to the basket if enough stock is available.
- Basket item quantity should be greater than zero.
- Basket total should be calculated from item quantity and unit price.
- Removing an item from the basket should not affect product stock.
- Clearing the basket should not affect product stock.

# Order Rules

- Checkout should not proceed if the basket is empty.
- Checkout should not proceed if stock is insufficient.
- Stock should be reduced only after checkout validation succeeds.
- Each order should have a unique order number.
- An order should contain one or more order items.
- Order items should preserve product snapshot data such as product code, product name, unit price, quantity, and line total.
- Order status should reflect the business state of the order.
- An order should not be considered completed until payment is processed.

# Payment Rules

- Payment amount should be greater than or equal to amount due.
- Change amount should be calculated when amount paid is greater than amount due.
- Payment method should be recorded.
- Duplicate payments for the same order should be prevented or controlled.
- Payment status should be updated after a successful payment.
- Payment records should preserve amount due, amount paid, change, payment date, payment method, and payment number.

# Receipt Rules

- Receipt should only be generated for paid orders.
- Duplicate receipts for the same order should be prevented.
- Receipt number should be unique.
- Receipt should include order, payment, and item details.
- Receipt export should not crash the system if file writing fails.
- Receipt records should be kept for future viewing or reprinting.

# Stock Movement Rules

- Stock In usually increases quantity.
- Stock Out usually decreases quantity.
- Stock Adjustment may increase, decrease, or keep quantity unchanged.
- QuantityChanged can be positive, negative, or zero depending on movement type.
- Stock movement records should include stock before and stock after values.
- Stock movement records should include product details.
- Stock movement records should include a reason.
- Stock movement records should include movement date.
- Stock movements should support inventory audit trail.

# Low-Stock Alert Rules

- A product is low stock when `QuantityInStock <= ReorderLevel`.
- Inactive products should be excluded from low-stock alerts.
- Low-stock alerts should help the business identify products that may need restocking.

# Notification Rules

- Notification records should include type, recipient, subject, message, creation date, and status.
- Simulated notifications should be clearly marked as simulated.
- Future real notification failures should not crash the system.
- Notification history should be viewable for tracking system-generated messages.

# API Rules

- API endpoints should return JSON.
- API endpoints should use proper HTTP status codes.
- Successful read requests should return 200 OK.
- Missing records should return 404 Not Found.
- Invalid input should return 400 Bad Request where applicable.
- API responses should avoid exposing unnecessary internal error details.
- API route names should be predictable and consistent.
- Controllers should not contain direct SQL logic.
- Controllers should eventually call services and repositories.

# Security Rules

- Sensitive actions should eventually require authentication.
- Product deletion should eventually require an authorized role.
- Payment actions should eventually require an authorized role.
- User roles should control what a person can do in the system.
- Secrets, passwords, connection strings, and tokens should not be committed to GitHub.
