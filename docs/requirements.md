# StockFlow Software Requirements Specification

**Document:** `docs/requirements.md`  
**System:** StockFlow Inventory and Sales Management System  
**Document status:** Living specification  
**Last reviewed:** 2026-09-23

## 1. Purpose and Scope

This document specifies **what StockFlow must do** and the quality constraints it must meet. It is organized by stable business and technical domains, not by development milestone. Update an existing requirement when its implementation changes; add a new requirement only when the system gains a distinct capability or constraint.

StockFlow supports small-business product management, a temporary shopping basket, checkout, payments, receipts, inventory history, business summaries, and simulated notifications. The current application includes a SQLite-backed Console workflow and a partially integrated ASP.NET Core Web API. Authentication, a frontend, and broader production readiness are planned.

**Related documents:**

| Document | Responsibility |
|---|---|
| [`project-overview.md`](project-overview.md) | Business context, users, goals and scope |
| [`business-rules.md`](business-rules.md) | Detailed business rules and invariants |
| [`architecture.md`](architecture.md) | How the system is designed and organized |
| [`database-design.md`](database-design.md) | Database schema and relationships |
| [`api-design.md`](api-design.md) | HTTP routes, requests and responses |
| [`acceptance-criteria.md`](acceptance-criteria.md) | Detailed verification scenarios |
| [`milestone-plan.md`](milestone-plan.md) | Milestone and version schedule |
| [`release-notes.md`](release-notes.md) | Historical changes and releases |

## 2. Requirements Conventions

### 2.1 Status definitions

| Status | Meaning |
|---|---|
| **Implemented** | Implemented in the stated application surface; may still await automated regression tests |
| **In Progress** | Partially implemented, being refactored, or awaiting required behavior |
| **Planned** | Approved for an upcoming milestone or version but not implemented |
| **Deferred** | Previously planned but postponed |
| **Future Enhancement** | Possible capability outside the current committed scope |

A requirement's status is not evidence that automated testing has passed. **Surface** identifies where the stated behavior applies: Console, API, Shared, or Future. When Console works but API still uses sample data, describe the difference explicitly rather than claiming full implementation across both.

### 2.2 Stable identifiers

| Prefix | Domain |
|---|---|
| `INV` | Product and inventory management |
| `ORD` | Basket and order management |
| `PAY` | Payment management |
| `RCT` | Receipt management |
| `STK` | Stock movement and audit history |
| `DASH` | Dashboard and reporting |
| `NOTIF` | Notifications |
| `USER` | Identity and authorization |
| `DB` | Data persistence |
| `API` | Web API |
| `NFR` | Non-functional requirements |

Never reuse a retired identifier for a different requirement. Keep milestone references short; the full roadmap belongs in `milestone-plan.md`.

## 3. Functional Requirements

### 3.1 Product and Inventory Management

**Business need:** Maintain an accurate product catalog and current stock information, with controlled handling of inactive and historical products.

| ID | Requirement | Status | Surface | Reference / notes |
|---|---|---|---|---|
| INV-001 | Create products with a unique product code and required product details. | Implemented | Console | M02, M46.2; code-generation cleanup in M47 |
| INV-002 | View product records, including active records as appropriate to the operation. | Implemented | Console, API | M01, M25; product API reads SQLite |
| INV-003 | Search products by product code; Console may also search by name. | Implemented | Console, API | M02, M25 |
| INV-004 | Update a product's name, category, unit price, and reorder level. Inventory quantity changes must use a stock movement workflow that records the reason and the stock quantity before and after the operation. | Implemented | Console | M47.1; quantity changes through Add Stock and Adjust Stock |
| INV-005 | Deactivate a product without deleting its record. | Implemented | Console | M02, M46.3 |
| INV-006 | Reactivate an inactive product. | Implemented | Console | M46.3 |
| INV-007 | Allow hard deletion only when permitted by referential-integrity and business rules. | In Progress | Console | M02, M47; deletion safeguards need verification |
| INV-008 | Track and persist each product's current stock quantity. | Implemented | Console | M02, M46.6; transaction consistency under review |
| INV-009 | Identify active products whose stock is at or below their reorder level. | Implemented | Console | M12; API summary integration differs by endpoint |

### 3.2 Basket and Order Management

**Business need:** Select available products, validate quantities and convert a valid basket into a durable order while preserving purchase details.

