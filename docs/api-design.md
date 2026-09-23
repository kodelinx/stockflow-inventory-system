# StockFlow API Design

**Document status:** Maintained  
**Last reviewed:** 2026-09-24  
**API implementation:** Partial; read endpoints exist, but not every endpoint uses live SQLite data

## 1. Purpose and Scope

This document defines the HTTP interface of `StockFlow.Api`: its routes, responsibilities, response conventions, integration status, and verification approach. It is a **living API reference**, not a milestone diary.

- **What the API must do:** [`requirements.md`](requirements.md)
- **Business rules:** [`business-rules.md`](business-rules.md)
- **Solution and dependency design:** [`architecture.md`](architecture.md)
- **How features are verified:** [`acceptance-criteria.md`](acceptance-criteria.md)
- **When work was completed:** [`milestone-plan.md`](milestone-plan.md) and [`release-notes.md`](release-notes.md)

The published OpenAPI document, when available at runtime, should be used to confirm the **exact current request and response schema**. This file describes the intended and known implementation; it does not replace inspection of the running API.

## 2. API Overview

### 2.1 Technology and entry point

| Item | Current approach |
|---|---|
| Application | `src/StockFlow.Api` |
| Framework | ASP.NET Core Web API |
| Transport | HTTP/HTTPS |
| Response representation | JSON |
| API route prefix | `/api` |
| API discovery | `/openapi/v1.json` |
| Interactive Swagger/Scalar UI | Not currently configured |
| Authentication and authorization | Planned; existing endpoints are not yet protected |

The Console and API are separate entry points. They share business models through `StockFlow.Core` and can use database implementations in `StockFlow.Infrastructure`. Console-specific services that read or write the terminal are **not** reusable API services.

### 2.2 Request processing

```text
HTTP client
   |
   v
StockFlow.Api controller
   |-- validate HTTP input and select HTTP response
   |-- use reusable business logic where available
   v
Repository (StockFlow.Infrastructure)
   |
   v
SQLite
   |
   v
JSON HTTP response
```

**Current exception:** The Product API has repository-backed reads and uses `ProductManager` for low-stock calculation. Order, Payment, and Dashboard controllers were initially implemented using typed sample data. Their live-database integration must be checked against the current code before changing their status below.

## 3. API Design Conventions

### 3.1 URLs and resources

- Use plural resource names: `/api/products`, `/api/orders`, `/api/payments`.
- Use business-facing identifiers in public routes where specified: `{productCode}`, `{orderNumber}`, `{paymentNumber}`.
- Keep internal SQLite IDs distinct from public business reference numbers.
- Prefer stable route names. A proposed route is **not** an available endpoint until implemented and tested.

### 3.2 HTTP methods

| Method | Intended use |
|---|---|
| `GET` | Read a collection or individual record |
| `POST` | Create a record or initiate a business action; design not yet finalized for current read-only API |
| `PUT` / `PATCH` | Update a record; not yet part of the confirmed current endpoint set |
| `DELETE` | Delete or deactivate as defined by business rules; not yet part of the confirmed current endpoint set |

### 3.3 Response and error conventions

| Situation | HTTP status | Expected behavior |
|---|---|---|
| Successful read | `200 OK` | Return JSON data |
| Empty collection | `200 OK` | Return an empty collection rather than `404` |
| Invalid route input | `400 Bad Request` | Return a clear, safe validation message where implemented |
| Missing individual record | `404 Not Found` | Return a clear, safe not-found message |
| Unexpected server failure | `500 Internal Server Error` | Do not expose sensitive exception details; standardized handling is planned |
| Unauthorized or forbidden action | `401` / `403` | Apply after authentication and authorization are implemented |

**Illustrative error shape only** — verify exact field names against the running controllers before treating these examples as a contractual schema:

```json
{
  "message": "Product with code P999 was not found."
}
```

```json
{
  "message": "Product code is required."
}
```

### 3.4 Validation and security

