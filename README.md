# StockFlow Inventory System

StockFlow is a C#/.NET inventory and sales management application for small businesses. It supports product management, stock tracking, checkout, payment recording, receipts, reporting, and simulated notifications. The project is also a development portfolio that demonstrates an incremental transition from a console MVP to a layered, database-backed application with a Web API.

> **Development status:** v0.6.0 in progress · M47 — Repository-First Console Service Refactor · Next: M48 — Automated Regression Testing Foundation. v0.6.0 has not yet been released.

## Overview

StockFlow is intended to help small businesses keep inventory and sales records in one system. Its current development focus is finishing the repository-backed Console workflow and establishing repeatable tests before the next release.

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

These features describe implemented workflows developed to date, not a claim that every workflow has passed final v0.6.0 regression testing. See [Requirements](docs/requirements.md) and [Acceptance Criteria](docs/acceptance-criteria.md) for their tracked status and verification conditions.

## Technology and Architecture

- **Language and runtime:** C# / .NET 10
- **API:** ASP.NET Core Web API with OpenAPI document support
- **Database:** SQLite via `Microsoft.Data.Sqlite`
- **Architecture:** Separate Console, API, Core, and Infrastructure projects; repository-based database access
- **Development:** Git, GitHub, and Markdown documentation
- **Testing:** Automated regression test foundation planned for M48

```text
stockflow-inventory-system/
├── src/
│   ├── StockFlow.Console/        # Menu, user interaction, and Console workflows
│   ├── StockFlow.Api/            # HTTP endpoints and API startup
│   ├── StockFlow.Core/           # Shared models and reusable business logic
│   └── StockFlow.Infrastructure/ # SQLite setup and repositories
├── tests/                        # Automated tests as implemented
├── docs/                         # Project specifications and engineering records
└── StockFlow.sln
```

The Console's persistent business records are being consolidated around SQLite repositories. `List<BasketItem>` intentionally remains temporary session state. Some Console service and `Program.cs` cleanup is still in progress; not all API endpoints are connected to live repository-backed data.

For component boundaries and data flows, see [Architecture](docs/architecture.md) and [Database Design](docs/database-design.md).

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

### Database and test data

The application uses a local SQLite database. Database initialization occurs when the relevant application startup calls `InitializeDatabase()`; `dotnet build` alone does not create a database. Database file paths can depend on the working directory until path configuration is finalized. Development database reset is destructive and must only be used on disposable development or test data. Database files should not be committed to Git.

Automated regression testing is planned for M48. Once the test project is added, its normal command will be:

```powershell
dotnet test
```

Do not interpret this planned command as confirmation that a complete automated suite exists yet.

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
| v0.6.0 | SQLite-backed Console, repository-first cleanup, and automated regression foundation | In Progress |
| v0.7.0 | Remaining API integration, authentication, and roles | Planned |
| v0.8.0 | Frontend web dashboard | Planned |
| v0.9.0 | Expanded testing and production readiness | Planned |
| v1.0.0 | Business MVP release | Planned |

The authoritative milestone breakdown, including M47 and M48, is maintained in [Milestone Plan](docs/milestone-plan.md). This README intentionally does not include individual milestone journals or duplicate detailed release history.

## Current Limitations

- The final repository-first Console refactor and regression verification are in progress.
- The API's order, payment, and dashboard reads may still rely on temporary sample data.
- Authentication, authorization, and frontend UI are not implemented.
- Automated regression coverage is planned but not yet established.
- Transactional consistency for multi-repository operations such as checkout needs further work before production use.
- Simulated notifications are not real email delivery, and payment recording is not a payment-gateway integration.

StockFlow is a development and portfolio project, not a production-ready financial or inventory system.

## Maintaining This README

Keep this page as the project's short public entry point. Update the **Development status**, **Capabilities**, **Getting Started**, **Roadmap**, and **Current Limitations** in place whenever they materially change. Record granular implementation progress in `docs/milestone-plan.md`, feature specifications in the relevant design documents, and historical changes in `docs/release-notes.md`. Do not append a new README section for each milestone.