| ID | Requirement | Status | Surface | Reference / notes |
|---|---|---|---|---|
| ORD-001 | Add available products to the current basket without exceeding available stock. | Implemented | Console | M04; M47.2 |
| ORD-002 | View basket items, quantities, prices and line totals. | Implemented | Console | M04 |
| ORD-003 | Remove a selected basket item. | Implemented | Console | M04 |
| ORD-004 | Clear the current basket. | Implemented | Console | M04 |
| ORD-005 | Calculate basket totals from item quantities and unit prices. | Implemented | Console | M04 |
| ORD-006 | Validate current stock and create an order from a valid basket. | Implemented | Console | M05, M46.4; atomic checkout is a reliability improvement |
| ORD-007 | Persist order header information with a business-facing order number. | Implemented | Console | M05, M46.4 |
| ORD-008 | Persist order items with an `OrderId` link and purchase-time product details. | Implemented | Console | M05, M46.4; full ID mapping in M47 |
| ORD-009 | View saved orders and their related order items. | In Progress | Console, API | M47; Console item loading being finalized, API integration separate |
| ORD-010 | Retrieve a specific order by its order number. | Implemented | Console; API route exists | M26, M46.4; API may use sample data |
| ORD-011 | Keep the current basket as session state; clearing it must not delete saved orders or order items. | Implemented | Console | M04, M47.2 |

### 3.3 Payment Management

**Business need:** Record payment transactions and reflect their effect on the associated order.

| ID | Requirement | Status | Surface | Reference / notes |
|---|---|---|---|---|
| PAY-001 | Process a payment against an existing, unpaid order. | Implemented | Console | M06, M46.5 |
| PAY-002 | Record the selected payment method. | Implemented | Console | M06 |
| PAY-003 | Reject a payment amount below the amount due. | Implemented | Console | M06 |
| PAY-004 | Calculate change when amount paid exceeds the amount due. | Implemented | Console | M06 |
| PAY-005 | Persist payment status and mark the associated order completed after successful payment. | Implemented | Console | M46.5; cross-record transaction safety still planned |
| PAY-006 | View persisted payment records. | Implemented | Console; API route exists | M27, M46.5; API data source to be verified |
| PAY-007 | Retrieve payments by payment number or associated order number. | Implemented | Repository / Console | M42; API currently has payment-number route |

**Current scope:** The Console flow processes full payment for an order. Repository support for multiple payments by order does **not** mean partial or split payments are implemented.

### 3.4 Receipt Management

**Business need:** Produce durable proof of payment and provide a readable or exportable transaction record.

| ID | Requirement | Status | Surface | Reference / notes |
|---|---|---|---|---|
| RCT-001 | Generate a receipt from a valid saved payment and its associated order. | Implemented | Console | M07, M46.5 |
| RCT-002 | Prevent more than one receipt from being issued for the same payment. | In Progress | Console | M47; add/verify lookup by `PaymentNumber`, ideally enforce uniqueness in SQLite |
| RCT-003 | Display saved order items, totals, payment method, amount paid and change. | Implemented | Console | M07; order items loaded from SQLite |
| RCT-004 | View previously saved receipts. | Implemented | Console | M07, M46.5 |
| RCT-005 | Export a saved receipt as a text file. | Implemented | Console | M13 |
| RCT-006 | Export receipts as PDF files. | Future Enhancement | Future | Not in current release scope |

### 3.5 Stock Movement and Audit History

**Business need:** Explain and trace every inventory quantity change.

| ID | Requirement | Status | Surface | Reference / notes |
|---|---|---|---|---|
| STK-001 | Save stock-in movements with positive quantity changes. | Implemented | Console | M11, M46.6 |
| STK-002 | Save stock adjustments, including increases and decreases. | Implemented | Console | M11, M46.6 |
| STK-003 | Save stock-out movements when checkout deducts inventory. | Implemented | Console | M11, M46.6 |
| STK-004 | Preserve stock-before, quantity-changed and stock-after values. | Implemented | Console | M11 |
| STK-005 | Record the movement reason and related business reference when available. | Implemented | Console | M11, M44 |
| STK-006 | View persisted stock movement history. | Implemented | Console | M46.6; removal of stale list parameters in M47 |

### 3.6 Dashboard and Reporting

**Business need:** Give the business a reliable summary of products, transactions, inventory warnings and sales performance.

