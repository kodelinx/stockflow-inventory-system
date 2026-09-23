# StockFlow Database Design

**Status:** Active technical reference  
**Last reviewed:** 2026-09-24  
**Database:** SQLite  
**Owner:** StockFlow development project

## 1. Purpose and Scope

This document defines StockFlow's **persistent data model**, its relationships, data conventions, and database lifecycle. It describes the schema the application is intended to maintain; it is not a milestone journal or a record of every repository change.

- **Requirements:** [`requirements.md`](requirements.md) defines *what* the system must do.
- **Business rules:** [`business-rules.md`](business-rules.md) defines the rules applied to transactions.
- **Architecture:** [`architecture.md`](architecture.md) describes where database responsibilities belong.
- **Milestones:** [`milestone-plan.md`](milestone-plan.md) tracks progress.
- **Release notes:** [`release-notes.md`](release-notes.md) records changes over time.

Update the existing entity, relationship, or operations section when the design changes. Do not add a new section for every milestone.

## 2. Database Overview

### 2.1 Storage architecture

SQLite is the primary persistent store for the Console application's business records. The repository-first service refactor is still being completed. `BasketItem` is intentionally kept in memory for the current Console session; it becomes persisted `Order` and `OrderItem` data at checkout.

```text
StockFlow.Console / StockFlow.Api
             |
     Application workflows
             |
  Infrastructure repositories
             |
  DatabaseConnectionService
             |
       SQLite database
```

`StockFlow.Core` defines the shared entity models. `StockFlow.Infrastructure` contains `DatabaseConnectionService` and repository implementations. The API's integration status varies by endpoint; see `api-design.md` rather than assuming every API endpoint uses SQLite.

### 2.2 Schema inventory

| Table | Responsibility | Repository |
| --- | --- | --- |
| `Products` | Product catalog and current quantity | `ProductRepository` |
| `Orders` | Checkout summary and order status | `OrderRepository` |
| `OrderItems` | Purchased-product snapshots for each order | `OrderItemRepository` |
| `Payments` | Payment transactions and amounts | `PaymentRepository` |
| `Receipts` | Generated proof of payment | `ReceiptRepository` |
| `StockMovements` | Inventory movement history | `StockMovementRepository` |
| `Notifications` | Simulated alerts and notification history | `NotificationRepository` |

Possible future entities include users, roles, customers, suppliers, categories, and audit logs. They are **not part of this current schema**.

## 3. Logical Data Model

The lists below document the currently described model and table columns. The SQL in `DatabaseConnectionService.InitializeDatabase()` remains the definitive reference for exact column constraints in a specific checkout of the code.

### 3.1 Products

**Purpose:** Maintain the product catalog, availability, pricing, and current stock quantity.

| Column | Purpose |
| --- | --- |
| `ProductId` | SQLite-generated primary key |
| `ProductCode` | Unique business-facing product identifier |
| `Name` | Product name |
| `Category` | Product classification |
| `UnitPrice` | Current selling price |
| `QuantityInStock` | Current quantity available |
| `ReorderLevel` | Low-stock threshold |
| `IsActive` | Active/inactive flag (`1`/`0`) |

**Design rules:** Products at or below `ReorderLevel` are low-stock when active. Normal removal should use deactivation so historical transactions remain meaningful. Hard deletion requires special care when related order items or movements exist.

### 3.2 Orders

**Purpose:** Store one checkout's summary and its order/payment lifecycle.

| Column | Purpose |
| --- | --- |
| `OrderId` | SQLite-generated primary key |
| `OrderNumber` | Unique business-facing order reference |
| `OrderDate` | Checkout timestamp |
| `TotalAmount` | Recorded order total |
| `OrderStatus` | Order lifecycle status |
| `PaymentStatus` | Payment lifecycle status |

**Design rules:** A newly checked-out order is initially `Pending Payment` / `Unpaid` in the current Console flow. After successful payment, the application updates its statuses to `Completed` / `Paid`. The parent `OrderId` is needed when saving its items.

### 3.3 OrderItems

**Purpose:** Preserve the purchased items and prices at the time of checkout.

| Column | Purpose |
| --- | --- |
| `OrderItemId` | SQLite-generated primary key |
| `OrderId` | Reference to the parent order |
| `ProductId` | Reference to the product |
| `ProductCode` | Product code snapshot |
| `ProductName` | Product name snapshot |
| `Quantity` | Units purchased |
| `UnitPrice` | Price at checkout |
| `LineTotal` | `Quantity × UnitPrice` at checkout |

