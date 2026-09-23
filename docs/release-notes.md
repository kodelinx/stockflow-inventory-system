# StockFlow Release Notes

Last updated: 2026-09-24  
Document status: Maintained  
Latest released version: v0.5.0  
Next release: v0.6.0 (in progress)

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

### v0.6.0 — Database-Backed Console and Service Refactor

**Status:** Unreleased  
**Release date:** TBD

#### Summary

Moves the Console application's persistent business records from JSON/list-based workflows to SQLite repositories, then consolidates service responsibilities and introduces a foundation for repeatable testing.

#### Added

- Completed repositories for Products, Orders, OrderItems, Payments, Receipts, StockMovements, and Notifications.
- SQLite-backed checkout, payment processing, receipt generation, inventory movement history, and simulated notification storage.
- Internal database identifiers and relationships for saved order items, payments, and receipts.

#### Changed

- Disabled active JSON saving in favor of SQLite as the primary persistence source.
- Refactoring Console services to fetch persistent records through repositories rather than relying on long-lived `List<T>` arguments.
- Keeping `List<BasketItem>` as temporary session state until checkout; saved `OrderItem` records remain in SQLite after the basket is cleared.
- Updating order and receipt display to load related `OrderItems` from their own repository.
- Updating alerts, dashboard summaries, sales reports, and notification simulations to use current repository-backed data.
- Completed repository-first inventory-service cleanup and separated ordinary product-detail updates from audited stock-quantity adjustments.


#### Fixed

- Corrected repository query/reader column-order issues that could map business references into date fields.
- Corrected early order/OrderItem SQL and column-naming issues encountered during SQLite migration.

#### Remaining Before Release

- Finish repository-first service and `Program.cs` cleanup, including unused lists and legacy JSON remnants.
- Complete and verify duplicate-receipt protection and consistent business-number formatting.
- Complete development-only database reset/path diagnostics and confirm database isolation.
- Run full manual regression checks and implement the first automated repository/workflow tests.
- Confirm that `dotnet build` and `dotnet test` pass before tagging v0.6.0.


#### Known Limitations

- Service refactoring and automated regression coverage are still in progress.
- Some Console workflows still combine user interaction and business orchestration; they are not yet reusable pure Core services.
- API order, payment, and dashboard routes may still use temporary sample data until separately integrated and verified.
- Checkout currently spans multiple repository writes; an all-or-nothing database transaction has not yet been established.
- Authentication, frontend UI, and production deployment are not included in this release.

---

## 3. Released Versions

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

### v0.1.0 — Console Inventory and Sales MVP

**Status:** Released  
**Release date:** 2026-08-23

#### Summary

Delivered the first working Console MVP for inventory and basic sales management.

#### Added

- Product inventory CRUD and search, including deactivation and hard-delete cleanup.
- Reusable input validation, basket management, and checkout with order items.
- Payment methods, payment/change calculations, receipt generation, and dashboard summaries.
- JSON save/load support for Console data.

#### Changed

- Organized the initial application into models, services, utilities, and storage components.

#### Known Limitations at Release

- Console application with local JSON storage only.
- No database, API, authentication, automated tests, or receipt file export.
- Some identifiers were generated from in-memory list counts.

---

## 4. Maintenance Rules

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