Controllers are responsible for HTTP-specific validation and responses. Reusable business rules belong in Core services when feasible; SQL belongs in repositories. Do not expose connection strings, stack traces, internal file paths, or credentials in API responses. Authentication, authorization, and complete request-body validation remain future work.

## 4. Endpoint Catalog

**Status meanings:** `Implemented` = route exists in the prior API implementation; `Repository-backed` = confirmed to use persisted data in the available project context; `Sample-backed` = route exists but was documented using temporary typed sample data; `Proposed` = not confirmed as implemented. `Verification needed` = re-test the live route and integration before release.

### 4.1 Product resources

| Method | Route | Purpose | Implementation | Data source |
|---|---|---|---|---|
| `GET` | `/api/products` | List active products | Implemented; verification needed | ProductRepository / SQLite |
| `GET` | `/api/products/{productCode}` | Find a product by code, with low-stock status where supported | Implemented; verification needed | ProductRepository + ProductManager |

**Collection:** `GET /api/products` should return `200 OK` and an empty JSON collection if there are no active products. The exact product response schema should be confirmed from the controller or generated OpenAPI document.

**Single record:** `GET /api/products/{productCode}` should return `200 OK` for a matching record, `400 Bad Request` for invalid required input where validation applies, and `404 Not Found` for an unknown code. Check the intended visibility of deactivated products before extending the route.

### 4.2 Order resources

| Method | Route | Purpose | Implementation | Data source |
|---|---|---|---|---|
| `GET` | `/api/orders` | List orders | Implemented; verification needed | Initially typed sample data |
| `GET` | `/api/orders/{orderNumber}` | Find an order by business reference | Implemented; verification needed | Initially typed sample data |

Expected responses: `200 OK` for a successful read, `200 OK` with an empty collection for a list with no entries, and `404 Not Found` for an unknown individual order. Integration with `OrderRepository` and optional inclusion of related OrderItems should be verified before marking the endpoint fully database-backed. A saved Order does not automatically include its OrderItems merely because they share `OrderId`.

### 4.3 Payment resources

| Method | Route | Purpose | Implementation | Data source |
|---|---|---|---|---|
| `GET` | `/api/payments` | List payments | Implemented; verification needed | Initially typed sample data |
| `GET` | `/api/payments/{paymentNumber}` | Find payment by business reference | Implemented; verification needed | Initially typed sample data |
| `GET` | `/api/payments/order/{orderNumber}` | Retrieve payments for an order | Proposed / optional | Future PaymentRepository integration |

Expected responses for confirmed read routes: `200 OK` for a successful read; `404 Not Found` for an unknown individual payment. Because the repository can represent more than one payment per order, any future order-payment route should explicitly specify whether it returns **a collection**, including the empty-result behavior.

### 4.4 Dashboard resource

| Method | Route | Purpose | Implementation | Data source |
|---|---|---|---|---|
| `GET` | `/api/dashboard/summary` | Return business summary metrics | Implemented; verification needed | Initially typed sample data |

The initial source document proposed `/api/dashboard`; the subsequently implemented route documented for the project is `/api/dashboard/summary`. **Do not advertise `/api/dashboard` as active unless it has been added and tested.**

Expected categories include product counts, low-stock count, order totals, completed/pending orders, payment count, and sales income. Only advertise specific response fields verified in the actual controller/OpenAPI document. Sales income should follow the business definition based on paid `AmountDue` rather than cash tendered before change.

### 4.5 Future write and management operations

Create, update, deactivate, checkout, payment processing, receipt generation, notification management, and secured admin routes are **not** part of the confirmed current API contract. Define their routes, request models, validation rules, authorization, and response schemas when that work begins. Track the requirement in `requirements.md`, not in a new milestone-specific section here.

## 5. Data Contracts and Business Boundaries

### 5.1 Identifiers and relationships

| Resource | Public reference | Internal relationship key |
|---|---|---|
| Product | `ProductCode` | `ProductId` |
| Order | `OrderNumber` | `OrderId` |
| Order item | Usually represented as part of an order | `OrderItemId`, `OrderId` |
| Payment | `PaymentNumber` | `PaymentId`, `OrderId` |
| Receipt | `ReceiptNumber` | `ReceiptId`, `OrderId`, `PaymentId` |

