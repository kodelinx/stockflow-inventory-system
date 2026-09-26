# StockFlow Architecture

> **Document type:** Living technical reference  
> **Status:** Implemented architecture with ongoing Console refactoring  
> **Last reviewed:** 2026-09-23

## 1. Purpose and Scope

This document describes StockFlow's system structure, project boundaries, dependency rules, data flow, and architectural decisions. It is a **current-state reference**, not a milestone journal: update the relevant existing section when the implementation changes instead of appending a new section for every milestone.

Related documents:

- [`requirements.md`](requirements.md) — functional and non-functional requirements.
- [`business-rules.md`](business-rules.md) — business constraints and transaction rules.
- [`database-design.md`](database-design.md) — tables, keys, relationships, and data types.
- [`api-design.md`](api-design.md) — endpoint contracts and API-specific behavior.
- [`acceptance-criteria.md`](acceptance-criteria.md) — verifiable behavior and regression criteria.
- [`milestone-plan.md`](milestone-plan.md) — work status and upcoming milestones.
- [`release-notes.md`](release-notes.md) — historical changes and releases.

## 2. Architecture Overview

StockFlow uses a layered .NET solution with two application entry points, shared business models and rules, and SQLite-backed persistence. The Console application contains the main operational workflow. The Web API currently has repository-backed product reads; other API areas require verification or further integration before being described as fully database-backed.

```text
Console user                         API client / future frontend
     |                                           |
     v                                           v
StockFlow.Console                             StockFlow.Api
(menu, input/output, workflows)               (HTTP controllers)
     |                                           |
     +----------> shared models and rules <------+ 
     |                 StockFlow.Core            |
     |                                           |
     +---------> StockFlow.Infrastructure <-------+
                  (repositories, database)
                             |
                             v
                          SQLite
```

**Important:** This is a logical interaction diagram, not a project-reference chain. `StockFlow.Core` does not reference Infrastructure; both application projects reference Core and Infrastructure as needed.

### Technology and architectural style

| Area | Current approach |
| --- | --- |
| Runtime and language | C# / .NET |
| Console interface | Menu-driven Console application |
| HTTP interface | ASP.NET Core Web API |
| Shared domain code | `StockFlow.Core` class library |
| Persistence | SQLite through `Microsoft.Data.Sqlite` |
| Data access | Repository classes in `StockFlow.Infrastructure` |
| Dependency composition | Explicit construction in Console `Program.cs`; DI registrations in the API |
| Testing | Manual regression checks currently; automated tests planned |

## 3. Solution Structure

The following tree shows the intended *logical placement* of the existing projects. It is not an exhaustive list of every file, and optional or planned test folders should not be mistaken for implemented test projects.

```text
stockflow-inventory-system/
├── README.md
├── docs/
│   ├── architecture.md
│   ├── requirements.md
│   ├── business-rules.md
│   ├── database-design.md
│   ├── api-design.md
│   ├── acceptance-criteria.md
│   ├── milestone-plan.md
│   └── release-notes.md
├── src/
│   ├── StockFlow.Console/
│   │   ├── Services/
│   │   ├── Utilities/
│   │   ├── Data/                 # legacy JSON service, while retained
│   │   └── Program.cs
│   ├── StockFlow.Api/
│   │   ├── Controllers/
│   │   └── Program.cs
│   ├── StockFlow.Core/
│   │   ├── Models/
│   │   ├── Services/
│   │   └── Interfaces/           # as interfaces are introduced
│   └── StockFlow.Infrastructure/
│       ├── Database/
│       └── Repositories/
├── tests/                        # automated test project planned
└── StockFlow.sln
```

### Project responsibilities

| Project | Owns | Does not own |
| --- | --- | --- |
| `StockFlow.Console` | Menu routing, user input/output, Console workflow orchestration, temporary basket session | SQL statements, shared domain models, HTTP endpoints |
| `StockFlow.Api` | HTTP controllers, request validation and responses, API configuration and dependency registration | Console interaction, database SQL inside controllers |
| `StockFlow.Core` | Shared models and reusable business rules without interface-specific input/output | Console input/output, HTTP response handling, SQLite connections |
| `StockFlow.Infrastructure` | SQLite connection setup, schema initialization and repositories | Menu routing, HTTP request handling, user prompts |

### Main components

