# StockFlow Architecture

## Purpose

This document explains how StockFlow is structured and how the main parts of the system work together.

Requirements explain what the system should do. Architecture explains how the system is organized.

---

# Current Solution Structure

StockFlow currently uses a staged architecture.

```text
stockflow-inventory-system/
├── README.md
├── docs/
├── src/
│   ├── StockFlow.Console/
│   ├── StockFlow.Api/
│   ├── StockFlow.Core/
│   └── StockFlow.Infrastructure/
├── tests/
└── StockFlow.sln
```

## Current Projects

### StockFlow.Console

The console project contains the original working application flow.

Responsibilities:

- Console menu
- User input
- Inventory actions
- Basket and checkout flow
- Payment processing
- Receipt generation
- Dashboard summaries
- JSON persistence
- Basic logging
- SQLite preparation
- Initial repository work

### StockFlow.Api

The API project is the new Web API entry point.

Responsibilities:

- HTTP endpoints
- API request/response handling
- OpenAPI document generation
- Future product, order, payment, and dashboard endpoints

### StockFlow.Core

The Core project contains shared business code that can be used by both the console app and the API.

Current responsibilities:

- Shared business models
- Future pure business services
- Future service interfaces

Current contents:

- Product
- BasketItem
- Order
- OrderItem
- Payment
- Receipt
- StockMovement
- Notification

Current status:

- StockFlow.Core was created in M31
- Shared models were moved to StockFlow.Core in M32
- Current services were not moved because they depend heavily on console input/output

### StockFlow.Infrastructure

The Infrastructure project contains technical implementation details.

Current responsibilities:

- Database connection setup
- SQLite integration
- Repository classes
- Future repository implementations

Current contents:

- DatabaseConnectionService
- ProductRepository

Current status:

- StockFlow.Infrastructure was created in M34
- DatabaseConnectionService was moved to StockFlow.Infrastructure
- ProductRepository was moved to StockFlow.Infrastructure
- StockFlow.Infrastructure references StockFlow.Core
- StockFlow.Api and StockFlow.Console reference StockFlow.Infrastructure

---

# Current Console Architecture

```text
User
    ↓
Program.cs
    ↓
Services
    ↓
Models
    ↓
JsonStorageService
    ↓
Local JSON files
```

## Console Folder Responsibilities

### Models

Contains business data classes.

Examples:

- Product
- BasketItem
- Order
- OrderItem
- Payment
- Receipt
- StockMovement
- Notification

### Services

Contains business actions.

Examples:

- InventoryService
- BasketService
- OrderService
- PaymentService
- ReceiptService
- DashboardService
- StockMovementService
- SalesReportService
- NotificationService
- AlertService

### Data

Contains storage-related services.

Example:

- JsonStorageService

### Database

Contains database setup and initialization logic.

Example:

- DatabaseConnectionService

### Repositories

Contains database access classes.

Example:

- ProductRepository

### Utilities

Contains reusable helper classes.

Examples:

- InputValidationService
- LoggingService

---

# Current API Architecture

Browser / API Client
    ↓
StockFlow.Api
    ↓
Controllers
    ↓
Data Source
    ↓
JSON Response

Current API data sources:

- ProductsController uses ProductRepository and SQLite.
- OrdersController uses typed temporary sample order data.
- PaymentsController uses typed temporary sample payment data.
- DashboardController uses typed temporary sample dashboard data.

Current API project:

```text
StockFlow.Api/
├── Controllers/
│   ├── ProductsController.cs
│   ├── OrdersController.cs
│   ├── PaymentsController.cs
│   └── DashboardController.cs
├── Program.cs
├── appsettings.json
└── StockFlow.Api.csproj
```

Current API notes:

- The API project was introduced in v0.4.0.
- OpenAPI document is available through `/openapi/v1.json`.
- Swagger UI is not currently configured.
- Product API endpoints were added in M25.
- Order API endpoints were added in M26.
- Payment API endpoints were added in M27.
- Dashboard summary endpoint was added in M28.
- Basic API validation and error response handling is being improved in M29.