An API response should not assume related data is already loaded. For example, returning OrderItems with an Order requires an explicit retrieval/composition decision.

### 5.2 Business rules relevant to future write endpoints

- Validate basket availability and quantities against current persisted products before checkout.
- Save parent Orders before child OrderItems so generated `OrderId` values can be used.
- Successful payment must update the persisted payment and related order state consistently.
- Prevent issuing more than one receipt for the same payment under the current business rule.
- Stock deduction and stock movement history must remain consistent; multi-step workflows need transaction handling before production use.

Detailed rules and changes to them belong in `business-rules.md`.

## 6. Local Development and Verification

### 6.1 Run the API

From the repository root:

```powershell
dotnet build
dotnet run --project src/StockFlow.Api
```

Use the HTTPS or HTTP base address shown in the terminal. Replace `<port>` in examples with the **actual running port**; do not paste the placeholder literally. Example routes:

```text
http://localhost:<port>/openapi/v1.json
http://localhost:<port>/api/products
http://localhost:<port>/api/products/P001
http://localhost:<port>/api/products/P999
http://localhost:<port>/api/orders
http://localhost:<port>/api/payments
http://localhost:<port>/api/dashboard/summary
```

Open localhost URLs directly in the browser address bar or send requests through an API client. OpenAPI JSON is available in the configured development environment; Swagger or Scalar UI is not currently configured.

### 6.2 Manual regression matrix

| Test | Expected result | Verified on / notes |
|---|---|---|
| Product collection | `200`, JSON collection | Not reverified in this document revision |
| Existing product code | `200`, matching product | Not reverified |
| Missing product code | `404` | Not reverified |
| Invalid product code | `400` where applicable | Not reverified |
| Order collection | `200`, JSON collection | Verify current data source |
| Missing order number | `404` | Not reverified |
| Payment collection | `200`, JSON collection | Verify current data source |
| Missing payment number | `404` | Not reverified |
| Dashboard summary | `200`, expected metric structure | Verify current data source |
| OpenAPI document | Successful document response | Not reverified |

Do not mark a route verified solely because `dotnet build` succeeds. Test the running route and compare its output with the expected behavior.

### 6.3 Automated API testing

Automated API integration tests are planned. When introduced, use a separate, isolated test database and cover successful reads, empty collections, invalid identifiers, missing records, and consistency of live data across endpoints. Record definitive pass/fail evidence in the test run or acceptance criteria, not as an accumulating milestone log in this document.

## 7. Current Constraints and Open Decisions

| Area | Current constraint or decision needed |
|---|---|
| Order, payment, dashboard reads | Confirm whether the current controllers still use sample data or have been migrated to repositories |
| Product reads | Reverify routes and confirm response schema against OpenAPI |
| Write endpoints | Define request/response contracts and transaction boundaries before implementation |
| Authentication | Not implemented; design roles and protect sensitive endpoints before production deployment |
| Interactive API UI | Optional Swagger/Scalar UI not currently configured |
| Automated API tests | Planned |
| Versioning | Introduce an explicit external API versioning policy when compatibility requirements arise |
| Error schema | Standardize and document a stable error response before broad client integration |

## 8. Document Maintenance

This file has permanent sections. **Do not add a section for every milestone or release.** When a feature changes:

1. Update the relevant endpoint row, description, data source, and verification notes **in place**.
2. Update conventions or data-contract rules only if the public API contract actually changes.
3. Update `requirements.md` for requirement/status changes and `architecture.md` for structural changes.
4. Record milestone completion in `milestone-plan.md` and historical changes in `release-notes.md`.
5. If a route or response changes incompatibly, explicitly record the compatibility impact and update OpenAPI and client tests.
6. Update the review date only after checking the document against current source code or running endpoints.

**Maintenance principle:** This is the current API contract and implementation-status reference, not the history of how each endpoint was built.
