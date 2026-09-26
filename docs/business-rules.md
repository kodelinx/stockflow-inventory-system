# StockFlow Business Rules

**Document status:** Maintained specification  
**Last reviewed:** 2026-09-24  
**Scope:** StockFlow inventory and sales operations

## 1. Purpose and Scope

This document defines the business constraints and decisions that StockFlow must enforce consistently across its Console application, API, and persistence layer. It describes **what must be true**, not the implementation history or the exact code that enforces it.

Related documents:

- [`requirements.md`](requirements.md) — features and system requirements.
- [`architecture.md`](architecture.md) — component responsibilities and data flow.
- [`database-design.md`](database-design.md) — tables, columns, keys, and persistence design.
- [`api-design.md`](api-design.md) — endpoints and HTTP response conventions.
- [`acceptance-criteria.md`](acceptance-criteria.md) — observable evidence that rules work.
- [`milestone-plan.md`](milestone-plan.md) — delivery progress.
- [`release-notes.md`](release-notes.md) — changes already delivered in each version.

The rules below are **intended system behavior**. Listing a rule here does not establish that its enforcement has been tested. Unverified and future enforcement work is tracked in Section 7.

## 2. Rule Conventions

### 2.1 Stable identifiers

Rule identifiers remain stable when implementation changes. Use the existing domain prefix and the next available number for a genuinely new rule. Edit an existing rule in place when its meaning is refined; do not add milestone-specific copies.

| Prefix | Domain |
| --- | --- |
| `BR-PRD` | Products |
| `BR-BSK` | Basket |
| `BR-ORD` | Orders and checkout |
| `BR-PAY` | Payments |
| `BR-RCT` | Receipts |
| `BR-STK` | Stock movements and alerts |
| `BR-NTF` | Notifications |
| `BR-DAT` | Data integrity and lifecycle |
| `BR-SEC` | Access and security |

**Language:** “Must” is mandatory; “should” is a preferred behavior that requires a documented exception if omitted. Details of HTTP transport belong primarily in `api-design.md`.

### 2.2 Business state conventions

- **Active product:** available for ordinary inventory and sales operations.
- **Inactive product:** retained for historical purposes but unavailable for ordinary checkout.
- **Temporary basket:** in-memory, uncommitted selection of products.
- **Completed order:** an order whose required payment has been successfully processed under the current single-payment workflow.
- **Paid order:** an order whose payment status records successful payment.
- **Simulated notification:** a stored message that was not actually sent externally.

## 3. Product and Inventory Rules

### 3.1 Product catalog

| ID | Rule |
| --- | --- |
| BR-PRD-001 | Every product must have a unique product code. |
| BR-PRD-002 | Product names must not be empty or whitespace-only. |
| BR-PRD-003 | A category must be provided where required by the product-entry flow. |
| BR-PRD-004 | Unit price must not be negative. |
| BR-PRD-005 | Quantity in stock must not be negative. |
| BR-PRD-006 | Reorder level must not be negative. |
| BR-PRD-007 | Only active products may participate in ordinary sales and inventory operations. |
| BR-PRD-008 | Inactive products must not be added to or checked out from the basket. |
| BR-PRD-009 | A product with transaction history should be deactivated rather than hard-deleted so historical references remain intact. |
| BR-PRD-010 | A reactivated product becomes available for ordinary inventory operations, subject to the other product rules. |

### 3.2 Stock availability and low-stock alerts

| ID | Rule |
| --- | --- |
| BR-STK-001 | An active product is considered low stock when `QuantityInStock <= ReorderLevel`. |
| BR-STK-002 | Inactive products must be excluded from low-stock alerts. |
| BR-STK-003 | Stock may not be reduced below zero through an ordinary sale or adjustment. |

## 4. Sales Transaction Rules

### 4.1 Basket

| ID | Rule |
| --- | --- |
| BR-BSK-001 | An item added to the basket must refer to an active product. |
| BR-BSK-002 | Basket item quantity must be greater than zero. |
| BR-BSK-003 | The total quantity selected for a product must not exceed its available stock when checked against current inventory. |
| BR-BSK-004 | Each basket line total equals its quantity multiplied by its unit price; basket total is the sum of its line totals. |
| BR-BSK-005 | Removing an item or clearing the basket must not alter persisted product stock. |
| BR-BSK-006 | The basket remains temporary session data until successful checkout; clearing it must not delete saved orders or order items. |

### 4.2 Orders and checkout