| ID | Requirement | Status | Surface | Reference / notes |
|---|---|---|---|---|
| DASH-001 | Show product counts and current stock totals. | In Progress | Console; API route exists | M08, M28, M47; verify active/inactive count uses all products |
| DASH-002 | Show order counts by current status. | Implemented | Console; API route exists | M08, M14; API may use sample summary |
| DASH-003 | Show payment counts and paid-payment summary. | Implemented | Console; API route exists | M08, M14 |
| DASH-004 | Calculate sales income from paid transactions using amount due. | Implemented | Console | M08, M14 |
| DASH-005 | Display low-stock product information. | Implemented | Console; API route exists | M08, M12 |
| DASH-006 | Group paid sales by payment method. | In Progress | Console | M14, M47; standardize grouping on paid payments only |
| DASH-007 | Expose dashboard summary data through an HTTP endpoint. | Implemented | API | M28; route exists, real repository data integration still planned |

### 3.7 Notifications

**Business need:** Keep a record of important business events and provide a foundation for later outbound messaging.

| ID | Requirement | Status | Surface | Reference / notes |
|---|---|---|---|---|
| NOTIF-001 | Create simulated low-stock notifications. | Implemented | Console | M15, M46.6 |
| NOTIF-002 | Create simulated completed-order notifications only for completed orders. | Implemented | Console | M15; current order data read from repository |
| NOTIF-003 | Create simulated receipt notifications for existing receipts. | Implemented | Console | M15 |
| NOTIF-004 | Store notification type, recipient, content, reference, time and state. | Implemented | Console / Repository | M45 |
| NOTIF-005 | View persisted notification history. | Implemented | Console | M46.6 |
| NOTIF-006 | Persist and retrieve notifications through SQLite. | Implemented | Console / Repository | M45, M46.6 |
| NOTIF-007 | Send real email notifications. | Future Enhancement | Future | Current emails are simulations only |

### 3.8 Identity and Access Management

**Business need:** Control access to sensitive business operations as the application grows beyond its developer-operated Console workflow.

| ID | Requirement | Status | Surface | Reference / notes |
|---|---|---|---|---|
| USER-001 | Support persistent user accounts. | Planned | Future | v0.7.0 |
| USER-002 | Authenticate users through a login workflow. | Planned | Future | v0.7.0 |
| USER-003 | Enforce role-based authorization. | Planned | Future | v0.7.0 |
| USER-004 | Support Admin, Staff and Cashier roles. | Planned | Future | v0.7.0 |
| USER-005 | Protect sensitive actions, including product deletion and payment processing. | Planned | Future | v0.7.0 |

Existing Console business actions are not role-secured merely because their requirement descriptions refer to future authorized users.

## 4. Data and Persistence Requirements

| ID | Requirement | Status | Surface | Reference / notes |
|---|---|---|---|---|
| DB-001 | Identify durable business entities. | Implemented | Shared | M18 |
| DB-002 | Define database tables for durable entities. | Implemented | Infrastructure | M19, M39–M45 |
| DB-003 | Define primary/foreign keys and intended relationships. | Implemented | Infrastructure | M19; enforcement and tests should be verified |
| DB-004 | Preserve purchase-time product details in OrderItems. | Implemented | Shared / Infrastructure | M19, M46.4 |
| DB-005 | Maintain SQL CRUD reference scripts. | Implemented | Documentation | M20 |
| DB-006 | Initialize a local SQLite database. | Implemented | Infrastructure | M21 |
| DB-007 | Keep database access in Infrastructure repository classes. | Implemented | Infrastructure | M22, M39–M45 |
| DB-008 | Use SQLite instead of JSON as the primary persistent source for Console business records. | In Progress | Console | M46 completed main migration; M47 removes remaining transitional code |
| DB-009 | Support safe, development-only database recreation and database-path diagnostics. | In Progress | Console / Infrastructure | M47.8; not a production feature |
| DB-010 | Execute multi-record checkout/payment changes without leaving partial business transactions. | Planned | Console / Infrastructure | Reliability work; requires transaction design and tests |

## 5. API Requirements

These requirements describe available HTTP capabilities. A working route using temporary data is not equivalent to full SQLite-backed API integration; the endpoint implementation detail remains in [`api-design.md`](api-design.md).

