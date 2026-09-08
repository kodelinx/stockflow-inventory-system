# StockFlow Requirements

## Document Purpose

This document defines what StockFlow should do as a system.

It should answer:

- What features should StockFlow support?
- What business needs should the system solve?
- What rules should the system follow?
- What requirements are implemented, in progress, planned, or deferred?

This file should not be the main milestone tracker. Version and milestone progress are tracked in `docs/milestone-plan.md`.

## Requirement Status Labels

Use only these status labels to keep this document easy to update:

- Implemented - already working
- In Progress - currently being developed
- Planned - planned for a future milestone or version
- Deferred - moved to a later version
- Future Enhancement - useful, but not part of the current MVP plan

## Requirement ID Guide

- INV - Inventory
- ORD - Orders and basket
- PAY - Payments
- RCT - Receipts
- STK - Stock movement
- DASH - Dashboard and reporting
- NOTIF - Notifications
- USER - Users and roles
- API - Web API
- DB - Database
- NFR - Non-functional requirements

## How to Update This File

When a milestone changes, update only the affected requirement cards.

Do not repeat the full version roadmap here. Use short references only, such as:

```text
Related Milestone: M25
```

The full version roadmap belongs in `docs/milestone-plan.md`.

---

# Functional Requirements

## Inventory Management

Business need:

Small businesses need to create, view, search, update, deactivate, reactivate, and monitor products.

#### INV-001 - Add Products

- Requirement: The system should allow authorized users to add products.
- Status: Implemented
- Related Milestone: M02
- Notes: Product code should be unique. Product name should not be empty.

#### INV-002 - View Products

- Requirement: The system should allow users or API clients to view product records.
- Status: Implemented
- Related Milestone: M01, M25
- Notes: Console version is implemented. API version is currently part of M25.

#### INV-003 - Search Products

- Requirement: The system should allow users or API clients to search products by product code.
- Status: Implemented
- Related Milestone: M02, M25
- Notes: API version should return 404 when the product code does not exist.

#### INV-004 - Update Products

- Requirement: The system should allow authorized users to update product details.
- Status: Implemented
- Related Milestone: M02
- Notes: API and database-backed versions are planned for later.

#### INV-005 - Deactivate Products

- Requirement: The system should allow authorized users to deactivate products.
- Status: Implemented
- Related Milestone: M02
- Notes: Deactivation should be preferred over hard deletion when transaction history exists.

#### INV-006 - Reactivate Products

- Requirement: The system should allow authorized users to reactivate inactive products.
- Status: Implemented
- Related Milestone: v0.2.0
- Notes: Reactivated products can be used again in normal inventory operations.

#### INV-007 - Delete Products

- Requirement: The system should allow authorized users to delete products only when appropriate.
- Status: Implemented
- Related Milestone: M02
- Notes: Hard delete should be controlled by business rules.

#### INV-008 - Track Stock Quantity

- Requirement: The system should track product stock quantity.
- Status: Implemented
- Related Milestone: M02
- Notes: Full database-backed stock tracking is planned for a later version.

#### INV-009 - Identify Low-Stock Products

- Requirement: The system should identify products where stock is at or below reorder level.
- Status: Implemented
- Related Milestone: M12
- Notes: Dashboard API version is planned for M28.

---

## Basket and Order Management

Business need:

The business needs to select products for checkout and convert them into order records.

#### ORD-001 - Add Items to Basket

- Requirement: The system should allow users to add products to a basket.
- Status: Implemented
- Related Milestone: M04
- Notes: Basket data is temporary in the current console version.

#### ORD-002 - View Basket

- Requirement: The system should allow users to view basket contents.
- Status: Implemented
- Related Milestone: M04

#### ORD-003 - Remove Items from Basket

- Requirement: The system should allow users to remove items from the basket.
- Status: Implemented
- Related Milestone: M04

#### ORD-004 - Clear Basket

- Requirement: The system should allow users to clear the basket.
- Status: Implemented
- Related Milestone: M04

#### ORD-005 - Calculate Basket Total

- Requirement: The system should calculate basket totals using quantity and unit price.
- Status: Implemented
- Related Milestone: M04

#### ORD-006 - Checkout Basket