| ID | Rule |
| --- | --- |
| BR-ORD-001 | Checkout must reject an empty basket. |
| BR-ORD-002 | Checkout must revalidate the availability and quantity of every selected product against current stock before saving the sale. |
| BR-ORD-003 | A new order must have a unique, business-facing order number. |
| BR-ORD-004 | An order must contain at least one order item. |
| BR-ORD-005 | Every saved order item must reference its parent order through `OrderId`. |
| BR-ORD-006 | Order items must preserve a transaction snapshot: product code, product name, quantity, unit price, and line total. |
| BR-ORD-007 | Product stock must be deducted only for a validated checkout. |
| BR-ORD-008 | An order must not be marked `Completed` until its required payment has been processed. |
| BR-ORD-009 | A completed checkout must preserve the saved order and its items after the temporary basket is cleared. |

### 4.3 Payments

The present workflow assumes **one completed payment per order**. Partial or split payments require an explicit revision to these rules before implementation.

| ID | Rule |
| --- | --- |
| BR-PAY-001 | The amount paid must be at least the amount due under the current full-payment workflow. |
| BR-PAY-002 | Change must equal `AmountPaid - AmountDue` when the amount paid exceeds the amount due; otherwise it is zero. |
| BR-PAY-003 | Every payment must record its payment method, date, payment number, amount due, amount paid, and change amount. |
| BR-PAY-004 | A successfully paid order must not accept an accidental second full payment. |
| BR-PAY-005 | Successful payment must update the order's payment status to `Paid` and order status to `Completed`. |
| BR-PAY-006 | Each payment must retain its relationship to the correct order. |

### 4.4 Receipts

| ID | Rule |
| --- | --- |
| BR-RCT-001 | A receipt may only be issued for a successfully paid order. |
| BR-RCT-002 | Under the current single-payment workflow, each payment must have at most one issued receipt. |
| BR-RCT-003 | Every receipt must have a unique business-facing receipt number and reference its associated payment and order. |
| BR-RCT-004 | Receipt output must contain order items, quantities, unit prices, totals, payment method, amount paid, and change. |
| BR-RCT-005 | Saved receipts must remain available for viewing and reprinting. |
| BR-RCT-006 | A receipt export failure must be reported without unexpectedly terminating the application. |

## 5. Stock Movement and Notification Rules

### 5.1 Stock movements

| ID | Rule |
| --- | --- |
| BR-STK-004 | Stock In must record a positive quantity change when stock increases. |
| BR-STK-005 | Stock Out must record a negative quantity change when stock decreases. |
| BR-STK-006 | An adjustment may record a positive, negative, or zero quantity change, according to the resulting stock difference. |
| BR-STK-007 | Each movement must record the related product, movement type, quantity change, stock before, stock after, reason, and date. |
| BR-STK-008 | For every movement, `StockAfter = StockBefore + QuantityChanged`. |
| BR-STK-009 | Stock changes must be reflected in both the current product quantity and the stock movement history. |
| BR-STK-010 | A checkout stock-out should retain the related order number for traceability. |

### 5.2 Notifications

| ID | Rule |
| --- | --- |
| BR-NTF-001 | A stored notification must contain its type, recipient, subject, message, creation date, and status. |
| BR-NTF-002 | Simulated notifications must be identifiable as simulations rather than real delivered messages. |
| BR-NTF-003 | Notification history must remain available for viewing. |
| BR-NTF-004 | Failure of a future external notification provider should be handled without unnecessarily terminating ordinary business operations. |
| BR-NTF-005 | An order-completed notification must relate to an order that has reached the completed state. |
| BR-NTF-006 | A receipt notification must relate to an existing receipt. |

## 6. Data Integrity, Access, and System Boundaries

### 6.1 Data integrity and lifecycle

| ID | Rule |
| --- | --- |
| BR-DAT-001 | SQLite is the intended persistent source of truth for products, orders, order items, payments, receipts, stock movements, and notifications. |
| BR-DAT-002 | Database-generated IDs and business-facing reference numbers must be treated as distinct identifiers. |
| BR-DAT-003 | Persistent business records must not depend on long-lived Console collections to survive application restarts. |
| BR-DAT-004 | Related data changes within one business transaction should succeed or fail together; checkout and related stock changes require atomic persistence before production use. |
| BR-DAT-005 | A development database reset must not be exposed as a normal production business action. |
| BR-DAT-006 | A successful database reset must recreate the configured development schema; seed records may be recreated when seeding is enabled. |

Implementation details for database transactions, foreign keys, reset safety, and test isolation belong in `database-design.md` and `architecture.md`.

### 6.2 Access and security

The following rules define the intended protected state; authentication and role-based authorization remain planned work.

