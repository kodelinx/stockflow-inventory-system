# StockFlow Milestone Plan

Last updated: 2026-09-05

## Purpose

This document is the source of truth for StockFlow version and milestone tracking.

Requirements are documented in `requirements.md`.  
Changes after release are documented in `release-notes.md`.

---

# Version Roadmap

## v0.1.0 - Console Inventory and Sales MVP

Status: Released

- M00 - Project Initialization - Completed
- M01 - Product Model and Inventory Basics - Completed
- M02 - Inventory CRUD Operations - Completed
- M03 - Service Structure and Input Validation - Completed
- M04 - Basket Management - Completed
- M05 - Checkout and Order Creation - Completed
- M06 - Payment Processing - Completed
- M07 - Receipt Generation - Completed
- M08 - Dashboard Summary - Completed
- M09 - JSON Persistence - Completed
- M10 - v0.1.0 Release - Completed

## v0.2.0 - Inventory Rules and Reporting

Status: Released

- M11 - Stock Movement Tracking - Completed
- M12 - Low Stock Alerts - Completed
- M13 - Receipt File Export - Completed
- M14 - Sales Summary Reports - Completed
- M15 - Email Notification Simulation - Completed
- M16 - Error Handling and Logging Preparation - Completed
- M17 - v0.2.0 Release - Completed

## v0.3.0 - Database-Ready Inventory System

Status: Released

- M18 - Database Requirements - Completed
- M19 - Database Table Design - Completed
- M20 - SQL CRUD Scripts - Completed
- M21 - SQLite Integration - Completed
- M22 - Repository Pattern Introduction - Completed
- M23 - v0.3.0 Release - Completed

## v0.4.0 - StockFlow Web API

Status: Released

- M24 - ASP.NET Core Web API Setup - Completed
- M25 - Product API Endpoints - Completed
- M26 - Order API Endpoints - Completed
- M27 - Payment API Endpoints - Completed
- M28 - Dashboard API Endpoints - Completed
- M29 - API Validation and Error Responses - Competed
- M30 - v0.4.0 Release - Completed

## v0.5.0 - Shared Architecture and Full API Integration

Status: In Progress

- M31 - Create StockFlow.Core Class Library - Completed
- M32 - Move Models to StockFlow.Core - Completed
- M33 - Service Layer Assessment - Completed
- M34 - Create StockFlow.Infrastructure Class Library - Completed
- M35 - Move Database and Repository Logic to StockFlow.Infrastructure - Completed
- M36 - Extract Pure Business Service to StockFlow.Core - Completed
- M37 - Connect API Endpoints to Real Service and Repositories - Competed
- M38 - v0.5.0 Release - Competed

## v0.6.0 - Full Database-Backed StockFlow

Status: Planned

- M39 - Complete Product Repository CRUD - Planned
- M40 - Add Order Repository - Planned
- M41 - Add OrderItem Repository - Planned
- M42 - Add Payment Repository - Planned
- M43 - Add Receipt Repository - Planned
- M44 - Add StockMovement Repository - Planned
- M45 - Add Notification Repository - Planned
- M46 - Replace JSON Flow with SQLite Flow - Planned
- M47 - v0.6.0 Release - Planned

## v0.7.0 - Authentication and User Roles

Status: Planned

- M48 - User and Role Requirements
- M49 - User Model and Role Model
- M50 - Login Endpoint
- M51 - Role-Based Authorization Rules
- M52 - Protect Product, Order, and Payment Endpoints
- M53 - v0.7.0 Release

## v0.8.0 - Frontend Web Dashboard

Status: Planned

- M54 - Frontend Project Setup
- M55 - Product Management Page
- M56 - Order Management Page
- M57 - Payment Management Page
- M58 - Dashboard Summary Page
- M59 - Connect Frontend to StockFlow API
- M60 - v0.8.0 Release

## v0.9.0 - Testing, Error Handling, and Production Readiness

Status: Planned

- M61 - Unit Test Project Setup
- M62 - Service Unit Tests
- M63 - Repository Tests
- M64 - API Integration Tests
- M65 - Standardized Error Handling
- M66 - Logging and Environment Configuration
- M67 - Seed Data and Demo Data Setup
- M68 - v0.9.0 Release

## v1.0.0 - Business MVP Release

Status: Planned

- M69 - Final Feature Review
- M70 - Final Bug Fixes and Cleanup
- M71 - Final Documentation Update
- M72 - Portfolio README Update
- M73 - Demo Walkthrough Preparation
- M74 - v1.0.0 Business MVP Release

---

# Milestone Update Rules

For every milestone:

1. Complete the code work.
2. Test the feature.
3. Update the affected documentation.
4. Commit code changes.
5. Commit documentation changes.
6. Update this milestone plan.
7. Update release notes if the milestone belongs to an active version.
8. Tag the release only at release milestones.

# Release Tag Rules

Use Git tags for completed release milestones:

```text
v0.1.0
v0.2.0
v0.3.0
v0.4.0
```

Do not create a tag for every milestone. Tags are for release checkpoints.