- Requirement: The system should convert a valid basket into an order.
- Status: Implemented
- Related Milestone: M05
- Notes: Checkout should not proceed when stock is insufficient.

#### ORD-007 - Create Order Records

- Requirement: The system should create order records.
- Status: Implemented
- Related Milestone: M05
- Notes: API read endpoint is planned for M26.

#### ORD-008 - Create Order Item Records

- Requirement: The system should create order item records for products inside an order.
- Status: Implemented
- Related Milestone: M05
- Notes: Order items should preserve product snapshot data.

#### ORD-009 - View Orders

- Requirement: The system should allow users or API clients to view orders.
- Status: Completed
- Related Milestone: M26

#### ORD-010 - Search Orders

- Requirement: The system should allow users or API clients to search orders by order number.
- Status: Completed
- Related Milestone: M26

---

## Payment Management

Business need:

The business needs to record customer payments and track paid or unpaid orders.

#### PAY-001 - Process Payment

- Requirement: The system should allow users to process payment for an order.
- Status: Implemented
- Related Milestone: M06

#### PAY-002 - Record Payment Method

- Requirement: The system should record payment method.
- Status: Implemented
- Related Milestone: M06

#### PAY-003 - Validate Payment Amount

- Requirement: The system should validate that amount paid is enough to cover amount due.
- Status: Implemented
- Related Milestone: M06

#### PAY-004 - Calculate Change

- Requirement: The system should calculate change when amount paid is greater than amount due.
- Status: Implemented
- Related Milestone: M06

#### PAY-005 - Track Payment Status

- Requirement: The system should track paid and unpaid orders.
- Status: Implemented
- Related Milestone: M06

#### PAY-006 - View Payment Records

- Requirement: The system should allow users or API clients to view payment records.
- Status: Planned
- Related Milestone: M27

#### PAY-007 - Search Payment Records

- Requirement: The system should allow users or API clients to search payment records by payment number or order number.
- Status: Planned
- Related Milestone: M27

---

## Receipt Management

Business need:

The business needs proof of completed transactions.

#### RCT-001 - Generate Receipt

- Requirement: The system should generate receipts after successful payment.
- Status: Implemented
- Related Milestone: M07

#### RCT-002 - Prevent Duplicate Receipts

- Requirement: The system should prevent duplicate receipt generation for the same order.
- Status: Implemented
- Related Milestone: M07

#### RCT-003 - Display Receipt Details

- Requirement: The receipt should display ordered items, quantities, prices, total amount, payment method, amount paid, and change.
- Status: Implemented
- Related Milestone: M07

#### RCT-004 - View Receipts

- Requirement: The system should allow users to view generated receipts.
- Status: Implemented
- Related Milestone: M07

#### RCT-005 - Export Receipt to Text File

- Requirement: The system should export receipts as text files.
- Status: Implemented
- Related Milestone: M13

#### RCT-006 - Export Receipt to PDF

- Requirement: The system should support receipt PDF export.
- Status: Future Enhancement
- Related Milestone: Future

---

## Stock Movement Requirements

Business need:

The business needs an audit trail of inventory quantity changes.

#### STK-001 - Record Stock In

- Requirement: The system should record stock increases.
- Status: Implemented
- Related Milestone: M11

#### STK-002 - Record Stock Adjustments

- Requirement: The system should record stock adjustments.
- Status: Implemented
- Related Milestone: M11

#### STK-003 - Record Stock Out

- Requirement: The system should record stock reductions caused by checkout.
- Status: Implemented
- Related Milestone: M11

#### STK-004 - Store Stock Before and After

- Requirement: Stock movement records should store stock before and stock after values.
- Status: Implemented
- Related Milestone: M11

#### STK-005 - Record Movement Reason

- Requirement: Stock movement records should include a reason.
- Status: Implemented
- Related Milestone: M11

#### STK-006 - View Stock Movement History

- Requirement: The system should allow users to view stock movement history.
- Status: Implemented
- Related Milestone: M11

---

## Dashboard and Reporting Requirements

Business need:

The business needs summaries to understand inventory, orders, payments, and sales performance.

#### DASH-001 - Inventory Summary

- Requirement: The system should show inventory summary data.
- Status: Implemented
- Related Milestone: M08
- Notes: API dashboard version is planned for M28.

