# StockFlow Milestone Plan

Last updated: 2026-09-22

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
- M29 - API Validation and Error Responses - Completed
- M30 - v0.4.0 Release - Completed

## v0.5.0 - Shared Architecture and API Integration

Status: Released

- M31 - Create StockFlow.Core Class Library - Completed
- M32 - Move Models to StockFlow.Core - Completed
- M33 - Service Layer Assessment - Completed
- M34 - Create StockFlow.Infrastructure Class Library - Completed
- M35 - Move Database and Repository Logic to StockFlow.Infrastructure - Completed
- M36 - Extract Product Business Service to StockFlow.Core - Completed
- M37 - Connect Product API to Core and Infrastructure - Completed
- M38 - v0.5.0 Release - Completed

## v0.6.0 - Full Database-Backed Console and Service Refactor

Status: In Progress

### Repository Foundation

- M39 - Complete Product Repository CRUD - Completed
- M40 - Add Order Repository - Completed
- M41 - Add OrderItem Repository - Completed
- M42 - Add Payment Repository - Completed
- M43 - Add Receipt Repository - Completed
- M44 - Add StockMovement Repository - Completed
- M45 - Add Notification Repository - Completed

### SQLite Migration

- M46 - Replace JSON Flow with SQLite Flow - Completed
  - M46.0 - Fix SQLite Database Initialization - Completed
  - M46.1 - Connect Product Read Flow to SQLite - Completed
  - M46.2 - Connect Product Add Flow to SQLite - Completed
  - M46.3 - Connect Product Update, Status, and Delete Flow to SQLite - Completed
  - M46.4 - Connect Order and OrderItem Saving Flow to SQLite - Completed
  - M46.5 - Connect Payment and Receipt Saving Flow to SQLite - Completed
  - M46.6 - Connect Stock Movement and Notification Saving Flow to SQLite - Completed
  - M46.7 - Disable Active JSON Persistence Flow - Completed

### Repository-First Service Refactor

- M47 - Repository-First Console Service Refactor - In Progress
  - M47.1 - Refactor InventoryService to Repository-First Flow - Completed
  - M47.2 - Refactor BasketService Product Access - Completed
  - M47.3 - Refactor Order and OrderItem Flow - Completed
  - M47.4 - Refactor Payment and Receipt Flow - Completed
  - M47.5 - Refactor StockMovementService - Planned
  - M47.6 - Refactor Alert, Dashboard, Sales Report, and Notification Services - In Progress
  - M47.7 - Clean Program.cs Temporary Lists and Legacy JSON Remnants - Planned
  - M47.8 - Add Development Database Reset and Path Diagnostics - In Progress
  - M47.9 - Manual Regression Verification - Planned

### Automated Verification

- M48 - Automated Regression Testing Foundation - Planned
  - M48.1 - Create Automated Test Project - Planned
  - M48.2 - Add Database Test Isolation and Reset Support - Planned
  - M48.3 - Add Product Repository Integration Tests - Planned
  - M48.4 - Add Order and OrderItem Integration Tests - Planned
  - M48.5 - Add Payment and Receipt Integration Tests - Planned
  - M48.6 - Add StockMovement and Notification Integration Tests - Planned
  - M48.7 - Add Full Sales Workflow Regression Test - Planned
  - M48.8 - Add Core Business Logic Unit Tests - Planned
  - M48.9 - Run Full Automated and Manual Regression Suite - Planned

- M49 - v0.6.0 Release - Planned

## v0.7.0 - Authentication, User Roles, and API Completion

Status: Planned

- M50 - Connect Remaining API Reads to Real Repositories
- M51 - User and Role Requirements
- M52 - User and Role Models and Persistence
- M53 - Login and Authentication Flow
- M54 - Role-Based Authorization Rules
- M55 - Protect Product, Order, and Payment Operations
- M56 - v0.7.0 Release

## v0.8.0 - Frontend Web Dashboard

Status: Planned

- M57 - Frontend Project Setup
- M58 - Product Management Page
- M59 - Order Management Page
- M60 - Payment Management Page
- M61 - Dashboard Summary Page
- M62 - Connect Frontend to StockFlow API
- M63 - v0.8.0 Release

## v0.9.0 - Production Readiness and Expanded Testing

Status: Planned

- M64 - Expand Unit Test Coverage
- M65 - Expand Repository Integration Test Coverage
- M66 - API Integration Tests
- M67 - Standardized Error Handling
- M68 - Structured Logging and Environment Configuration
- M69 - Seed Data and Demo Data Setup
- M70 - Deployment Preparation
- M71 - v0.9.0 Release

## v1.0.0 - Business MVP Release

Status: Planned

- M72 - Final Feature Review
- M73 - Final Bug Fixes and Cleanup
- M74 - Final Documentation Review
- M75 - Portfolio README Update
- M76 - Demo Walkthrough Preparation
- M77 - v1.0.0 Business MVP Release

---