# StockFlow API Design

Last updated: 2026-09-05

## Purpose

This document describes the planned API structure for StockFlow.

The API allows clients such as browsers, frontend dashboards, mobile apps, Postman, or `.http` files to communicate with StockFlow through HTTP requests.

---

# Current API Status

- Current API version: v0.4.0 - StockFlow Web API
- Current API milestone: M30 - v0.4.0 Release
- Current API release status: Released

---

# Local API Testing

Run the API:

```powershell
dotnet run --project src/StockFlow.Api
```

Open the API using the port shown in the terminal.

OpenAPI document:

```text
http://localhost:<port>/openapi/v1.json
```

Product endpoints:

http://localhost:<port>/api/products
http://localhost:<port>/api/products/P001
http://localhost:<port>/api/products/P999

Order endpoints:

http://localhost:<port>/api/orders
http://localhost:<port>/api/orders/ORD-001
http://localhost:<port>/api/orders/ORD-999

Payment endpoints:

http://localhost:<port>/api/payments
http://localhost:<port>/api/payments/PAY-001
http://localhost:<port>/api/payments/PAY-999

Dashboard endpoint:

http://localhost:<port>/api/dashboard/summary

Important:

- Do not search localhost in Google.
- Paste the localhost URL directly into the browser address bar.
- Current project uses OpenAPI JSON.
- Swagger UI is not configured yet.

---

# API Principles

- APIs should return JSON.
- Endpoints should use predictable route names.
- Controllers should handle HTTP requests and responses.
- Controllers should not contain direct SQL logic.
- Business logic should eventually live in services.
- Database access should eventually live in repositories.
- API responses should use proper HTTP status codes.

---

# Planned API Endpoints

## Product Endpoints

### GET /api/products

Purpose:Returns the active product list as JSON.

Controller: `ProductsController`

Repository method: `ProductRepository.GetActiveProducts()`

Expected response: `200 OK`

Example URL: `/api/products`

Current behavior:

- Returns active products only.
- Retrieves product records through `ProductRepository`.

---

### GET /api/products/{productCode}

Status: Implemented

Purpose:Returns one product by product code.

Controller: `ProductsController`

Repository method: `ProductRepository.FindProductByCode(productCode)`

Expected responses:

- `200 OK` if the product exists
- `404 Not Found` if the product does not exist

Example test URLs:

- `/api/products/P001`
- `/api/products/P999`

Current behavior:

- Searches product records by product code.
- Returns a clear not-found response when no matching product exists.

---

## Order Endpoints

### GET /api/orders

Status: Implemented

Purpose:Returns the order list as JSON.

Controller: `OrdersController`

Expected response: `200 OK`

Example URL: `/api/orders`

Current behavior:

- Returns typed temporary sample order data.
- Used for API route and response testing.

Current limitation: Not yet connected to SQLite or `OrderRepository`.

---

### GET /api/orders/{orderNumber}

Status: Implemented

Purpose: Returns one order by order number.

Controller:`OrdersController`

Expected responses:

- `200 OK` if the order exists
- `404 Not Found` if the order does not exist

Example test URLs:

- `/api/orders/ORD-001`
- `/api/orders/ORD-999`

Current behavior:

- Searches temporary sample orders by order number.
- Uses case-insensitive comparison.
- Returns a clear not-found response when no matching order exists.

Current limitation: Not yet connected to SQLite or `OrderRepository`.

---

## Payment Endpoints

### GET /api/payments

Status: Implemented

Purpose:

Returns payment records as JSON.

Controller: `PaymentsController`

Expected response: `200 OK`

Example URL: `/api/payments`

Current behavior:

- Returns typed temporary sample payment data.
- Used for API route and response testing.

Current limitation:

- Not yet connected to SQLite or `PaymentRepository`.

---

### GET /api/payments/{paymentNumber}

Status: Implemented

Purpose:

Returns one payment by payment number.

Controller: `PaymentsController`

Expected responses:

- `200 OK` if the payment exists
- `404 Not Found` if the payment does not exist

Example test URLs:

- `/api/payments/PAY-001`
- `/api/payments/PAY-999`

Current behavior:

- Searches temporary sample payments by payment number.
- Uses case-insensitive comparison.
- Returns a clear not-found response when no matching payment exists.

Current limitation:

- Not yet connected to SQLite or `PaymentRepository`.

---

## Dashboard Endpoints

### GET /api/dashboard/summary

Status: Implemented

Purpose:

Returns dashboard summary data as JSON.

Controller: `DashboardController`

Expected response:

- `200 OK`

Example URL:

`/api/dashboard/summary`

Current behavior:

- Returns total products
- Returns low-stock product count
- Returns total orders
- Returns completed order count
- Returns pending order count
- Returns total payments
- Returns total sales income

Current limitation:

- Uses typed temporary sample dashboard data
- Not yet connected to real product, order, payment, or dashboard services

---

# API Validation and Error Responses

Status: In Progress

Related Milestone: M29

Expected behavior:

- Return `200 OK` for successful read requests.
- Return `400 Bad Request` for invalid input where applicable.
- Return `404 Not Found` when requested data does not exist.
- Return clear error messages.
- Avoid exposing unnecessary internal error details.

Current response rules:

- List endpoints should return `200 OK`, even when the list is empty.
- Search endpoints should return `404 Not Found` when the record does not exist.
- Route parameters should be validated before searching.
- Empty list responses are not errors.

Examples:

- `GET /api/products` returns `200 OK`
- `GET /api/products/P999` returns `404 Not Found`
- `GET /api/orders/ORD-999` returns `404 Not Found`
- `GET /api/payments/PAY-999` returns `404 Not Found`