## Current API Controller Flows

### Product API Flow

HTTP GET Request
    ↓
ProductsController
    ↓
ProductRepository
    ↓
SQLite Database
    ↓
HTTP JSON Response

### Order API Flow

HTTP GET Request
    ↓
OrdersController
    ↓
Typed temporary sample order data
    ↓
HTTP JSON Response

### Payment API Flow

HTTP GET Request
    ↓
PaymentsController
    ↓
Typed temporary sample payment data
    ↓
HTTP JSON Response

### Dashboard API Flow

HTTP GET Request
    ↓
DashboardController
    ↓
Typed temporary sample dashboard summary data
    ↓
HTTP JSON Response

---

# Current Database Preparation Architecture

```text
Program.cs
    ↓
DatabaseConnectionService
    ↓
SQLite database file
    ↓
Products table
```

Current database notes:

- SQLite has been added.
- DatabaseConnectionService owns the connection string and initialization logic.
- Products table can be created from C#.
- ProductRepository has been started.
- Most app flows still use lists and JSON persistence for now.

---

# Architecture Principles

## Separation of Concerns

Each part of the system should have a clear responsibility.

- Models represent data.
- Services perform business actions.
- Repositories handle database access.
- Data services handle file-based persistence.
- Utilities provide reusable helper logic.
- Controllers handle API requests and responses.
- Program.cs coordinates startup and app flow.

## Gradual Refactoring

StockFlow is intentionally built in stages.

The project starts with a working console system before being refactored into a more professional architecture.

This allows the project to demonstrate:

- Feature development
- Refactoring
- Database migration
- API development
- Better layering over time

---

# Target Future Architecture

A later version should move toward this structure:

```text
src/
├── StockFlow.Api/
├── StockFlow.Console/
├── StockFlow.Core/
└── StockFlow.Infrastructure/
```

## Planned Project Responsibilities

### StockFlow.Core

Will contain shared business models and business service contracts.

Possible contents:

- Models
- Business rules
- Service interfaces
- DTOs if needed

### StockFlow.Infrastructure

Will contain technical implementation details.

Possible contents:

- Repositories
- Database connection
- SQLite implementation
- Logging implementation
- External integrations

### StockFlow.Api

Will contain Web API-specific code.

Possible contents:

- Controllers
- API request/response models
- API validation
- API startup configuration

### StockFlow.Console

Will contain console-specific code.

Possible contents:

- Console menu
- Console input and output
- Console-only workflow

---

# Target Future Flow

```text
Frontend / API Client
    ↓
StockFlow.Api
    ↓
Services
    ↓
Repositories
    ↓
SQLite Database
```

Console target flow:

```text
Console User
    ↓
StockFlow.Console
    ↓
Services
    ↓
Repositories
    ↓
SQLite Database
```

Shared logic target:

```text
StockFlow.Api      StockFlow.Console
       ↓                 ↓
          StockFlow.Core
                ↓
      StockFlow.Infrastructure
                ↓
           SQLite Database
```

---

# Current Known Architecture Limitations

- The console app still contains the main working business flow.
- The API project has started repository integration for product read endpoints, but it is not fully integrated with all business services yet.
- JSON persistence still exists.
- SQLite integration has started but is not yet the main storage flow.
- ProductRepository is currently used by the Product API endpoints, but other repositories are not implemented yet.
- Other repositories are not implemented yet.
- No shared Core or Infrastructure class library exists yet.
- No automated tests yet.
- Authentication and authorization are not implemented yet.

# Architecture Improvement Plan

- v0.4.0 - Introduce Web API endpoints.
- v0.5.0 - Create shared Core and Infrastructure projects.
- v0.6.0 - Make SQLite the main storage system.
- v0.7.0 - Add authentication and user roles.
- v0.8.0 - Add frontend dashboard.
- v0.9.0 - Add automated tests and production-readiness improvements.
