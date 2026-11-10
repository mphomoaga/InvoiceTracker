# InvoiceTracker

InvoiceTracker is an enterprise-grade Invoice Management and Payment Tracking platform built using Clean Architecture, .NET 10 Web API, Angular, and Entity Framework Core.

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
- .NET 10 Web API
- Entity Framework Core
- SQL Server
- Serilog

### Frontend
-Node.js: LTS version 20.x or newer
-Angular: version 20.3.x



src/environments/environment.ts
API base URL(IIS Express): https://localhost:44358/api


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
cd src/InvoiceTracker.App/Presentation/InvoiceTracker.WebApi
dotnet restore
dotnet build
dotnet run
```

### Run the Web API with Docker

Run these commands from the repository root. The Docker build context includes the API and its Application, Domain, and Infrastructure project references.

```powershell
docker build -f src/InvoiceTracker.App/Presentation/InvoiceTracker.WebApi/Dockerfile -t invoice-tracker-api src/InvoiceTracker.App
docker run --rm --name invoice-tracker-api -p 44358:44358 -e "ConnectionStrings__InvoiceTrackerDatabase=Server=host.docker.internal,1433;Database=InvoiceTracker;User Id=YOUR_USER;Password=YOUR_PASSWORD;Encrypt=False;TrustServerCertificate=True" invoice-tracker-api
```

### WebApi: 
	Create a SQL Server database and update the connection string in `appsettings.json` of the API project.

### Frontend
```bash
open front-end folder: src/InvoiceTracker.App/Presentation/InvoiceTracker.WebUI/

	-Change the Web Api port from the following File :src/InvoiceTracker.App/Presentation/InvoiceTracker.WebUI/environments/environment.ts
	Minimum setup:

	-Install Node.js LTS 20+
	-Open a terminal in the Web UI project folder:
	-src/InvoiceTracker.App/Presentation/InvoiceTracker.WebUI
	-Run:
	-npm install
	-Start the app:
	-npm start
	-Then open:
	N.B: This project is Angular 20 and uses the Angular CLI version 20.3.42, as shown in package.json.
ng serve -o
```