| ID | Rule |
| --- | --- |
| BR-SEC-001 | Sensitive business operations must require authentication when user accounts are introduced. |
| BR-SEC-002 | Product deletion must be restricted to roles authorized to perform it. |
| BR-SEC-003 | Payment processing must be restricted to roles authorized to perform it. |
| BR-SEC-004 | User role assignments must control access to protected operations. |
| BR-SEC-005 | Credentials, tokens, passwords, and connection strings containing secrets must not be committed to Git. |

### 6.3 API behavior boundary

The same business rules apply regardless of whether an operation originates from the Console app or an API client. HTTP-specific conventions are specified in `api-design.md`, including JSON responses, consistent routes, `200 OK` for successful reads, `400 Bad Request` for applicable invalid input, `404 Not Found` for missing records, and avoidance of unnecessary internal error details. Controllers must not contain direct SQL.

## 7. Enforcement and Verification Register

This section records important gaps **without asserting that every documented rule has already been fully enforced or tested**. Keep the rows current; do not append a new milestone report after each release.

| Area | Verification or enforcement still needed | Related work |
| --- | --- | --- |
| Checkout | Verify all items are checked against current stock; introduce one database transaction for order, items, stock updates, and movements. | Repository-first refactor / regression testing |
| Basket | Verify that BasketService uses current repository-backed product data when adding items. Confirm that inactive products cannot be added, requested quantity is positive, and the combined quantity already in the basket plus the new request does not exceed available stock. | M47.2 / M48 |
| Basket state | Verify that removing an item or clearing the basket changes only temporary session state and does not modify persistent product stock. | M47.2 / M48 |
| Checkout revalidation | Verify that product availability is checked again during checkout because basket contents do not reserve inventory. | M47.3 / M48 |
| Order details | Verify saved orders reload their related order items correctly. | Service refactor / integration tests |
| Checkout | Verify final product availability and aggregate requested quantities against current SQLite stock before persistence. | M47.3 / M48 |
| Order relationships | Verify that every OrderItem uses the SQLite-generated parent `OrderId` and preserves its transaction snapshot values. | M47.3 / M48 |
| Inventory consistency | Verify that successful checkout persists the stock deduction and corresponding Stock Out movement with the OrderNumber as its reference. | M47.3 / M48 |
| Checkout atomicity | Order, OrderItem, product update, and stock movement operations currently use independent repository connections. Shared transaction handling remains an outstanding reliability requirement. | Future persistence hardening / M48 verification |
| Payments | Verify that only valid unpaid Orders can receive a full payment and that duplicate successful payments are rejected. | M47.4 / M48 |
| Payment consistency | Verify that the persisted Payment links to the correct Order and that successful payment updates both PaymentStatus and OrderStatus. | M47.4 / M48 |
| Receipts | Verify that receipts can only be generated from paid Payments and that the Receipt preserves the correct OrderId, PaymentId, OrderNumber, and PaymentNumber. | M47.4 / M48 |
| Duplicate receipts | Verify application-level prevention of multiple receipts for the same Payment. Database-level uniqueness remains future hardening. | M47.4 / M48 |
| Payment atomicity | Payment insertion and Order status updates currently use independent repository connections. Shared transaction handling remains an outstanding reliability improvement. | Future persistence hardening |
| Inventory | Verify stock history and product quantity cannot diverge after a partial failure. | Transaction handling / integration tests |
| Database reset | Verify safe development-only access, correct file path, initialization, and isolated test databases. | Development tooling / integration tests |
| API | Verify documented responses against live controller behavior and connect remaining temporary data sources when planned. | API integration / API tests |
| Access control | Implement and test authentication and role authorization. | Authentication and roles |
| Notifications | Test simulated history and failure handling when a real provider is introduced. | Integration tests / future email integration |

Detailed test steps and expected results belong in `acceptance-criteria.md` or automated test code, not in this document.

## 8. Document Maintenance

1. **Keep Sections 1–8 permanent.** Update the relevant rule or verification row when implementation changes.
2. **Do not create milestone headings.** Milestone dates, work items, and completion belong in `milestone-plan.md`; historical changes belong in `release-notes.md`.
3. **Do not reuse rule IDs.** If a rule is retired, retain its ID in version history and document its replacement when relevant.
4. **Keep rules technology-neutral where possible.** Put table/column details in `database-design.md`, endpoint details in `api-design.md`, and component wiring in `architecture.md`.
5. **Treat implementation and verification separately.** Change an open enforcement item only after confirming the code and relevant test results.
6. **Review affected rules for every change** to inventory, checkout, payments, receipts, stock movement, notifications, or authorization. Update the document's review date when that review is done.
