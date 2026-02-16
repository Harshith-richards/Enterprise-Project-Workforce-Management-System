# Enterprise Project & Workforce Management System (HVAC/MEP)

A professional **ASP.NET Core MVC (.NET 8)** enterprise application designed for large HVAC/MEP organizations to manage employees, projects, assignments, attendance, and role-based executive dashboards.

## 1) Project Overview

This system provides:
- Centralized user/role control with ASP.NET Core Identity.
- Employee directory for Admin/HR operations.
- Project planning and workforce assignments.
- Engineer attendance capture and HR reporting.
- Role-based dashboards for Admin, Manager, Engineer, HR, and CEO.

## 2) Architecture Explanation

The application uses a clean beginner-to-intermediate layered architecture in a **single web project**:
- **MVC Layer**: Controllers + Razor Views.
- **Service Layer**: Business workflows and orchestration.
- **Repository Layer**: Data access abstraction over EF Core.
- **Data Layer**: DbContext and seeders.
- **Identity Layer**: Authentication/authorization, roles, claims.

Patterns used:
- MVC pattern.
- Basic Repository pattern.
- Service layer pattern.
- ViewModel-based UI contracts.
- Async/await across data and service calls.

## 3) Role Explanation

### Admin
- Create users.
- Assign roles.
- Manage all modules (projects, attendance, users).
- Access full admin dashboard.

### Manager
- Manage assigned projects.
- Assign engineers through project setup.
- View manager dashboard and deadlines.

### Engineer
- View assigned projects.
- Mark and track own attendance.
- View own milestones and attendance history.

### HR
- View all attendance reports.
- Generate monthly summaries and late reports.
- View employee directory.

### CEO
- Read-only analytics dashboard.
- No edit permissions on system modules.

## 4) Database Schema Overview

Key entities:
- `ApplicationUser` (Identity extension): FullName, Department, DateJoined.
- `Project`: project metadata, manager assignment, deadlines/status.
- `ProjectAssignment`: many-to-many bridge between Project and Engineer.
- `Milestone`: one-to-many from Project.
- `Attendance`: employee attendance entries by date.

Relationships:
- One Manager can own many Projects (`Project.ManagerId`).
- One Project can have many Engineers via `ProjectAssignment`.
- One Project can have many Milestones.
- One Employee can have many Attendance records.

## 5) Step-by-step Setup Guide

### Prerequisites
1. Install **.NET 8 SDK**.
2. Install **SQL Server Express**.
3. Install **SQL Server Management Studio** (recommended).

### Clone Repository
```bash
git clone <your-repo-url>
cd Enterprise-Project-Workforce-Management-System
```

### Configure SQL Server Express
Update `appsettings.json` if needed:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=EnterpriseWorkforceDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

### Run migrations
```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> Note: The app also executes migrations automatically at startup through `DbSeeder`.

### Run project
```bash
dotnet restore
dotnet run
```
Browse to:
- `https://localhost:xxxx`
- `http://localhost:xxxx`

## 6) Default Login Credentials

> Password for seeded users: `P@ssw0rd123!`

- Admin: `admin@hvaccorp.com`
- Manager: `manager@hvaccorp.com`
- Engineer: `engineer@hvaccorp.com`
- HR: `hr@hvaccorp.com`
- CEO: `ceo@hvaccorp.com`

## 7) Folder Structure Explanation

```text
Controllers/      -> MVC request handlers
Models/           -> Domain entities + enums
Data/             -> DbContext + database seed logic
Repositories/     -> Generic and specialized data access
Services/         -> Business logic/orchestration
ViewModels/       -> UI-specific DTOs
Views/            -> Razor pages (role dashboards and modules)
wwwroot/          -> Static files (CSS)
```

## 8) Explanation of Relationships

- **One-to-Many**:
  - Project -> Milestones.
  - User -> Attendance records.
- **Many-to-Many**:
  - Project <-> Engineers via `ProjectAssignments`.
- **Role Mapping**:
  - Identity roles seeded at startup and assigned to seed users.

## 9) How Dashboards Work

- **Admin Dashboard**:
  - Aggregates employee count, project counts, delays, deadlines.
  - Attendance summary pulled from attendance service.
  - Charts rendered via Chart.js (Pie + Bar).
- **Manager Dashboard**:
  - Displays manager-specific projects and deadlines.
- **Engineer Dashboard**:
  - Displays assigned projects and next pending milestone.
- **HR Dashboard**:
  - Displays monthly attendance and late-by-employee metrics.
- **CEO Dashboard**:
  - High-level read-only KPIs.

## 10) Future Improvements

- Add task-level project management and progress workflows.
- Add export to Excel/PDF for HR reports.
- Add notifications for deadline risks and absence patterns.
- Add audit log and approval trails.
- Add unit/integration test suite and CI pipeline.
- Add tenant support for multi-branch organizations.

---

## Important operational notes

- This repository is intentionally a **single-project enterprise MVC app**.
- SQL Server Express is the target DB engine.
- EF Core Code First is used for schema evolution.
