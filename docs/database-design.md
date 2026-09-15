# StockFlow Database Design

Last updated: 2026-09-05

## Version

v0.3.0 - Database-Ready Inventory System

## Purpose

This document describes the planned database structure for StockFlow.

The goal is to prepare the system for database-backed storage while preserving important business records such as products, orders, payments, receipts, stock movements, and notifications.

---

# Current Storage

StockFlow currently still uses JSON file storage for the main console app flow.

```text
StockFlow.Console
    ↓
JsonStorageService
    ↓
Local JSON files
```

# Planned Storage

StockFlow will move toward repository-based database storage.

```text
Application
    ↓
Services
    ↓
Repositories
    ↓
SQLite Database
```

---

# Planned Tables

Main planned tables:

- Products
- Orders
- OrderItems
- Payments
- Receipts
- StockMovements
- Notifications

Future possible tables:

- Users
- Roles
- Customers
- Suppliers
- Categories
- AuditLogs

---

# Table Design Summary

## Products

Purpose:

Stores the product catalog and current inventory quantity.

Main columns:

- ProductId - Primary key
- ProductCode - Unique product code
- Name
- Category
- UnitPrice
- QuantityInStock
- ReorderLevel
- IsActive

Design notes:

- Products should be deactivated instead of hard deleted when transaction history exists.
- `IsActive = 1` means active.
- `IsActive = 0` means inactive.
- ProductCode should be treated as the business-facing product identifier.

Current implementation status:

- Products table is initialized from C#.
- ProductRepository is implemented in StockFlow.Infrastructure.
- ProductRepository supports product create, read, update, deactivate, and delete operations.
- Product API currently uses ProductRepository for read operations.
- Full Console product flow is not yet fully SQLite-backed.

## Orders

Purpose:

Stores checkout transactions.

Main columns:

- OrderId - Primary key
- OrderNumber - Unique order number
- OrderDate
- TotalAmount
- OrderStatus
- PaymentStatus

Design notes:

- Orders are created during checkout.
- An order should only be completed after payment is processed.
- OrderNumber is the user-facing business reference.

Current implementation status:

- Orders table is initialized from C#.
- OrderRepository has been added to StockFlow.Infrastructure.
- OrderRepository supports adding orders, reading all orders, finding orders by order number, and updating order/payment status.
- Order items are planned for M41.

## OrderItems

Purpose:

Stores the individual products inside each order.

Main columns:

- OrderItemId - Primary key
- OrderId - Foreign key to Orders
- ProductId - Foreign key to Products
- ProductCode
- ProductName
- Quantity
- UnitPrice
- LineTotal

Design notes:

- One order can have many order items.
- Order items store product snapshot data.
- Product name and unit price are saved here so old receipts remain accurate even if product details change later.

Current implementation status:

- OrderItems table is initialized from C#.
- OrderItemRepository has been added to StockFlow.Infrastructure.
- OrderItemRepository supports adding order items and retrieving order items by OrderId.
- Full checkout integration is planned for a later milestone.

## Payments

Purpose:

Stores payment records for orders.

Main columns:

- PaymentId - Primary key
- PaymentNumber - Unique payment number
- OrderId - Foreign key to Orders
- OrderNumber
- PaymentDate
- PaymentMethod
- AmountDue
- AmountPaid
- ChangeAmount
- PaymentStatus

Design notes:

- AmountDue is the actual sales income.
- AmountPaid is the money received from the customer.
- ChangeAmount is the money returned to the customer.
- PaymentNumber is the user-facing payment reference.

Current implementation status:

- Payments table is initialized from C#.
- PaymentRepository has been added to StockFlow.Infrastructure.
- PaymentRepository supports adding payments, reading all payments, finding one payment by payment number, and getting payments by order number.
- PaymentNumber is used to identify one specific payment transaction.
- OrderNumber is used to group payments under one order.
- The design can support future partial payments, split payments, and payment history tracking.

## Receipts

Purpose:

Stores receipt records generated from paid orders.

Main columns:

- ReceiptId - Primary key
- ReceiptNumber - Unique receipt number
- OrderId - Foreign key to Orders
- PaymentId - Foreign key to Payments
- OrderNumber
- PaymentNumber
- ReceiptDate
- TotalAmount
- PaymentMethod
- AmountPaid
- ChangeAmount

Design notes:

- A receipt should only be generated for a paid order.
- Receipts are used for transaction proof, viewing, and future export or reprinting.
- ReceiptNumber is the user-facing receipt reference.

Current implementation status:

- Receipts table is initialized from C#.
- ReceiptRepository has been added to StockFlow.Infrastructure.
- ReceiptRepository supports adding receipts, reading all receipts, finding one receipt by receipt number, and getting receipts by order number.
- ReceiptNumber is used to identify one specific issued receipt.
- OrderNumber is used to retrieve receipts connected to one order.
- Receipts are linked to Orders and Payments through OrderId and PaymentId.

## StockMovements

