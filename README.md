# StockFlow Inventory System

StockFlow is a C#/.NET inventory and sales management application for small businesses. It supports product management, stock tracking, checkout, payment recording, receipts, reporting, and simulated notifications. The project is also a development portfolio that demonstrates an incremental transition from a console MVP to a layered, database-backed application with a Web API.

> **Development status:** v0.6.0 released · Current development: v0.7.0 — Authentication, User Roles, and API Completion · M50 — Connect Remaining API Reads to Real Repositories.


## Overview

StockFlow is intended to help small businesses keep inventory and sales records in one system. Its current development focus is final v0.6.0 release verification and documentation after completing the repository-backed Console workflow and automated regression foundation.

### Capabilities

| Area | Current scope |
| --- | --- |
| Inventory | Add, search, update, deactivate/reactivate, and manage products |
| Sales | Temporary basket, checkout, and persistent orders and order items |
| Payments and receipts | Record payments, calculate change, generate/view receipts, and export receipt text files |
| Stock | Stock-in, adjustments, checkout stock-out history, and low-stock alerts |
| Insights | Inventory dashboard and sales summaries |
| Notifications | Simulated low-stock, completed-order, and receipt messages |
| API | Read endpoints for products, orders, payments, and dashboard summary; integration levels vary by endpoint |

These features describe implemented workflows developed to date. See [Requirements](docs/requirements.md) and [Acceptance Criteria](docs/acceptance-criteria.md) for tracked status and verification conditions.

## Technology and Architecture

- **Language and runtime:** C# / .NET 10
- **API:** ASP.NET Core Web API with OpenAPI document support
- **Database:** SQLite via `Microsoft.Data.Sqlite`
- **Architecture:** Separate Console, API, Core, and Infrastructure projects; repository-based database access
- **Development:** Git, GitHub, and Markdown documentation
- **Testing:** xUnit unit tests, SQLite repository integration tests, isolated temporary test databases, workflow regression testing, and manual Console regression

```text
stockflow-inventory-system/
├── src/
│   ├── StockFlow.Console/        # Menu, user interaction, and Console workflows
│   ├── StockFlow.Api/            # HTTP endpoints and API startup
│   ├── StockFlow.Core/           # Shared models and reusable business logic
│   └── StockFlow.Infrastructure/ # SQLite setup and repositories
├── tests/
│   └── StockFlow.Tests/          # xUnit unit, integration, and workflow tests
├── docs/                         # Project specifications and engineering records
└── StockFlow.sln
```

The Console's persistent business records use SQLite repositories as the authoritative data source. `List<BasketItem>` intentionally remains temporary session state. The repository-first Console refactor is complete, while some API endpoints still require migration or verification against live repository-backed data.

For component boundaries and data flows, see [Architecture](docs/architecture.md) and [Database Design](docs/database-design.md).

### Persistence

StockFlow uses SQLite as its authoritative persistence layer.

Persistent application data includes Products, Orders, OrderItems, Payments, Receipts, StockMovements, and Notifications.

Legacy JSON save/reload persistence has been retired.

The shopping basket remains temporary session state and is cleared when the Console application ends.

File output is still used for operational logging and explicit receipt text exports, but these files are not the application's primary data store.

### Development Database Reset

The Console application includes a development-only SQLite reset operation.

Reset displays the active database path and requires the exact confirmation text `RESET`. A successful reset recreates and reseeds the development database and clears temporary basket state.

Cancelling the operation leaves the database and basket unchanged.

## Getting Started

### Prerequisites

- .NET 10 SDK
- Git (to clone the repository)
- An editor such as Visual Studio Code or Visual Studio

### Build and run

From the repository root:

```powershell
dotnet restore
dotnet build
```

Start the Console application:

```powershell
dotnet run --project src/StockFlow.Console
```

Start the API in a separate terminal:

```powershell
dotnet run --project src/StockFlow.Api
```

Use the base address printed by the API at startup (the port may vary). Available read routes include:

```text
GET /api/products
GET /api/products/{productCode}
GET /api/orders
GET /api/orders/{orderNumber}
GET /api/payments
GET /api/payments/{paymentNumber}
GET /api/dashboard/summary
```