#### DASH-002 - Order Summary

- Requirement: The system should show order summary data.
- Status: Implemented
- Related Milestone: M08, M14
- Notes: API dashboard version is planned for M28.

#### DASH-003 - Payment Summary

- Requirement: The system should show payment summary data.
- Status: Implemented
- Related Milestone: M08, M14
- Notes: API dashboard version is planned for M28.

#### DASH-004 - Income Summary

- Requirement: The system should show total sales income.
- Status: Implemented
- Related Milestone: M08, M14
- Notes: Use amount due as actual sales income.

#### DASH-005 - Low-Stock Summary

- Requirement: The system should show low-stock product summary.
- Status: Implemented
- Related Milestone: M08, M12

#### DASH-006 - Sales by Payment Method

- Requirement: The system should display sales grouped by payment method.
- Status: Implemented
- Related Milestone: M14

#### DASH-007 - Dashboard API Endpoint

- Requirement: The API should expose dashboard summary data.
- Status: Planned
- Related Milestone: M28

---

## Notification Requirements

Business need:

The system should record or notify important business events.

#### NOTIF-001 - Simulate Low-Stock Notification

- Requirement: The system should create simulated low-stock notification records.
- Status: Implemented
- Related Milestone: M15

#### NOTIF-002 - Simulate Completed Order Notification

- Requirement: The system should create simulated completed order notification records.
- Status: Implemented
- Related Milestone: M15

#### NOTIF-003 - Simulate Receipt Notification

- Requirement: The system should create simulated receipt notification records.
- Status: Implemented
- Related Milestone: M15

#### NOTIF-004 - Store Notification Details

- Requirement: The system should store notification type, recipient, subject, message, creation date, and status.
- Status: Implemented
- Related Milestone: M15

#### NOTIF-005 - View Notification History

- Requirement: The system should allow users to view notification history.
- Status: Implemented
- Related Milestone: M15

#### NOTIF-006 - Save and Load Notifications

- Requirement: The system should save and load notification records through persistence.
- Status: Implemented
- Related Milestone: M15

#### NOTIF-007 - Real Email Notification

- Requirement: The system should support real email sending in the future.
- Status: Future Enhancement
- Related Milestone: Future

---

## User Management and Role Requirements

Business need:

A business-ready system should restrict actions based on user roles.

#### USER-001 - User Accounts

- Requirement: The system should support user accounts.
- Status: Planned
- Related Milestone: v0.7.0

#### USER-002 - Login

- Requirement: The system should support user login.
- Status: Planned
- Related Milestone: v0.7.0

#### USER-003 - Role-Based Access

- Requirement: The system should support role-based access.
- Status: Planned
- Related Milestone: v0.7.0

#### USER-004 - Admin, Staff, and Cashier Roles

- Requirement: The system should support Admin, Staff, and Cashier roles.
- Status: Planned
- Related Milestone: v0.7.0

#### USER-005 - Protected Actions

- Requirement: The system should restrict sensitive actions such as product deletion and payment processing.
- Status: Planned
- Related Milestone: v0.7.0

---

# Database Requirements

#### DB-001 - Define Database Entities

- Requirement: The system should identify main business entities that need storage.
- Status: Implemented
- Related Milestone: M18

#### DB-002 - Design Database Tables

- Requirement: The system should define planned database tables.
- Status: Implemented
- Related Milestone: M19

#### DB-003 - Define Keys and Relationships

- Requirement: The system should define primary keys, foreign keys, and table relationships.
- Status: Implemented
- Related Milestone: M19

#### DB-004 - Preserve Historical Transaction Data

- Requirement: The system should preserve historical product details in order items.
- Status: Implemented
- Related Milestone: M19

#### DB-005 - SQL CRUD Scripts

- Requirement: The system should include SQL scripts for create, read, update, and delete operations.
- Status: Implemented
- Related Milestone: M20

#### DB-006 - SQLite Integration

- Requirement: The system should initialize a local SQLite database file.
- Status: Implemented
- Related Milestone: M21

#### DB-007 - Repository Pattern

- Requirement: The system should separate database access through repositories.
- Status: Started
- Related Milestone: M22

#### DB-008 - Full Database-Backed Flow

