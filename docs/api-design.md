# StockFlow API Design

Last updated: 2026-09-05

## Purpose

This document describes the planned API structure for StockFlow.

The API allows clients such as browsers, frontend dashboards, mobile apps, Postman, or `.http` files to communicate with StockFlow through HTTP requests.

---

# Current API Status

- Current API version: v0.4.0 - StockFlow Web API
- Current API milestone: M25 - Product API Endpoints

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

Product endpoint:

```text
http://localhost:<port>/api/products
```

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

Purpose:

Return order records.

Status:

- M26 - Planned

Expected response:

- 200 OK with order list as JSON

### GET /api/orders/{orderNumber}

Purpose:

Return one order by order number.

Status:

- M26 - Planned

Expected responses:

- 200 OK when order exists
- 404 Not Found when order does not exist

---

## Payment Endpoints

### GET /api/payments

Purpose:

Return payment records.

Status:

- M27 - Planned

Expected response:

- 200 OK with payment list as JSON

### GET /api/payments/{paymentNumber}

Purpose:

Return one payment by payment number.

Status:

- M27 - Planned

Expected responses:

- 200 OK when payment exists
- 404 Not Found when payment does not exist

### GET /api/payments/order/{orderNumber}

Purpose:

Return payment record by order number.

Status:

- M27 - Optional / Planned

Expected responses:

- 200 OK when payment exists for order
- 404 Not Found when no payment exists for order

---

## Dashboard Endpoints

### GET /api/dashboard

Purpose:

Return business summary data.

Status:

- M28 - Planned

Expected response:

- 200 OK with dashboard summary JSON

Possible summary fields:

- Total products
- Active products
- Low-stock count
- Total orders
- Completed orders
- Pending orders
- Total payments
- Total income

---

# API Validation and Error Responses

Planned for M29.

Expected behavior:

- Return 200 OK for successful read requests.
- Return 400 Bad Request for invalid input where applicable.
- Return 404 Not Found when requested data does not exist.
- Return clear error messages.
- Avoid exposing unnecessary internal error details.

Example 404 response:

```json
{
  "message": "Product with code P999 was not found."
}
```

Example 400 response:

```json
{
  "message": "Product code is required."
}
```

---

# Current Limitations

- API endpoints may still use temporary sample data.
- API is not fully connected to SQLite yet.
- No authentication yet.
- No frontend UI yet.
- No Swagger UI or Scalar UI yet.
- No automated API tests yet.
