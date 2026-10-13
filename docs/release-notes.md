# StockFlow Release Notes

Last updated: 2026-10-11  
Document status: Maintained  
Latest released version: v0.6.0  
Next release: v0.7.0 (in progress)

## 1. Purpose and Conventions

This document records **version-level changes** to StockFlow. It is a changelog, not a daily development journal or milestone checklist.

Each version uses the same permanent format: **Summary**, **Added**, **Changed**, **Fixed** (when applicable), and **Known Limitations**. Add a new version section only when starting work toward a release; update that section in place until the release is published. Do not add a subsection for each milestone.

- **Release status:** `Unreleased` or `Released`.
- **Release date:** Record the date only after a release is confirmed against Git history or its tag.
- **Change categories:** `Added` = new capabilities; `Changed` = improved or refactored behavior; `Fixed` = corrected defects.
- **History rule:** Known limitations describe the system *at the time of that version*. Do not retroactively rewrite them when a later release resolves them.

For milestone status, see [`milestone-plan.md`](milestone-plan.md). For current system design, see [`architecture.md`](architecture.md), [`database-design.md`](database-design.md), and [`api-design.md`](api-design.md). For requirements and verification, see [`requirements.md`](requirements.md) and [`acceptance-criteria.md`](acceptance-criteria.md).

---

## 2. Unreleased

### v0.7.0 — Authentication, Roles, and API Completion

**Status:** Unreleased  
**Release date:** TBD

#### Summary

Expands StockFlow's API integration and introduces the identity and authorization foundation required for controlled application access.

#### Added

- Defined the initial StockFlow user-account requirements.
- Defined `Admin` and `Staff` application roles.
- Defined the initial role-permission matrix for inventory, sales, and user-management operations.
- Defined security requirements for password storage, inactive users, and protected operations.
- Added the shared `User` model for persisted application identities.
- Added SQLite-backed user-account persistence through `UserRepository`.
- Added unique usernames, application roles, active/inactive account status, and password-hash storage.
- Added automated repository tests for user persistence and lifecycle updates.
- Added secure password hashing for StockFlow user credentials.
- Added JWT-based authentication.
- Added `POST /api/auth/login`.
- Added user identity and role claims to issued authentication tokens.
- Added development administrator bootstrap using local secret configuration.

#### Changed

- Connected Order API reads to `OrderRepository` and live SQLite data.
- Connected Payment API reads to `PaymentRepository` and live SQLite data.
- Connected Dashboard API summary calculations to Product, Order, and Payment repository data.
- Removed typed temporary sample data from the remaining implemented API read endpoints.
- Completed repository-backed integration for the current StockFlow API read surface.

#### Known Limitations

- API write endpoints are not yet implemented.
- Authentication and role-based authorization are not yet implemented.
- Interactive Swagger/Scalar UI is not currently configured.
- Frontend integration remains planned for a later version.

## 3. Released Versions

### v0.6.0 — Database-Backed Console and Service Refactor

**Status:** Released  
**Release date:** 2026-10-11

#### Summary

v0.6.0 completes StockFlow's transition from legacy JSON/list-based Console persistence to a repository-first SQLite architecture.

The release moves persistent Console business data into SQLite-backed repositories, completes the repository-first Console service refactor, keeps the Basket as intentional temporary session state, and introduces StockFlow's first automated regression foundation using xUnit.


#### Added

- Completed repositories for Products, Orders, OrderItems, Payments, Receipts, StockMovements, and Notifications.
- SQLite-backed checkout, payment processing, receipt generation, inventory movement history, and simulated notification storage.
- Internal database identifiers and relationships for saved OrderItems, Payments, and Receipts.
- Development-only SQLite reset tooling with explicit path diagnostics and exact `RESET` confirmation.
- Isolated temporary SQLite databases for automated repository tests.
- xUnit repository integration tests for ProductRepository, OrderRepository, OrderItemRepository, PaymentRepository, ReceiptRepository, StockMovementRepository, and NotificationRepository.
- Full sales workflow regression testing across Product, Order, OrderItem, stock reduction, StockMovement, Payment, Order completion, and Receipt persistence.
- Core business-logic unit tests for `ProductManager`.
- Test cleanup safeguards for temporary SQLite files.
- Assembly-level xUnit configuration to prevent SQLite test cleanup from interfering with concurrently running tests.

#### Changed

