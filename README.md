# InvoiceTracker

InvoiceTracker is an enterprise-grade Invoice Management and Payment Tracking platform built using Clean Architecture, .NET 9 Web API, Angular, and Entity Framework Core.

## Solution Architecture

- Domain – Core business rules, Models and entities
- Application – DTOs, Business workflows and logic
- Infrastructure – Data persistence and external integrations
- API – REST endpoints and presentation layer
- Angular Frontend – Web application UI
- Tests – Automated testing projects

## Folder Structure

```text
InvoiceTracker/
├── src/
	├── InvoiceTracker.App/
	│	│	├── InvoiceTracker.sln
	│   │   ├── InvoiceTracker.Domain/
	│   │   ├── InvoiceTracker.Application/
	│   │   ├── InvoiceTracker.Infrastructure/
	│   │   └── InvoiceTracker.Presentation/
	│	│		└── InvoiceTracker.WebApi
	│	│		└── invoicetracker-web/
	│	│
	│	│
	│	└── InvoiceTracker.Database/
	|		
	|
	├── README.md
	└── .gitignore
	└── tests/
```

## Technology Stack

### Backend
- .NET 9 Web API
- Entity Framework Core
- SQL Server
- Serilog

### Frontend
- Angular 20+
- TypeScript
- Angular Material
- RxJS

### deployment
- Run Web Api on Docker or IIS Express
- Node


## Getting Started
### Database Setup

### Database: 
	Step 1: Create new database named: InvoiceTracker.
	Step 2: Execute the SQL scripts to create the necessary tables and constraints. Scripts can be found in the `src/InvoiceTracker.Database` folder.
### Backend
```bash
cd src/InvoiceTracker/Presentation/InvoiceTracker.WebApi
dotnet restore
dotnet build
dotnet run
```

### WebApi: 
	Create a SQL Server database and update the connection string in `appsettings.json` of the API project.

### Frontend
```bash
cd	src/InvoiceTracker/Presentation/invoicetracker-web
npm install
ng serve -o
```
	-Change the Web Api port from the following File : environment.ts