An `OrderItem` model should contain **both** `OrderItemId` and `OrderId`. At checkout, the service saves the order first, retrieves its generated `OrderId`, and inserts each item using that ID. The repository should load both IDs when reconstructing an item.

The item snapshots must not be silently changed when the catalog product is later renamed or repriced. Clearing the temporary basket does not remove persisted order items.

### 3.4 Payments

**Purpose:** Record money collected for orders.

| Column | Purpose |
| --- | --- |
| `PaymentId` | SQLite-generated primary key |
| `PaymentNumber` | Unique business-facing payment reference |
| `OrderId` | Reference to the paid order |
| `OrderNumber` | Readable order reference |
| `PaymentDate` | Payment timestamp |
| `PaymentMethod` | Selected payment channel |
| `AmountDue` | Amount owed for the transaction |
| `AmountPaid` | Amount tendered |
| `ChangeAmount` | Amount returned, if any |
| `PaymentStatus` | Payment lifecycle status |

**Reporting rule:** `AmountDue` is the sale amount for a fully paid transaction. `AmountPaid` includes any overpayment subsequently returned as change, so it should not be treated as sales income without adjustment. The current Console flow processes full payments; multiple or partial payments are possible future requirements, not verified current behavior.

### 3.5 Receipts

**Purpose:** Record proof of payment and support display, reprinting, and text export.

| Column | Purpose |
| --- | --- |
| `ReceiptId` | SQLite-generated primary key |
| `ReceiptNumber` | Unique business-facing receipt reference |
| `OrderId` | Reference to the related order |
| `PaymentId` | Reference to the related payment |
| `OrderNumber` | Readable order reference |
| `PaymentNumber` | Readable payment reference |
| `ReceiptDate` | Receipt creation timestamp |
| `TotalAmount` | Transaction total snapshot |
| `PaymentMethod` | Payment method snapshot |
| `AmountPaid` | Tendered amount snapshot |
| `ChangeAmount` | Change snapshot |

**Current intended rule:** At most one receipt per payment. The service should check for an existing receipt before inserting. Whether the database also enforces `UNIQUE(PaymentId)` must be checked against the actual initialization SQL; application validation alone does not eliminate concurrency races. Receipt item details are currently obtained by loading the related order's `OrderItems`, not from a separate `ReceiptItems` table.

### 3.6 StockMovements

**Purpose:** Explain how and why available stock changed.

| Column | Purpose |
| --- | --- |
| `StockMovementId` | SQLite-generated primary key |
| `ProductId` | Reference to the product |
| `ProductCode` | Readable product reference |
| `ProductName` | Product name at recording time |
| `MovementType` | Such as `Stock In`, `Stock Out`, or `Adjustment` |
| `QuantityChanged` | Signed inventory difference |
| `StockBefore` | Quantity before the change |
| `StockAfter` | Quantity after the change |
| `Reason` | Explanation for the movement |
| `MovementDate` | Event timestamp |
| `ReferenceNumber` | Related order or other business reference, if applicable |

A stock increase has a positive `QuantityChanged`; a sale stock-out has a negative value. Adjustments may be positive, negative, or zero. For a valid movement, `StockAfter = StockBefore + QuantityChanged`.

### 3.7 Notifications

**Purpose:** Preserve simulated email/alert history and support future real notification delivery.

| Column | Purpose |
| --- | --- |
| `NotificationId` | SQLite-generated primary key |
| `NotificationType` | Type of business event |
| `Recipient` | Intended recipient |
| `Subject` | Message subject |
| `Title` | Display title |
| `Message` | Notification body |
| `RelatedReference` | Related business reference, if any |
| `IsRead` | Read state (`1`/`0`) |
| `CreatedAt` | Creation timestamp |
| `Status` | Current notification status, e.g. `Simulated` |

Notifications are currently simulated; a saved row does **not** mean an email was actually sent.

## 4. Relationships and Integrity

### 4.1 Entity relationships

```text
Products (1) ───────────< OrderItems >─────────── (1) Orders
    |                                                |
    └─────────< StockMovements                      └──< Payments
                                                         |
                                                         └──< Receipts

Notifications: related business events are currently identified with
RelatedReference rather than a universal database foreign key.
```