- Replaced active JSON runtime persistence with SQLite as the authoritative persistence source for durable Console business records.
- Refactored Console services to fetch persistent records through repositories rather than depending on long-lived in-memory `List<T>` collections.
- Kept `List<BasketItem>` as intentional temporary session state until checkout.
- Updated order and receipt display flows to load related OrderItems from `OrderItemRepository`.
- Updated alerts, dashboard summaries, sales reports, and simulated notification workflows to use current repository-backed data.
- Completed repository-first inventory-service cleanup and separated ordinary product-detail updates from audited stock-quantity changes.
- Completed repository-first basket validation while keeping the Basket temporary and non-persistent.
- Improved basket stock validation so repeated additions of the same product respect the total quantity already present in the Basket.
- Improved Order persistence by using SQLite-generated `OrderId` values to link related OrderItems.
- Improved payment and receipt persistence by using generated internal IDs and persisted parent relationships.
- Added duplicate-payment and duplicate-receipt safeguards at the application level.
- Improved stock movement validation, including active-product checks, signed quantity changes, no-change adjustment handling, and OrderNumber references for checkout Stock Out records.
- Centralized low-stock evaluation around current repository-backed Product data.
- Updated Dashboard and Sales Report calculations to use persisted repository-backed information.
- Removed retired JSON runtime persistence options and legacy long-lived persistence collections from active Console workflow.
- Simplified Console composition and menu wiring while retaining temporary Basket state, logging, and receipt text export.
- Hardened development database reset with absolute database path diagnostics, explicit `RESET` confirmation, SQLite connection-pool cleanup, sidecar-file handling, database recreation, seed-data restoration, and temporary Basket clearing only after successful reset.
- Added a repeatable automated regression workflow through `dotnet test`.
- Added multiple testing levels: Core unit tests, repository integration tests, isolated SQLite database tests, full sales workflow regression, and manual Console regression.
- Disabled xUnit test parallelization for the current SQLite test suite because temporary database cleanup clears process-wide SQLite connection pools.

#### Fixed

- Corrected repository query/reader column-order issues that could map business references into incorrect model fields.
- Corrected early Order/OrderItem SQL and column-naming issues encountered during SQLite migration.
- Corrected Dashboard inactive-product counting so inactive products are not lost by reading only active-product results.
- Corrected Sales Report payment-method calculations to use Paid Payments consistently.
- Corrected stale-list dependencies that could make Console workflows display or operate on outdated business data.
- Corrected repeated Basket additions so their combined requested quantity cannot exceed available stock.
- Corrected SQLite automated-test instability caused by parallel tests clearing shared SQLite connection pools.

#### Testing and Verification

The v0.6.0 automated regression foundation includes:

- isolated SQLite database initialization and cleanup
- Product repository CRUD tests
- Order and OrderItem parent-child persistence tests
- Payment and Receipt relationship tests
- StockMovement audit-history tests
- Notification persistence and read-state tests
- full successful sales workflow regression
- ProductManager business-logic unit tests
- repeated full-suite execution through `dotnet test`

Manual regression covers Console-driven behavior that remains interactive, including application startup, Product management, Basket behavior, checkout, stock deduction and stock movement history, Orders and OrderItems, Payment processing, duplicate-payment protection, Receipt generation, duplicate-receipt protection, receipt export, manual stock operations, low-stock behavior, Dashboard, Sales Reports, Notifications, restart persistence, development reset cancellation, successful development reset, and logging.

#### Known Limitations

- Some Console workflow services still combine user interaction and business orchestration and are not yet reusable pure Core services.
- API Order, Payment, and Dashboard routes may still use temporary sample data until separately integrated and verified.
- Checkout, payment processing, and stock-update workflows span multiple repository writes and are not yet coordinated by one shared SQLite transaction.
- Product hard deletion requires stronger historical-reference safeguards before production use.
- Business-facing reference-number generation uses a simple strategy suitable for the current single-user learning workflow but is not designed for concurrent production use.
- Database-level duplicate-receipt protection can be strengthened with a uniqueness constraint.
- Foreign-key enforcement and relationship constraints should be verified and strengthened before production use.
- Authentication and role-based authorization are not implemented.
- Frontend UI is not implemented.
- Production deployment configuration is not included in this release.
- Simulated notifications are not real email delivery.
- Payment processing records payments internally and does not integrate with a real payment gateway.

---

### v0.5.0 — Shared Architecture and Product API Integration

**Status:** Released  
**Release date:** 2026-09-13

#### Summary

Separated shared business models, reusable product rules, and database implementation into Core and Infrastructure projects, reducing coupling between the Console and API applications.

#### Added

- `StockFlow.Core` class library for shared models and reusable business logic.
- `StockFlow.Infrastructure` class library for SQLite/database and repository implementation.
- `ProductManager` for reusable product business rules.

#### Changed

- Moved shared models into Core and moved `DatabaseConnectionService` and `ProductRepository` into Infrastructure.
- Connected Product API endpoints to Core product rules and Infrastructure data access.
- Kept Console-specific input/output services in `StockFlow.Console` rather than moving them into Core.

