# StockFlow Inventory System

StockFlow is an inventory and sales management system for small business operations. It is being built as a portfolio project to demonstrate practical C#/.NET development, software planning, documentation, Git workflow, database preparation, and Web API development.

## Current Status

- Current version: v0.6.0 - Full Database-Backed StockFlow
- Current milestone: M39 - Complete Product Repository CRUD
- Current milestone status: In Progress
- Last updated: 2026-09-13


## Project Goal

Build StockFlow from a console-based inventory system into a business-ready application with database storage, API access, user roles, frontend dashboard, testing, and production-ready documentation.

## Main Features

Current and planned feature areas:

- Product and inventory management
- Basket and order processing
- Payment tracking
- Receipt generation
- Stock movement history
- Low-stock alerts
- Sales reports
- Notification simulation
- SQLite database preparation
- ASP.NET Core Web API
- Future authentication and user roles
- Future frontend dashboard
- Future automated tests

## Technology Stack

Current:

- C#
- .NET
- ASP.NET Core Web API
- SQLite
- JSON file persistence
- Git and GitHub
- Markdown documentation

Planned:

- Shared class libraries
- Full repository-based database persistence
- Authentication and authorization
- Frontend dashboard
- Automated tests
- Deployment preparation

## How to Run

From the project root:

```powershell
dotnet build
```

Run the console app:

```powershell
dotnet run --project src/StockFlow.Console
```

Run the API project:

```powershell
dotnet run --project src/StockFlow.Api
```

Open the API locally using the terminal port:

```text
http://localhost:<port>/openapi/v1.json
http://localhost:<port>/api/products
```

## Documentation Map

- [Project Overview](docs/project-overview.md) - business context, goals, users, scope
- [Requirements](docs/requirements.md) - what the system should do
- [Business Rules](docs/business-rules.md) - rules the system must follow
- [Architecture](docs/architecture.md) - how the solution is structured
- [Database Design](docs/database-design.md) - tables, columns, relationships
- [API Design](docs/api-design.md) - API endpoints and testing approach
- [Acceptance Criteria](docs/acceptance-criteria.md) - how features are verified
- [Milestone Plan](docs/milestone-plan.md) - version roadmap and progress tracker
- [Release Notes](docs/release-notes.md) - release history and changes

## Version Roadmap

### v0.1.0 - Console Inventory and Sales MVP

Status: Released

### v0.2.0 - Inventory Rules and Reporting

Status: Released

### v0.3.0 - Database-Ready Inventory System

Status: Released

### v0.4.0 - StockFlow Web API

Status: Released

StockFlow now includes an ASP.NET Core Web API project that exposes product, order, payment, and dashboard endpoints through HTTP.

Current API features:

- OpenAPI JSON support
- Product read endpoints
- Order read endpoints
- Payment read endpoints
- Dashboard summary endpoint
- Basic API validation and error response handling

Current limitations:

- Product endpoints use repository-backed SQLite access
- Order, payment, and dashboard endpoints currently use typed temporary sample data
- Create, update, and delete API endpoints are not yet implemented
- Authentication and authorization are not yet implemented
- Full shared architecture cleanup is planned for v0.5.0
- Full database-backed flow is planned for v0.6.0

### v0.5.0 - Shared Architecture and Full API Integration

Status: Released

StockFlow has been refactored into a cleaner layered architecture with separate projects for API, Console, Core, and Infrastructure.

Current architecture highlights:

- `StockFlow.Core` contains shared business models
- `StockFlow.Core` contains `ProductManager` for reusable product business logic
- `StockFlow.Infrastructure` contains database and repository implementation
- `DatabaseConnectionService` was moved to Infrastructure
- `ProductRepository` was moved to Infrastructure
- `StockFlow.Api` no longer references `StockFlow.Console`
- Product API uses Core business logic and Infrastructure data access

Current limitations:

- Existing console services still contain console input/output workflow
- Only ProductRepository is currently implemented
- Order, payment, receipt, and stock movement repositories are not yet implemented
- Full database-backed business flow is planned for v0.6.0

### v0.6.0 - Full Database-Backed StockFlow

Status: In Progress

### v0.7.0 - Authentication and User Roles

Status: Planned

### v0.8.0 - Frontend Web Dashboard

Status: Planned

### v0.9.0 - Testing, Error Handling, and Production Readiness

Status: Planned

### v1.0.0 - Business MVP Release

Status: Planned