| Parent | Child | Relationship / intended rule |
| --- | --- | --- |
| `Orders.OrderId` | `OrderItems.OrderId` | One order has many items |
| `Products.ProductId` | `OrderItems.ProductId` | One product can appear in many items |
| `Orders.OrderId` | `Payments.OrderId` | Current flow allows one completed payment per order; future partial payments could change this |
| `Orders.OrderId` | `Receipts.OrderId` | A receipt refers to its order |
| `Payments.PaymentId` | `Receipts.PaymentId` | Current intended rule: at most one receipt per payment |
| `Products.ProductId` | `StockMovements.ProductId` | One product can have many stock movements |

**Enforcement note:** Confirm actual `FOREIGN KEY`, `UNIQUE`, and delete constraints in `InitializeDatabase()` before claiming they are guaranteed by SQLite. SQLite foreign-key enforcement also depends on connection configuration such as `PRAGMA foreign_keys = ON`.

### 4.2 Internal IDs and business references

Internal IDs (`ProductId`, `OrderId`, `OrderItemId`, `PaymentId`, `ReceiptId`, `StockMovementId`, `NotificationId`) identify database rows. Business references (`ProductCode`, `OrderNumber`, `PaymentNumber`, `ReceiptNumber`) are intended for users, display, and lookup.

SQLite creates internal row IDs. The application currently generates business references; generating them from the highest stored ID is adequate for a single-user learning workflow but is not a complete concurrency-safe numbering strategy.

### 4.3 Historical consistency

Order item names and prices, payment amounts, and receipt transaction details are snapshots of completed activity. Changes to the current product catalog should not rewrite past transactions. Product deactivation is preferable to hard deletion where transactional history references the product.

## 5. Data Types and Mapping Conventions

| Data | Current convention | Important qualification |
| --- | --- | --- |
| IDs and quantities | SQLite `INTEGER` ↔ C# `int` | Exact PK definition comes from initialization SQL |
| Names, codes, statuses, messages | SQLite `TEXT` ↔ C# `string` | Required/nullable constraints vary by column |
| Money | SQL declarations such as `DECIMAL(10,2)` ↔ C# `decimal` | SQLite does not enforce decimal precision or fixed scale from the declaration alone; verify monetary round-tripping |
| Timestamps | Timestamp text ↔ C# `DateTime` | Current repositories serialize/parse dates; use a consistent format |
| Booleans | SQLite `INTEGER` (`0`/`1`) ↔ C# `bool` | Reader mapping typically uses `GetInt32(...) == 1` |
| Optional references | Nullable SQL field ↔ nullable C# property where appropriate | Check `IsDBNull` before reading null values |

**Reader-mapping rule:** Explicit `SELECT` column order must match the positional indexes in each `MapReaderTo...()` helper. A misplaced index can cause conversion errors (for example, parsing an order number as a date). Maintain consistent SELECT lists for all methods sharing one mapper.

## 6. Persistence and Transaction Flows

### 6.1 Initialization and repository access

`DatabaseConnectionService` supplies the SQLite connection string and creates required tables using `CREATE TABLE IF NOT EXISTS`. Repositories use `Microsoft.Data.Sqlite` commands with SQL parameters for supplied values. Run initialization before executing repository operations.

`CREATE TABLE IF NOT EXISTS` **does not migrate an existing table**. Changes to columns or constraints require an explicit migration or a disposable development database reset.

### 6.2 Checkout

```text
Validate basket against current Products
  → create Order and insert into Orders
  → obtain generated OrderId
  → insert related OrderItems
  → update affected Product quantities
  → insert Stock Out movements using OrderNumber
  → clear temporary basket after successful completion
```

**Current reliability gap:** The previously reviewed Console implementation calls multiple repository operations using separate connections. Until a shared transaction coordinates the inserts and stock updates, a failure midway could leave a partially completed checkout. Implement and test atomic checkout before treating the database workflow as production-safe.

### 6.3 Payment and receipt

A successful payment saves a `Payment` row linked to an existing order and updates order/payment status. Receipt generation locates the saved payment, checks for an existing receipt, loads the related order and its items for display, and saves the receipt linked to the payment and order.

The current process should be regression-tested for consistent `Completed` / `Paid` statuses, prevention of duplicate payments and receipts, and recovery from partial failures. Treat multi-write payment updates as a candidate for a shared transaction.