- **Core models:** `Product`, `BasketItem`, `Order`, `OrderItem`, `Payment`, `Receipt`, `StockMovement`, and `Notification`.
- **Core business logic:** `ProductManager`, including reusable product rules; additional pure business logic can be extracted here when it is independent of the Console.
- **Console services:** `InventoryService`, `BasketService`, `OrderService`, `PaymentService`, `ReceiptService`, `StockMovementService`, `AlertService`, `DashboardService`, `SalesReportService`, and `NotificationService`.
- **Console utilities:** `InputValidationService` and `LoggingService`.
- **Infrastructure database service:** `DatabaseConnectionService`.
- **Infrastructure repositories:** `ProductRepository`, `OrderRepository`, `OrderItemRepository`, `PaymentRepository`, `ReceiptRepository`, `StockMovementRepository`, and `NotificationRepository`.
- **API controllers:** Product, order, payment, and dashboard controllers. Exact routes and their verified data sources belong in [`api-design.md`](api-design.md).

## 4. Dependency Rules

Project references must point in these directions:

```text
StockFlow.Console ──────> StockFlow.Core
        └──────────────> StockFlow.Infrastructure ──> StockFlow.Core

StockFlow.Api ──────────> StockFlow.Core
        └──────────────> StockFlow.Infrastructure
```

`StockFlow.Core` has no dependency on any other StockFlow project. Infrastructure must not reference Console or API. Console and API must not reference each other. These restrictions prevent circular project references and allow shared business code and data access to be reused by either entry point.

Within an application workflow:

1. The entry point routes a request to the appropriate service or controller.
2. A Console service coordinates user interaction and the relevant business operation; an API controller handles HTTP concerns.
3. Repositories perform persistent reads and writes.
4. Shared models represent business data; pure, interface-independent rules belong in Core.

**Current boundary:** Console services still contain `Console.ReadLine`, `Console.WriteLine`, and `InputValidationService` use. They remain in Console, not Core. Repository-first refactoring does not by itself make a Console service reusable in the API.

## 5. Data Ownership and Persistence

### Persistent records

SQLite is the primary persistent source for the Console application's product, order, order-item, payment, receipt, stock-movement, and notification records. Repositories should retrieve current records when a workflow needs them; long-lived copies in `Program.cs` are being removed.

| Data | Repository | General role |
| --- | --- | --- |
| Products | `ProductRepository` | Product catalog, stock quantity, active status |
| Orders | `OrderRepository` | Order header and payment/order statuses |
| Order items | `OrderItemRepository` | Purchased-item snapshots linked to a parent order |
| Payments | `PaymentRepository` | Recorded payments linked to orders |
| Receipts | `ReceiptRepository` | Issued receipts linked to orders and payments |
| Stock movements | `StockMovementRepository` | History of inventory quantity changes |
| Notifications | `NotificationRepository` | Simulated system messages and their read state |

Full columns, keys, constraints, and relationships are maintained in [`database-design.md`](database-design.md), not duplicated here.

### Session state

`List<BasketItem>` remains intentional temporary state for the current Console shopping session. At checkout, basket entries are converted into separate `OrderItem` records that are persisted in SQLite. Clearing the basket **does not** clear or delete saved order items.

A list returned by a repository may still be used as a *local method variable* for processing or display. It should not become a second source of truth that must be manually synchronized across services.

### Record identity

- SQLite-generated IDs, such as `OrderId`, `OrderItemId`, `PaymentId`, and `ReceiptId`, identify database records and support relationships.
- Application-generated references, such as `OrderNumber`, `PaymentNumber`, and `ReceiptNumber`, identify transactions in the user-facing workflow.
- `OrderRepository` loads order headers. Workflows that need purchased items load the corresponding rows through `OrderItemRepository` using `OrderId`.

### Legacy JSON

JSON was used as the original persistence mechanism. Its active save path has been disabled during migration; any retained `JsonStorageService` or JSON files are legacy code/data rather than authoritative records. Final removal is handled as code cleanup, not by creating additional architecture sections.

## 6. Application and Business Flows

These diagrams document stable responsibilities. Detailed rules and acceptance cases belong in the linked domain documents.

### Product and inventory management

```text
Console menu
    |
    v
InventoryService ──> ProductManager (pure product rules, where used)
    |
    v
ProductRepository ──> SQLite Products
```

Product management covers read, create, update, deactivate, reactivate, and controlled deletion. Soft deletion is preferred where transaction history must be preserved. `AlertService` evaluates low-stock conditions from current product records.

### Basket and checkout

```text
List<BasketItem> (session)
    |
    v
OrderService: validate current product availability and stock
    |
    +──> OrderRepository: insert order header
    |            |
    |            v
    |        saved OrderId
    |
    +──> OrderItemRepository: insert each OrderItem using OrderId
    |
    +──> ProductRepository: persist deducted stock quantities
    |
    +──> StockMovementRepository: record stock-out history
    |
    v
Clear basket only after successful checkout
```