- Requirement: The system should eventually replace JSON persistence with database-backed storage.
- Status: Planned
- Related Milestone: v0.6.0

---

# API Requirements

#### API-001 - API Project Setup

- Requirement: The system should include an ASP.NET Core Web API project.
- Status: Implemented
- Related Milestone: M24

#### API-002 - OpenAPI Document

- Requirement: The API should expose an OpenAPI document for endpoint discovery.
- Status: Implemented
- Related Milestone: M24
- Notes: Current route is `/openapi/v1.json`.

#### API-003 - Product List Endpoint

- Requirement: The API should expose `GET /api/products`.
- Status: Implemented
- Related Milestone: M25

#### API-004 - Product Search Endpoint

- Requirement: The API should expose `GET /api/products/{productCode}`.
- Status: Implemented
- Related Milestone: M25

#### API-005 - Order List Endpoint

- Requirement: The API should expose `GET /api/orders`.
- Status: Completed
- Related Milestone: M26

#### API-006 - Order Search Endpoint

- Requirement: The API should expose `GET /api/orders/{orderNumber}`.
- Status: Completed
- Related Milestone: M26

#### API-007 - Payment List Endpoint

- Requirement: The API should expose `GET /api/payments`.
- Status: Planned
- Related Milestone: M27

#### API-008 - Payment Search Endpoint

- Requirement: The API should expose `GET /api/payments/{paymentNumber}`.
- Status: Planned
- Related Milestone: M27

#### API-009 - Dashboard Endpoint

- Requirement: The API should expose dashboard summary data.
- Status: Planned
- Related Milestone: M28

#### API-010 - API Validation and Error Responses

- Requirement: The API should return proper status codes and clear error messages.
- Status: Planned
- Related Milestone: M29

#### API-011 - Swagger UI or Scalar UI

- Requirement: The API may support an interactive browser testing UI.
- Status: Future Enhancement
- Related Milestone: Future

---

# Non-Functional Requirements

#### NFR-001 - Input Validation

- Requirement: The system should validate user input and prevent common invalid inputs from crashing the app.
- Status: Implemented
- Related Milestone: M03, M29

#### NFR-002 - Clear Code Structure

- Requirement: The system should separate responsibilities into appropriate folders and classes.
- Status: Ongoing
- Related Milestone: All milestones

#### NFR-003 - Persistence

- Requirement: The system should save important business data so it is not lost after closing the app.
- Status: Implemented
- Related Milestone: M09, v0.6.0

#### NFR-004 - Logging

- Requirement: The system should record informational and error log entries.
- Status: Implemented
- Related Milestone: M16

#### NFR-005 - Reliability

- Requirement: The system should handle expected errors safely.
- Status: Ongoing
- Related Milestone: M03, M13, M16, M29, v0.9.0

#### NFR-006 - Security

- Requirement: The system should protect sensitive actions and avoid storing secrets in source code.
- Status: Planned
- Related Milestone: v0.7.0

#### NFR-007 - Testing

- Requirement: The system should include automated tests.
- Status: Planned
- Related Milestone: v0.9.0

#### NFR-008 - Documentation

- Requirement: The system should be documented through README and project docs.
- Status: Ongoing
- Related Milestone: All releases

#### NFR-009 - Version Control

- Requirement: The system should use Git and GitHub with clear commits and release tags.
- Status: Ongoing
- Related Milestone: All releases

---

# Known Current Limitations

- The full app is not yet production-ready.
- The full app is not yet fully database-backed.
- JSON persistence still exists.
- API endpoints may still use temporary sample data.
- M25 should remain In Progress until API routes are fully verified.
- Authentication is not yet implemented.
- Role-based access is not yet implemented.
- Frontend UI is not yet implemented.
- Automated tests are not yet implemented.
- Real email sending is not yet implemented.
- Deployment is not yet implemented.

# Future Enhancements

- Add Swagger UI or Scalar UI for interactive API testing.
- Add full SQLite-backed API integration.
- Add authentication and authorization.
- Add frontend dashboard.
- Add automated tests.
- Add deployment configuration.
- Add real email notifications.
- Add customer management.
- Add supplier management.
- Add product categories.
- Add audit logs.
- Add advanced reports.
- Add PDF and Excel exports.
- Add barcode scanning.