#### Fixed

- Removed the API dependency on Console and resolved duplicate-type, circular-reference, and project-dependency issues.

#### Known Limitations at Release

- Only product business rules had been extracted into reusable Core services.
- Other repositories and fully database-backed Console workflows were scheduled for v0.6.0.
- Order, payment, and dashboard API areas still used temporary sample data.

---

### v0.4.0 — StockFlow Web API

**Status:** Released  
**Release date:** Verify against Git tag (the previous notes list both 2026-09-10 and 2026-09-11).

#### Summary

Introduced ASP.NET Core Web API endpoints and basic HTTP response conventions for StockFlow.

#### Added

- `StockFlow.Api` project with OpenAPI JSON at `/openapi/v1.json`.
- Read endpoints for products, orders, and payments.
- `GET /api/dashboard/summary` for dashboard data.
- Basic input validation and `200 OK`, `400 Bad Request`, and `404 Not Found` responses where appropriate.

#### Changed

- Connected product reads to SQLite through `ProductRepository`.

#### Known Limitations at Release

- Order, payment, and dashboard endpoints used typed temporary sample data.
- Endpoints were primarily read-only; API create/update/delete workflows were not yet implemented.
- Authentication and interactive Swagger/Scalar UI were not configured.

---

### v0.3.0 — Database-Ready Inventory System

**Status:** Released  
**Release date:** 2026-09-02

#### Summary

Established the initial SQLite foundation while the Console application continued to use JSON for its primary workflow.

#### Added

- Database requirements, table design, and SQL CRUD scripts.
- SQLite package support and `DatabaseConnectionService`.
- Initial `Products` table initialization and `ProductRepository` read/insert methods.
- Parameterized SQL commands and SQLite database-file ignore rules.

#### Changed

- Introduced the repository pattern and separated database setup from product data access.

#### Known Limitations at Release

- JSON remained the Console application's primary storage.
- Only initial product database access had been implemented; other repositories and end-to-end database workflows were future work.

---

### v0.2.0 — Inventory Rules and Reporting

**Status:** Released  
**Release date:** 2026-08-29

#### Summary

Expanded the Console MVP with inventory traceability, alerts, reports, receipt export, notification simulations, and logging preparation.

#### Added

- Stock-in, stock adjustment, and checkout stock-out movement history.
- Low-stock alerts and dashboard/reporting improvements, including sales by payment method.
- Text receipt export and simulated low-stock, completed-order, and receipt emails.
- Notification history/JSON persistence and a basic logging service.

#### Changed

- Separated stock movement, alert, reporting, notification, and logging concerns into dedicated services.

#### Known Limitations at Release

- Console and JSON storage only; no SQLite-backed transaction flow or Web API.
- Notifications were simulated, not real email delivery.
- No automated tests, authentication, or advanced reporting filters.

---

### v0.1.0 — Console Inventory and Sales MVP

**Status:** Released  
**Release date:** 2026-08-23

#### Summary

Delivered the first working Console MVP for inventory and basic sales management.

#### Added

- Product inventory CRUD and search, including deactivation and hard-delete cleanup.
- Reusable input validation, Basket management, and checkout with OrderItems.
- Payment methods, payment/change calculations, Receipt generation, and Dashboard summaries.
- JSON save/load support for Console data.

#### Changed

- Organized the initial application into models, services, utilities, and storage components.

#### Known Limitations at Release

- Console application with local JSON storage only.
- No database, API, authentication, automated tests, or receipt file export.
- Some identifiers were generated from in-memory list counts.

---

## 3. Maintenance Rules

1. Add **one version section** when development starts on a new version; do not append one section per milestone.
2. Keep updates short and user- or architecture-facing. Detailed work logs belong in Git commits, milestone notes, and Notion.
3. Record only verified, shipped items as completed in a released version. While a version is unreleased, keep incomplete work under **Remaining Before Release**.
4. Confirm release dates and tags against Git before marking a version released.
5. Update the existing **Known Limitations** list for an unreleased version as work progresses; preserve historical limitations in released versions.
6. Update current technical details in their owning documents instead of duplicating architecture, schemas, and endpoint specifications here.

### Release Entry Template

Copy this format only when starting the *next version*:

```markdown
### vX.Y.Z — Release Name

**Status:** Unreleased  
**Release date:** TBD

#### Summary

One short paragraph describing the release outcome.

#### Added

- New capability.

#### Changed

- Improved behavior or structure.

#### Fixed

- Resolved issue, if applicable.

#### Remaining Before Release

- Pending verification or work. Remove this section after release.

#### Known Limitations

- Important limitations that remain in this version.
```
