# StockFlow Release Notes

Last updated: 2026-09-05

## Purpose

This document records what changed in each released version of StockFlow.

Milestone tracking belongs in `milestone-plan.md`.  
Requirements belong in `requirements.md`.

---

# v0.1.0 - Console Inventory and Sales MVP

Release Date: 2026-08-23

## Summary

StockFlow v0.1.0 is the first working console-based MVP of the inventory and sales management system.

This release includes inventory management, basket management, checkout, payment processing, receipt generation, dashboard summaries, and JSON file persistence.

## Completed Features

- Product model and inventory listing
- Product CRUD operations
- Product search by name or product code
- Product update
- Product soft delete through deactivation
- Product hard delete for cleanup/testing
- Input validation service
- Basket item management
- Basket total calculation
- Checkout and order creation
- Order item records
- Payment processing
- Payment method selection
- Change calculation
- Receipt generation
- Dashboard summary
- JSON save and load support

## Technical Improvements

- Separated models, services, utilities, and data storage
- Used service classes for business operations
- Used JSON serialization and deserialization
- Added reusable generic storage methods
- Added validation for common invalid inputs
- Added basic error handling for file operations

## Known Limitations

- Console application only
- Data is stored in local JSON files
- No database yet
- No user login or role-based access yet
- No automated tests yet
- No web API yet
- No receipt export to PDF/text yet
- ID generation is still based on list counts
- No date-based reports yet

---

# v0.2.0 - Inventory Rules and Reporting

Release Date: 2026-08-29

## Summary

StockFlow v0.2.0 improves the console MVP by adding inventory traceability, low-stock alerting, receipt export, sales reporting, notification simulation, and basic logging preparation.

## Completed Features

- Stock movement tracking
- Stock-in records
- Stock adjustment records
- Automatic stock-out records during checkout
- Low-stock alert viewing
- Receipt text file export
- Sales summary reports
- Sales by payment method
- Completed order details report
- Simulated low-stock email notifications
- Simulated order completed email notifications
- Simulated receipt email notifications
- Notification history
- Notification JSON persistence
- Basic logging service
- JSON save/load event logging
- JSON save/load error logging

## Technical Improvements

- Added StockMovement model
- Added Notification model
- Added StockMovementService
- Added AlertService
- Added SalesReportService
- Added NotificationService
- Added LoggingService
- Improved separation of concerns
- Added basic application log file output
- Improved troubleshooting support for JSON storage

## Known Limitations

- Console application only
- Local JSON file storage only
- No database yet
- No real email sending yet
- No PDF receipt export yet
- No authentication or user roles yet
- No automated tests yet
- No web API yet
- No advanced reporting filters yet
- No structured logging or log rotation yet

---

# v0.3.0 - Database-Ready Inventory System

Release Date: 2026-09-02

## Summary

StockFlow v0.3.0 prepares the application for database-backed storage. This version adds database requirements, database table design, SQL CRUD scripts, SQLite initialization, and the first repository class for product database access.

## Completed Features

- Defined database requirements
- Designed planned database tables
- Created SQL CRUD scripts
- Added SQLite package support
- Created DatabaseConnectionService
- Added SQLite database initialization
- Added automatic Products table creation
- Created ProductRepository
- Added product insert database method
- Added product read database methods
- Added product search by product code
- Added SQL parameters for safer database commands
- Added SQLite database file ignore rules

## Technical Improvements

- Introduced database-ready project direction
- Introduced repository pattern
- Separated database connection setup
- Separated product database access logic
- Prepared future repository-based storage
- Preserved JSON storage temporarily during migration

## Known Limitations

- The full app is not yet database-backed.
- Product menu operations are not fully using SQLite yet.
- JSON persistence still exists.
- Only Products table is initialized from C#.
- Only ProductRepository has been started.
- Other repositories are not implemented yet.
- No Web API yet.
- No automated tests yet.

---

# v0.4.0 - StockFlow Web API

Release Date: 2026-09-11

## Summary

StockFlow v0.4.0 introduces ASP.NET Core Web API support. The goal is to expose core StockFlow features through HTTP endpoints.

## Progress

### M24 - ASP.NET Core Web API Setup

Status: Completed

Completed:

- Created StockFlow.Api project
- Added API project to the StockFlow solution
- Confirmed API project builds
- Confirmed API project runs locally
- Tested sample `/weatherforecast` endpoint
- Tested OpenAPI document access through `/openapi/v1.json`
- Reviewed basic ASP.NET Core Web API startup flow

Known limitations:

- Swagger UI is not configured yet.
- Custom StockFlow endpoints were not part of M24.

### M25 - Product API Endpoints

Status: Completed

Completed:

- Created `ProductsController`
- Added `GET /api/products`
- Added `GET /api/products/{productCode}`
- Connected product endpoints to `ProductRepository`
- Retrieved active products through repository-based data access
- Returned product data as JSON
- Added `404 Not Found` response for missing product codes
- Confirmed product API routes work locally

Known limitations:

- Product API currently supports read operations only
- Create, update, delete, and deactivate product endpoints are not yet implemented
- API authentication and authorization are not yet implemented
- Shared architecture cleanup is planned for a future version

### M26 - Order API Endpoints

Status: Completed

Completed:

- Created `OrdersController`
- Added `GET /api/orders`
- Added `GET /api/orders/{orderNumber}`
- Returned order data as JSON
- Added `404 Not Found` response for missing order numbers
- Used typed temporary sample order data for API testing
- Confirmed order API routes work locally