The order header is saved before its items so the items can use the generated parent `OrderId`. The current implementation coordinates multiple repository calls from `OrderService`; **a single cross-repository SQLite transaction is a reliability improvement still to be implemented/verified**. Do not describe checkout as atomic until that improvement exists.

### Basket Management

The Console application keeps `List<BasketItem>` as temporary session state for the current shopping basket.

`BasketService` is responsible for basket operations such as adding products, viewing basket contents, removing items, and clearing the basket.

Persistent product information should be obtained through `ProductRepository` rather than through a separately maintained in-memory product catalog.

The intended flow is:

Program.cs
    |
    v
BasketService
    |
    +--> List<BasketItem>   Temporary session state
    |
    v
ProductRepository
    |
    v
SQLite Products

```markdown
Basket operations retrieve current product information through `ProductRepository` while keeping the basket itself as temporary session state.

The basket validates product availability before an item is added, including the total quantity already present in the basket. Removing or clearing basket items does not modify persistent stock.

Final product availability is revalidated during checkout because the basket does not reserve inventory.

### Checkout and Order Processing

Checkout converts the temporary `List<BasketItem>` session state into persistent sales records.

`OrderService` performs final product and stock validation using repository-backed product data before creating an order.

The checkout flow is:

List<BasketItem>
    |
    v
OrderService
    |
    +--> ProductRepository
    |       Revalidate active products and available stock
    |
    +--> OrderRepository
    |       Persist the parent Order
    |       Return the SQLite-generated OrderId
    |
    +--> OrderItemRepository
    |       Persist child OrderItems using OrderId
    |
    +--> ProductRepository
    |       Persist stock deductions
    |
    +--> StockMovementService
            Persist Stock Out audit records

### Payment and order completion

```text
PaymentService
    |
    +──> OrderRepository: find order and validate unpaid status
    +──> PaymentRepository: insert payment
    +──> OrderRepository: set PaymentStatus = Paid
    +──> OrderRepository: set OrderStatus = Completed
```

The order status is stored in SQLite. A later notification workflow reads the updated persisted order rather than relying on a stale in-memory order list. Payment insertion and order-status updates likewise require transaction-level consistency work before being considered atomic.

### Payment and Receipt Processing

Payment processing retrieves the related Order from SQLite and validates that the order has not already been paid.

A successful payment creates a persistent Payment record linked to the parent Order through `OrderId`. The Payment also preserves the business-facing `OrderNumber`, payment method, amount due, amount paid, change amount, payment date, and payment status.

The payment flow is:

```text
Order
    |
    v
PaymentService
    |
    +--> OrderRepository
    |       Load and validate Order
    |
    +--> PaymentRepository
    |       Persist Payment and return PaymentId
    |
    +--> OrderRepository
            Update PaymentStatus and OrderStatus
```
Receipt generation begins from a persisted paid Payment. The related Order and OrderItems are reloaded from SQLite before the receipt is generated.
```text
Payment
    |
    v
ReceiptService
    |
    +--> PaymentRepository
    |       Load Payment
    |
    +--> ReceiptRepository
    |       Check for an existing receipt
    |
    +--> OrderRepository
    |       Load related Order
    |
    +--> OrderItemRepository
    |       Load purchased OrderItems
    |
    +--> ReceiptRepository
            Persist Receipt and return ReceiptId
```
### Receipt generation and export

```text
ReceiptService
    |
    +──> PaymentRepository: find payment
    +──> ReceiptRepository: check for existing receipt (in progress)
    +──> OrderRepository: load associated order
    +──> OrderItemRepository: load purchased items
    +──> ReceiptRepository: save or retrieve receipt
    +──> Console display / text-file export
```

Receipt output includes item snapshots loaded from `OrderItems`. The intended rule is one receipt per payment; application-level duplicate protection is being added and a database uniqueness constraint should be considered for stronger enforcement.

### Inventory movements, alerts, and notifications

```text
StockMovementService ──> ProductRepository (current quantity)
                  └────> StockMovementRepository (movement history)

AlertService ──────────> ProductRepository (low-stock data)

NotificationService ───> Product / Order / Receipt repositories
                    └─> NotificationRepository (saved notification)

DashboardService / SalesReportService
                    └─> relevant repositories (current summaries)
```

Stock-in uses a positive `QuantityChanged`; stock-out uses a negative value; adjustments may be positive, negative, or zero. Notifications are simulations, not actual outgoing emails.

## 7. API Architecture and Integration Status

The API is a separate ASP.NET Core entry point. Controllers own HTTP concerns and may use Core business logic and Infrastructure repositories through configured dependencies.

```text
HTTP request
    |
    v
