# InvoiceTracker

InvoiceTracker is an enterprise-grade Invoice Management and Payment Tracking platform built using Clean Architecture, .NET 9 Web API, Angular, and Entity Framework Core.

## Solution Architecture

- Domain – Core business rules and entities
- Application – Use cases, CQRS handlers, and business workflows
- Infrastructure – Data persistence and external integrations
- API – REST endpoints and presentation layer
- Angular Frontend – Web application UI
- Tests – Automated testing projects

## Folder Structure

```text
InvoiceTracker/
├── src/
│   ├── Backend/
│   │   ├── InvoiceTracker.Domain/
│   │   ├── InvoiceTracker.Application/
│   │   ├── InvoiceTracker.Infrastructure/
│   │   └── InvoiceTracker.API/
│   └── Frontend/
│       └── invoicetracker-web/
├── tests/
├── docs/
├── InvoiceTracker.sln
└── README.md
```

## Technology Stack

### Backend
- .NET 9 Web API
- Entity Framework Core
- SQL Server
- MediatR
- FluentValidation
- AutoMapper
- Serilog

### Frontend
- Angular 20+
- TypeScript
- Angular Material
- RxJS

### deployment
- Docker


## Getting Started
### Database Setup

### Database: 
	Step 1: Create new database named: InvoiceTracker.
	Step 2: Execute the SQL scripts to create the necessary tables and constraints. Scripts can be found in the `sr/InvoiceTracker.Database` folder.
### Backend
```bash
cd src/Backend/InvoiceTracker.API
dotnet restore
dotnet build
dotnet run
```
### WebApi: 
	Create a SQL Server database and update the connection string in `appsettings.json` of the API project.

### Frontend
```bash
cd src/Frontend/invoicetracker-web
npm install
ng serve
```