The OpenAPI document is available at `/openapi/v1.json` when enabled in the current environment. Some order, payment, and dashboard API routes may still return temporary sample data; consult [API Design](docs/api-design.md) for endpoint-level status.

### Database and Test Data

The application uses a local SQLite database. Database initialization occurs when application startup calls `InitializeDatabase()`; `dotnet build` alone does not create the database.

`DatabaseConnectionService` resolves the configured development database path for repository connections and diagnostics. Development database reset is destructive and should only be used with disposable development data.

Generated SQLite database files and sidecar files such as `.db`, `.db-wal`, and `.db-shm` should not be committed to Git.

### Automated Testing

StockFlow includes an xUnit automated regression suite.

Current automated coverage includes:

- Core business logic unit tests
- Product repository integration tests
- Order and OrderItem integration tests
- Payment and Receipt integration tests
- StockMovement and Notification integration tests
- isolated temporary SQLite test databases
- full sales workflow regression testing

Run the complete automated test suite from the repository root:

```powershell
dotnet test
```

Automated repository tests use isolated temporary SQLite databases and must not modify the normal development database.

The actual Console interface and interactive business workflows are also verified through manual regression testing.

## Documentation

| Document | What it contains |
| --- | --- |
| [Project Overview](docs/project-overview.md) | Business context, intended users, goals, and scope |
| [Requirements](docs/requirements.md) | Tracked functional and non-functional requirements |
| [Business Rules](docs/business-rules.md) | Rules that govern products, sales, and persistent data |
| [Architecture](docs/architecture.md) | Project boundaries, dependencies, and application flows |
| [Database Design](docs/database-design.md) | Tables, relationships, persistence, and data conventions |
| [API Design](docs/api-design.md) | Endpoint catalog, contracts, and verification guidance |
| [Acceptance Criteria](docs/acceptance-criteria.md) | Conditions for verifying features and releases |
| [Milestone Plan](docs/milestone-plan.md) | Complete version roadmap and active milestone status |
| [Release Notes](docs/release-notes.md) | Released changes and work in progress for the next release |

## Roadmap

| Version | Focus | Status |
| --- | --- | --- |
| v0.1.0 | Console inventory and sales MVP | Released |
| v0.2.0 | Stock movements, alerts, reporting, and notification simulation | Released |
| v0.3.0 | Database design and initial SQLite/repository integration | Released |
| v0.4.0 | Initial Web API read endpoints | Released |
| v0.5.0 | Shared Core and Infrastructure architecture | Released |
| v0.6.0 | SQLite-backed Console, repository-first cleanup, and automated regression foundation | Released |
| v0.7.0 | Remaining API integration, authentication, and roles | In Progress |
| v0.8.0 | Frontend web dashboard | Planned |
| v0.9.0 | Expanded testing and production readiness | Planned |
| v1.0.0 | Business MVP release | Planned |

The authoritative milestone breakdown is maintained in [Milestone Plan](docs/milestone-plan.md). This README intentionally does not duplicate detailed milestone or release history.

## Current Limitations

- Multi-repository operations such as checkout, payment processing, and stock changes are not yet wrapped in one shared SQLite transaction, so atomic all-or-nothing persistence still requires future hardening.
- The API's order, payment, and dashboard reads may still rely on temporary sample data and require separate repository-integration verification.
- Authentication and role-based authorization are not yet implemented.
- A frontend web interface is not yet implemented.
- Some Console workflow services still combine user interaction with business orchestration and are not yet reusable pure Core services.
- Product hard deletion requires stronger historical-reference safeguards before production use.
- Business-facing reference-number generation uses a simple strategy suitable for the current single-user learning workflow but is not designed for concurrent production use.
- Database-level duplicate-receipt protection and foreign-key enforcement can be strengthened further.
- Simulated notifications are not real email delivery.
- Payment processing records transactions internally and does not integrate with a real payment gateway.

StockFlow is a development and portfolio project, not a production-ready financial or inventory system.

## Maintaining This README

Keep this page as the project's short public entry point. Update the **Development status**, **Capabilities**, **Getting Started**, **Roadmap**, and **Current Limitations** in place whenever they materially change. Record granular implementation progress in `docs/milestone-plan.md`, feature specifications in the relevant design documents, and historical changes in `docs/release-notes.md`. Do not append a new README section for each milestone.