### 6.4 Stock adjustments and notification history

Stock-in and adjustment flows update `Products.QuantityInStock` and insert a matching `StockMovements` row. Notification simulation queries the relevant persisted records and stores simulated messages through `NotificationRepository`.

For reliability, the product quantity change and corresponding movement should eventually be saved within the same database transaction.

## 7. Database Lifecycle and Environments

### 7.1 Location and configuration

The current development configuration has used a relative path such as `Database/stockflow.db`. Its physical location depends on the process's working directory unless normalized centrally. `GetDatabaseFilePath()` can return `Path.GetFullPath(_databaseFilePath)` for troubleshooting; ideally the service consistently uses one normalized path for **both** connection creation and reset.

Keep generated `.db`, `.db-shm`, and `.db-wal` files out of Git. Do not store production data or secrets in the repository.

### 7.2 Development reset and seed data

A **development-only** `ResetDatabase()` may delete the configured database file and call `InitializeDatabase()` to recreate all tables. It must be protected by explicit confirmation in the Console and must never point at production data. Clear any remaining in-memory basket state after reset. If product seeding is configured, document whether reset recreates sample products or leaves an empty catalog.

A file-level reset should only run when application connections to that file are closed. Handle SQLite sidecar files appropriately and use disposable test databases for automated tests.

### 7.3 Schema changes and migrations

For disposable development data, recreate the database when a schema changes. For data that must be retained, write a deliberate migration (for example, `ALTER TABLE`) and test it against a backup. Track future migration/versioning conventions here when implemented; do not describe a proposed migration system as active.

### 7.4 Test isolation

Automated integration tests should each use an isolated temporary SQLite database (or another clearly isolated supported setup) rather than the normal development `stockflow.db`. Important checks include initialization, CRUD, relationships, receipt uniqueness, stock consistency, reset behavior, and a complete checkout/payment/receipt workflow.

## 8. Security, Reliability, and Operational Constraints

- Use parameterized SQL for externally supplied values; do not interpolate user input into SQL commands.
- Restrict destructive operations such as product hard deletion and database reset; application-level roles are planned, not yet enforced.
- Favor explicit database transactions for workflows that must succeed or fail as one unit.
- Verify foreign-key enforcement and uniqueness constraints on actual connections.
- Use consistent status values and timestamps; validate money/quantity input before persistence.
- Avoid duplicate business references and test behavior after deleting/deactivating records.
- Do not infer successful notification delivery from `Status = Simulated`.

## 9. Open Design Items

This table is an ongoing list of architecture-level database gaps, **not** a milestone history. Update rows in place as the implementation changes.

| Item | Current position | Verification / next decision |
| --- | --- | --- |
| Cross-repository transactions | Not confirmed in reviewed Console flow | Make checkout and multi-write stock/payment operations atomic |
| Foreign-key enforcement | Table relationships described; connection-level enforcement not verified | Check SQL definitions and connection PRAGMA |
| One receipt per payment | Intended business rule; service protection being added | Verify repository lookup and consider database UNIQUE constraint |
| Business number generation | Derived from stored records in current learning implementation | Review collision handling for multi-user use |
| Monetary storage | C# decimal with SQLite decimal-style declarations | Verify round-tripping; choose explicit storage policy before production |
| Schema migrations | Manual/development reset approach | Define migrations when retaining user data matters |
| Automated database tests | Planned | Add isolated integration tests and workflow regression coverage |
| API parity | Product API is repository-backed; other endpoint integration needs code verification | Update `api-design.md` as endpoints are connected |

## 10. Document Maintenance

Treat sections **2–9 as stable headings**. Update the existing table or subsection when an entity, constraint, mapping rule, or data flow changes; create a new entity subsection only when the actual schema gains a new entity.

- Update **Section 3** when entity fields change.
- Update **Section 4** when relationships or constraints change.
- Update **Sections 5–7** when mapping, persistence, configuration, or migration conventions change.
- Update **Sections 8–9** when reliability requirements or design decisions change.
- Change **Last reviewed** only after checking the document against the relevant source files.
- Record milestone completion in `milestone-plan.md` and historical changes in `release-notes.md`; do not append milestone-by-milestone updates here.

**Source-of-truth reminder:** This document describes the maintained design. The current initialization SQL and repository implementations must be checked when exact database behavior matters.
