# StockFlow Project Overview

**Project:** StockFlow Inventory System  
**Document owner:** Project maintainer  
**Last reviewed:** 2026-09-24  
**Document status:** Living document

## 1. Executive Summary

StockFlow is an inventory and sales management system for small businesses. It brings product records, stock tracking, checkout, payments, receipts, alerts, and sales summaries into a single application.

StockFlow is also a developer portfolio project. Its staged development demonstrates practical C#/.NET engineering, software planning, layered architecture, database design, API development, testing, documentation, and Git-based delivery. The project is intended to evolve into a demonstrable business MVP; it is not currently presented as production-ready software.

## 2. Business Context and Problem

Small businesses may manage inventory and sales using notebooks, spreadsheets, or disconnected tools. This makes it harder to maintain accurate stock counts, identify products that need replenishment, preserve transaction records, and evaluate sales performance.

StockFlow addresses these needs by maintaining a consistent record of products and transactions and making the information available through operational workflows and summary views.

## 3. Objectives and Success Criteria

### 3.1 Business Objectives

- Maintain a searchable product catalog with current quantities and reorder levels.
- Support a repeatable basket-to-checkout, payment, and receipt workflow.
- Preserve historical orders, payments, receipts, and inventory movements.
- Identify low-stock products and provide useful sales and operational summaries.
- Introduce controlled user access when authentication and authorization are implemented.

### 3.2 Engineering and Portfolio Objectives

- Demonstrate maintainable separation between entry points, business logic, and persistence.
- Use SQLite-backed repositories for persistent records.
- Expose appropriate capabilities through an ASP.NET Core Web API.
- Establish repeatable automated tests for critical business flows.
- Maintain clear requirements, design documents, release notes, and version-control history.

### 3.3 Success Criteria

StockFlow's business MVP should allow a user to complete a sale—from product selection through order creation, payment, receipt issuance, and stock-history review—while preserving the resulting records across application restarts. Its important business rules should be verified by tests, and its supported features and limitations should be documented accurately.

Detailed, testable acceptance conditions belong in [acceptance-criteria.md](acceptance-criteria.md); implementation progress belongs in [milestone-plan.md](milestone-plan.md).

## 4. Intended Users and Stakeholders

These are intended personas and responsibilities, **not a statement that authentication or role-based authorization is already implemented**.

| Persona | Primary need | Typical activities |
| --- | --- | --- |
| Business owner | Visibility into operations and performance | Review inventory, low-stock alerts, orders, sales, and summaries |
| Administrator | Oversight of catalog and system configuration | Maintain products and, when implemented, manage users and permissions |
| Inventory staff | Accurate product availability | Update quantities, record stock movements, monitor reorder levels |
| Cashier | Reliable transaction processing | Prepare baskets, create orders, process payments, issue receipts |
| API or future web user | Access from another application or browser | Retrieve or manage permitted data through supported interfaces |
| Project maintainer | Deliver and demonstrate the application | Implement, test, document, and release changes |

## 5. Product Scope

### 5.1 Core Capabilities

The intended product scope comprises the following capability areas. Their detailed behavior and implementation statuses are maintained in [requirements.md](requirements.md).

| Capability | Intended outcome |
| --- | --- |
| Product and inventory management | Create, search, update, activate/deactivate, and monitor products |
| Basket and orders | Validate product selections, calculate totals, and retain completed order details |
| Payments | Record payment methods, amounts, change, and payment status |
| Receipts | Produce and retain transaction proof; support text export |
| Stock movements | Preserve stock-in, adjustment, and sale-related stock-out history |
| Alerts and notifications | Flag low stock and record simulated operational messages |
| Dashboard and reports | Summarize inventory, orders, payments, and sales performance |
| Persistence | Store business records reliably using SQLite |
| API | Provide HTTP access to supported business information and, as expanded, operations |
| User access | Introduce authentication and role-based authorization |
| Web interface | Offer browser-based business workflows and summaries |
| Quality and delivery | Validate workflows with automated testing and maintain release documentation |

### 5.2 Current Product Boundary

StockFlow currently centers on its Console application and a developing ASP.NET Core API. The Console's main persistent workflows have been connected to SQLite, while repository-first service cleanup and automated regression testing remain part of the active development plan. Some API areas may still use temporary data rather than the full database-backed business workflow.

For the authoritative current milestone and release status, see [milestone-plan.md](milestone-plan.md). Do not use this overview as a second progress tracker.

### 5.3 Outside the Committed MVP Scope

The following capabilities are possible future enhancements, not commitments for the current business MVP:

- Real payment gateway integration and real email delivery
- Barcode scanning and mobile applications
- Multi-branch inventory and supplier purchasing workflows
- Customer loyalty programs
- Cloud deployment and enterprise-scale operational capabilities

Their inclusion would require updated requirements, business rules, architecture, and milestones before implementation.

## 6. Product Direction

The intended direction is a business MVP with persistent inventory and sales records, a documented API, controlled access, a usable web dashboard, and repeatable tests. StockFlow develops incrementally so that the current application can be reviewed and validated before additional interfaces or features are introduced.

This section describes product direction only. Dates, version targets, dependencies, and milestone completion belong exclusively in [milestone-plan.md](milestone-plan.md).

## 7. Constraints and Assumptions

- **Primary audience:** Small-business operations, rather than multi-branch or enterprise-scale deployment.
- **Application evolution:** Console-first development, with API and browser access expanded separately.
- **Primary persistence:** SQLite for current local business records; temporary basket state may remain in memory.
- **Notifications:** Simulated unless an actual delivery provider is deliberately integrated.
- **Payments:** Internal payment recording; no claim of payment-gateway processing.
- **Security:** Role-based access is a product objective, not an assumption about currently protected routes.
- **Reliability:** Existing features and proposed safeguards must be distinguished until validation is complete.

Architecture decisions and their technical implications are maintained in [architecture.md](architecture.md), [database-design.md](database-design.md), and [business-rules.md](business-rules.md).

## 8. Documentation and Governance

| Document | Source of truth for |
| --- | --- |
| [requirements.md](requirements.md) | Functional and non-functional requirements and their statuses |
| [business-rules.md](business-rules.md) | Rules and constraints that workflows must enforce |
| [architecture.md](architecture.md) | System structure, dependencies, and runtime flows |
| [database-design.md](database-design.md) | Persistent entities, keys, mappings, and data integrity |
| [api-design.md](api-design.md) | Routes, contracts, conventions, and API integration status |
| [acceptance-criteria.md](acceptance-criteria.md) | Verifiable conditions for features and workflows |
| [milestone-plan.md](milestone-plan.md) | Version roadmap, milestones, and development status |
| [release-notes.md](release-notes.md) | Historical changes grouped by release |
| [../README.md](../README.md) | Public-facing introduction, setup, and navigation |

### Maintenance Rules

1. Keep these eight sections stable. Revise existing content when the business purpose, audience, scope, or direction changes.
2. Do not add milestone-by-milestone or version-by-version sections to this document.
3. Update requirements and acceptance criteria for feature-level changes; update architecture and database documents for technical changes.
4. Record development progress in the milestone plan and released changes in release notes.
5. Review this overview when a significant product or scope decision changes. Update **Last reviewed** after that review, not after every code commit.