Purpose:

Stores inventory quantity changes.

Main columns:

- StockMovementId - Primary key
- ProductId - Foreign key to Products
- ProductCode
- ProductName
- MovementType
- QuantityChanged
- StockBefore
- StockAfter
- Reason
- MovementDate

Design notes:

- Stock movements create an inventory audit trail.
- QuantityChanged can be positive, negative, or zero.
- Stock In is usually positive.
- Stock Out is usually negative.
- Adjustment can be positive, negative, or zero.

Current implementation status:

- StockMovements table is initialized from C#.
- StockMovementRepository has been added to StockFlow.Infrastructure.
- StockMovementRepository supports adding stock movements, reading all stock movements, retrieving movements by product code, and retrieving movements by reference number.
- QuantityChanged stores positive values for stock increases and negative values for stock decreases.
- ReferenceNumber can be used to connect movement history to an order, receipt, or adjustment.

## Notifications

Purpose:

Stores notification records generated by the app.

Main columns:

- NotificationId - Primary key
- NotificationType
- Recipient
- Subject
- Message
- CreatedAt
- Status

Design notes:

- Notifications are simulated for now.
- This table prepares the system for future real email sending.
- Possible future statuses include Pending, Sent, Failed, and Simulated.

Current implementation status:

- Notifications table is initialized from C#.
- NotificationRepository has been added to StockFlow.Infrastructure.
- NotificationRepository supports adding notifications, reading all notifications, reading unread notifications, and marking notifications as read.
- IsRead uses 0 or 1 in SQLite and is converted to true or false in C#.
- RelatedReference is optional and can connect a notification to a product, order, payment, receipt, or stock movement.

---

# Relationship Summary

- One product can appear in many order items.
- One order can contain many order items.
- One order can have one payment.
- One payment can have one receipt.
- One product can have many stock movements.
- Notifications store system-generated message history.

```text
Products
   ↓
OrderItems
   ↑
Orders
   ↓
Payments
   ↓
Receipts

Products
   ↓
StockMovements

Notifications
```

---

# Data Type Notes

Planned database type guide:

- INTEGER - IDs, quantities, boolean values in SQLite
- TEXT - names, codes, statuses, messages
- DECIMAL(10,2) - money values
- DATETIME - dates and timestamps

SQLite boolean note:

```text
1 = true
0 = false
```

Example:

```text
IsActive = 1 means active
IsActive = 0 means inactive
```

---

# Database Design Rules

- Product records should be deactivated instead of hard deleted when transaction history exists.
- Order items should preserve historical product details.
- Sales reports should use AmountDue as income.
- Stock movements should explain why inventory quantity changed.
- Receipts should be generated only for paid orders.
- Notifications are simulated for now but prepare the app for future real email integration.
- Future database access should be separated using repositories.
- Generated database files should not be committed to Git.

# Current Limitations

- Only Products table is currently initialized from C#.
- ProductRepository has been started.
- Other repositories are not yet implemented.
- Full app flow is not yet database-backed.
- JSON persistence still exists.

## Current Database Implementation Status

- SQLite integration has started.
- DatabaseConnectionService has been moved to StockFlow.Infrastructure.
- ProductRepository has been moved to StockFlow.Infrastructure.
- ProductRepository currently supports product data access.
- Product API endpoints use ProductRepository and SQLite.
- Other repositories are not yet implemented.
- Full database-backed application flow is planned for a later version.

### Database initialization

Current implementation status:
- DatabaseConnectionService creates the SQLite database folder.
- DatabaseConnectionService opens a SQLite connection.
- DatabaseConnectionService creates required tables using CREATE TABLE IF NOT EXISTS.
- ExecuteNonQuery is used to run CREATE TABLE commands.
- Database initialization currently creates tables for Products, Orders, OrderItems, Payments, Receipts, StockMovements, and Notifications.

### Product read flow status

Current implementation status:

- Products table is initialized by DatabaseConnectionService.
- ProductRepository reads active product records from SQLite.
- The Console app now begins using ProductRepository.GetActiveProducts() for product display.
- Product data is loaded into List<Product> as a temporary in-memory working copy.
- Product add/update/deactivate flow is planned for the next SQLite integration step.

### Product add flow status

Current implementation status:

- ProductRepository supports inserting new product records into SQLite.
- InventoryService now uses ProductRepository.AddProduct() when adding products.
- Newly added products are saved in the Products table.
- The Console product list is refreshed from SQLite after adding a product.

### Product management flow status

Current implementation status:

- Products can be updated through ProductRepository.UpdateProduct().
- Products can be deactivated using ProductRepository.DeactivateProduct().
- Products can be reactivated using ProductRepository.ReactivateProduct().
- Products can be hard deleted using ProductRepository.DeleteProduct().
- IsActive uses 1 or 0 in SQLite and is converted to true or false in C#.
- GetActiveProducts() returns only active products.
- GetAllProducts() returns both active and inactive products.
