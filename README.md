# StockFlow Inventory System

StockFlow is an inventory and sales management system for small business operations. It is being built as a portfolio project to demonstrate practical C#/.NET development, software planning, documentation, Git workflow, database preparation, and Web API development.

## Current Status

- Current version: v0.4.0 - StockFlow Web API
- Current milestone: M29 - API Validation and Error Responses
- Current milestone status: In Progress
- Last updated: 2026-09-10


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

Status: In Progress

### v0.5.0 - Shared Architecture and Full API Integration

Status: Planned

### v0.6.0 - Full Database-Backed StockFlow

Status: Planned

### v0.7.0 - Authentication and User Roles

Status: Planned

### v0.8.0 - Frontend Web Dashboard

Status: Planned

### v0.9.0 - Testing, Error Handling, and Production Readiness

Status: Planned

### v1.0.0 - Business MVP Release

Status: Planned