| ID | Requirement | Status | Surface | Reference / notes |
|---|---|---|---|---|
| API-001 | Provide an ASP.NET Core Web API project. | Implemented | API | M24 |
| API-002 | Expose an OpenAPI document for endpoint discovery. | Implemented | API | M24; `/openapi/v1.json` |
| API-003 | Expose `GET /api/products`. | Implemented | API | M25; SQLite-backed |
| API-004 | Expose `GET /api/products/{productCode}`. | Implemented | API | M25; SQLite-backed |
| API-005 | Expose `GET /api/orders`. | Implemented | API | M26; repository integration not yet verified |
| API-006 | Expose `GET /api/orders/{orderNumber}`. | Implemented | API | M26; repository integration not yet verified |
| API-007 | Expose `GET /api/payments`. | Implemented | API | M27; repository integration not yet verified |
| API-008 | Expose `GET /api/payments/{paymentNumber}`. | Implemented | API | M27; repository integration not yet verified |
| API-009 | Expose `GET /api/dashboard/summary`. | Implemented | API | M28; repository integration not yet verified |
| API-010 | Return appropriate HTTP success and client-error responses. | Implemented | API | M29; expand automated verification later |
| API-011 | Provide interactive API documentation through Swagger UI or Scalar. | Future Enhancement | API | Optional |
| API-012 | Replace remaining temporary API sample data with real business data. | Planned | API | v0.7.0 API completion |

## 6. Non-Functional Requirements

| ID | Quality attribute | Requirement | Status | Verification / reference |
|---|---|---|---|---|
| NFR-001 | Input quality | Validate required values and expected numeric ranges; reject invalid input safely. | Implemented | Console validation; API route validation; M03, M29 |
| NFR-002 | Maintainability | Separate Console/UI orchestration, shared domain logic and Infrastructure persistence with correct dependency direction. | In Progress | Architecture review and build; M31–M37, M47 |
| NFR-003 | Durability | Persist durable business records between application restarts. | Implemented | SQLite restart checks; M46 |
| NFR-004 | Observability | Record useful application and error information without relying solely on Console output. | In Progress | Logging implementation and coverage review; M16 |
| NFR-005 | Reliability | Handle expected failures without inconsistent partial transactions or unexpected crashes. | In Progress | Manual regression and future transaction tests; M47–M48 |
| NFR-006 | Security | Protect privileged operations and keep secrets out of source control. | Planned | Authentication and authorization; v0.7.0 |
| NFR-007 | Testability | Provide repeatable automated unit and integration tests using isolated test data. | Planned | M48; expanded coverage in v0.9.0 |
| NFR-008 | Documentation | Maintain accurate, navigable Markdown requirements, design, acceptance criteria and release history. | In Progress | Review alongside relevant code changes; all releases |
| NFR-009 | Change management | Use Git, meaningful commits and release tags for released versions. | Implemented | Git history and tagged release checks |
| NFR-010 | Test isolation | Ensure automated tests and database resets cannot unintentionally erase development or production databases. | Planned | Dedicated temporary test database; M48 |

## 7. Scope Boundaries and Open Items

The following are intentionally **not** treated as implemented today: authenticated Console/API access; a production frontend; real outbound email; partial/split-payment workflows; production deployment; PDF receipt export; and broad automated regression coverage.

**Active verification and design work:** Finalize repository-first Console cleanup; confirm that order display loads related OrderItems; prevent duplicate receipts; verify inventory dashboard counts and sales grouping; isolate development/test databases; and design transaction boundaries for multi-step writes. Track work and completion dates in `milestone-plan.md`, not by appending new milestone sections here.

**Potential future enhancements:** customer/supplier management, product categories, audit logs, advanced and date-filtered reporting, PDF/Excel exports, barcode scanning and interactive API documentation. Move any enhancement into the numbered requirements tables when its scope is agreed.

## 8. Requirement Maintenance and Traceability

1. **Update in place.** Keep the section names and requirement IDs stable. Change the existing row's status, scope or notes when implementation evolves; do not append a new section for each milestone.
2. **Preserve identity.** Never renumber existing requirements after implementation. Add the next available ID within its domain only for genuinely new behavior.
3. **Separate implementation surfaces.** Explicitly say Console, API, Shared or Future when the same capability has different implementation status across applications.
4. **Separate evidence.** A code review or reported completion is not the same as a passing regression test. Detailed test cases belong in `acceptance-criteria.md`; test runs belong in the testing workflow.
5. **Record history elsewhere.** Milestone progress belongs in `milestone-plan.md`; completed changes and release dates belong in `release-notes.md`; implementation details belong in architecture, database and API design documents.
6. **Review on change.** For each feature/fix, check its requirement ID, business rules, acceptance criteria and affected design pages. Update `Last reviewed` when the document is reviewed against current source.