Known limitations:

- Order API currently supports read operations only
- Order endpoints use temporary sample data
- Order endpoints are not yet connected to SQLite
- Create, update, and checkout order API endpoints are not yet implemented
- API authentication and authorization are not yet implemented

## Expected v0.4.0 Outcome

By the end of v0.4.0, StockFlow should have:

- Working API project
- Product API endpoints
- Order API endpoints
- Payment API endpoints
- Dashboard API endpoints
- Basic API validation
- Basic API error responses
- OpenAPI document access

## Expected v0.4.0 Limitations

- API may still use temporary sample data.
- Full SQLite-backed API integration may not be complete.
- Console app may still contain the main business flow.
- Authentication is not yet implemented.
- Frontend UI is not yet implemented.
- Swagger UI or Scalar UI is not yet configured unless added later.

### M27 - Payment API Endpoints

Status: Completed

Completed:

- Created PaymentsController
- Added GET /api/payments
- Added GET /api/payments/{paymentNumber}
- Returned payment data as JSON
- Added 404 Not Found response for missing payment numbers
- Used typed temporary sample payment data for API testing

Pending verification:

- Confirm GET /api/payments returns payment records
- Confirm GET /api/payments/PAY-001 returns one payment
- Confirm GET /api/payments/PAY-999 returns 404 Not Found

Known limitations:

- Payment API currently supports read operations only
- Payment endpoints use temporary sample data
- Payment endpoints are not yet connected to SQLite
- Payment processing through API is not yet implemented
- API authentication and authorization are not yet implemented

### M28 - Dashboard API Endpoints

Status: Completed

Completed:

- Created DashboardController
- Added GET /api/dashboard/summary
- Returned dashboard summary data as JSON
- Used typed temporary sample dashboard data for API testing

Pending verification:

- Confirm GET /api/dashboard/summary returns dashboard summary data
- Confirm response includes inventory summary values
- Confirm response includes order summary values
- Confirm response includes payment and income summary values

Known limitations:

- Dashboard API currently supports read-only summary data
- Dashboard endpoint uses temporary sample data
- Dashboard endpoint is not yet connected to DashboardService
- Dashboard endpoint is not yet connected to SQLite
- API authentication and authorization are not yet implemented

### M29 - API Validation and Error Responses

Status: Completed

Completed:

- Added basic route parameter validation
- Added `400 Bad Request` response for invalid required values
- Confirmed `404 Not Found` behavior for missing products
- Confirmed `404 Not Found` behavior for missing orders
- Confirmed `404 Not Found` behavior for missing payments
- Clarified that empty list responses should return `200 OK`

Pending verification:

- Confirm product search response behavior
- Confirm order search response behavior
- Confirm payment search response behavior
- Confirm dashboard summary still returns `200 OK`

Known limitations:

- Validation is still basic
- Full request body validation is not yet implemented
- Create, update, and delete endpoints are not yet implemented
- Authentication and authorization are not yet implemented

## v0.4.0 - StockFlow Web API

Release date: 2026-09-10

Status: Released

### Summary

v0.4.0 introduced the ASP.NET Core Web API layer for StockFlow.

This release added API endpoints for products, orders, payments, and dashboard summary data. It also introduced basic API response handling using standard HTTP responses such as 200 OK, 400 Bad Request, and 404 Not Found.

### Completed Milestones

- M24 - ASP.NET Core Web API Setup
- M25 - Product API Endpoints
- M26 - Order API Endpoints
- M27 - Payment API Endpoints
- M28 - Dashboard API Endpoints
- M29 - API Validation and Error Responses
- M30 - v0.4.0 Release

### Added

- Added `StockFlow.Api` ASP.NET Core Web API project
- Added OpenAPI JSON support through `/openapi/v1.json`
- Added `ProductsController`
- Added `OrdersController`
- Added `PaymentsController`
- Added `DashboardController`
- Added `GET /api/products`
- Added `GET /api/products/{productCode}`
- Added `GET /api/orders`
- Added `GET /api/orders/{orderNumber}`
- Added `GET /api/payments`
- Added `GET /api/payments/{paymentNumber}`
- Added `GET /api/dashboard/summary`

### Improved

- Added basic route parameter validation
- Added clearer API error responses
- Used `200 OK` for successful requests
- Used `400 Bad Request` for invalid input where applicable
- Used `404 Not Found` for missing records
- Clarified that empty list responses are successful responses

### Current Limitations

- The API is not production-ready yet
- Product endpoints use repository-backed SQLite access
- Order endpoints use typed temporary sample data
- Payment endpoints use typed temporary sample data
- Dashboard endpoint uses typed temporary sample data
- Create, update, and delete API endpoints are not yet implemented
- Authentication and authorization are not yet implemented
- Full shared architecture cleanup is planned for v0.5.0
- Full database-backed flow is planned for v0.6.0

## v0.5.0 - Shared Architecture and Full API Integration

Status: In Progress

### M31 - Create StockFlow.Core Class Library

Status: Completed

Completed:

- Created `StockFlow.Core` class library project
- Added `StockFlow.Core` to the solution
- Added project reference from `StockFlow.Console` to `StockFlow.Core`
- Added project reference from `StockFlow.Api` to `StockFlow.Core`
- Created initial Core folders for Models, Services, and Interfaces

Known limitations:

- Shared models have not been moved yet
- Shared services have not been moved yet
- Repository and database logic are still outside Infrastructure