StockFlow.Api controller
    |
    +──> Core business rules, where integrated
    +──> Infrastructure repository, where integrated
    |
    v
HTTP JSON response
```

| API area | Documented integration state |
| --- | --- |
| Products | Reads from `ProductRepository` / SQLite and uses `ProductManager` for low-stock calculation |
| Orders | Earlier implementation uses typed temporary sample data; live repository integration not yet confirmed |
| Payments | Earlier implementation uses typed temporary sample data; live repository integration not yet confirmed |
| Dashboard | Earlier implementation uses typed temporary sample data; live repository integration not yet confirmed |

The OpenAPI JSON document is available at `/openapi/v1.json` when the API is running. Exact routes, responses, and verification status are tracked in [`api-design.md`](api-design.md). No live order/payment/dashboard integration should be claimed until the corresponding controllers have been checked.

## 8. Cross-Cutting Concerns

### Validation and error handling

Console input validation lives in `InputValidationService`. SQL parameters are used in repositories for values supplied to SQL commands. Controllers handle HTTP-specific validation and response codes. These measures do not replace transaction checks or database constraints.

### Logging

`LoggingService` supports application diagnostics. Its original JSON save/load logging context is historical; logging coverage of newer SQLite workflows should be verified separately rather than assumed.

### Database lifecycle and diagnostics

`DatabaseConnectionService` owns connection configuration and schema initialization (`InitializeDatabase()`). Development helpers such as `GetDatabaseFilePath()` and `ResetDatabase()` may be available as the ongoing refactor is completed. Reset must be restricted to development/testing, require deliberate user confirmation in the Console, and clear the current basket if invoked during an active session. Tests should use an **isolated database path**, never the working development database.

`CREATE TABLE IF NOT EXISTS` creates missing tables; it does not migrate the shape of existing tables. Schema changes require an explicit migration strategy or a deliberate development reset.

### Testing

Manual regression checks currently verify the end-to-end Console workflow. Automated unit and repository integration tests are planned, with independent temporary SQLite databases and a repeatable `dotnet test` command. A successful build alone does not verify business behavior or persistence.

### Security and deployment

Authentication, role-based authorization, production deployment configuration, and real email delivery are not implemented in the current architecture. Database reset and destructive product operations require stricter controls before production use.

## 9. Architecture Principles and Constraints

1. **Separation of concerns:** Entry points handle interfaces; Console services coordinate workflows; Core holds reusable business rules; Infrastructure implements persistence.
2. **Dependency direction:** Core depends on no application or Infrastructure project. Neither application entry point references the other.
3. **Single persistent source of truth:** SQLite owns saved business records; repository results are working copies.
4. **Intentional session state:** Keep the basket in memory until requirements call for persisted baskets.
5. **Explicit related-data loading:** Load `OrderItems` when full order details or receipts require them.
6. **Historical integrity:** Preserve product snapshots in order items; prefer product deactivation over deletion when history exists.
7. **Incremental refactoring:** Keep existing behavior testable while removing transitional list and JSON dependencies.
8. **Verifiable documentation:** Record current implementation here and milestone progress in `milestone-plan.md`; do not imply future work is already complete.

### Current constraints and improvement priorities

| Area | Current constraint or work in progress |
| --- | --- |
| Console architecture | Some services and `Program.cs` still contain transitional list arguments or legacy setup |
| Transaction consistency | Multi-repository checkout and payment writes are not yet verified to run atomically |
| Receipts | Duplicate-per-payment protection is being implemented/verified |
| API | Non-product areas may still use sample data |
| Automated tests | Repository isolation and regression suite are planned |
| Production security | Authentication, roles, and guarded administrative operations remain future work |

These are maintained as **current constraints**, not milestone-specific subsections. Remove or revise each row when the implementation changes.

## 10. Maintenance Rules

Keep this document stable as the project grows:

- **Update existing sections in place** when classes, dependencies, data ownership, or flows change. Do not add “Mxx architecture update” sections.
- **Update the solution tree and component inventory** only when projects or responsibilities change, not for every new method.
- **Maintain diagrams at the workflow level**. Repository method signatures and table schemas belong in code and `database-design.md`.
- **State current vs. planned behavior explicitly**. Move work out of the constraints table only after code review and tests support the change.
- **Record why an important architectural decision changed** in an Architecture Decision Record (ADR) if needed; record when in `release-notes.md` and its milestone status in `milestone-plan.md`.
- **Review this file before each release** and update the “Last reviewed” date after checking it against the repository.
