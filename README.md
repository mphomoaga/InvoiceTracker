# InvoiceTracker

InvoiceTracker is an Invoice Management and Payment Tracking platform built using Clean Architecture, .NET 10 Web API, Angular, Entity Framework Core, PostgreSQL DB and docker.

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
	│	│		├── InvoiceTracker.WebApi
	│	│		├── InvoiceTracker.Worker/
	│	│		└── invoicetracker-web/
	│	│
	│	│
	│	└── InvoiceTracker.Database/
	|		
	|
	├── README.md
	└── .gitignore
	
```

## Technology Stack

### Backend
- .NET 10 Web API
- Entity Framework Core
- PostgreSQL databse
- Serilog

### Frontend
-Node.js: LTS version 20.x or newer
-Angular: version 20.3.x



### deployment
- Run Web Api on Docker 
- Node

## Getting Started
### Database Setup
	Step 1: Install docker desktop from the docker offical website
	Step 2: run the docker compaose file that will download postgreSQL into docker. Source path:
			`InvoiceTracker.Database -> Database setup -> compose.yaml`
	Step 3: Execute the SQL scripts to create the necessary tables and constraints. Source path
			`InvoiceTracker.Database -> Data scripts ->PostgreSQL\*.sql`.

### Database connection string configuration
	Database connection string in project  InvoiceTracker.WebApi and InvoiceTracker.Worker `appsettings.json`
	Example postgreSQL connection string: `Host=172.18.0.2;Port=5432;Database=InvoiceTracker;Username=Your_Username;Password=Your_Password;SSL Mode=Disable`

### Backend: Web Api
	
	cd src/InvoiceTracker.App/Presentation/InvoiceTracker.WebApi
	dotnet restore
	dotnet build
	dotnet run
	

### Backend: Web Worker
	
	cd src/InvoiceTracker.App/Presentation/InvoiceTracker.Worker
	dotnet restore
	dotnet build
	dotnet run
	

### Frontend requirements:
	bash
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
	

### Frontend docker container deployment:
	cd `src\InvoiceTracker.App\Presentation\InvoiceTracker.WebUI`
	run command as admin in powershell `PS>docker run --rm -p 8080:80 invoice-tracker-web-ui`
### Frontend local run:
	run the following command `ng serve -o`
	


