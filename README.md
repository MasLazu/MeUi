# MeUi - Modular Authentication & Authorization System

A modern .NET 9 web API built with clean architecture principles, featuring modular authentication, authorization, and tenant management capabilities.

## Overview

MeUi is a comprehensive backend system designed for multi-tenant applications with robust authentication and authorization features. Built using FastEndpoints, Entity Framework Core, and PostgreSQL, it provides a solid foundation for scalable web applications.

## Architecture

The project follows a modular monolith architecture with clear separation of concerns:

### Core Modules

- **MeUi.Shared** - Common infrastructure, domain models, and application contracts
- **MeUi.Authentication** - Authentication services including password and OAuth support
- **MeUi.Authorization** - Authorization and permission management
- **MeUi.Tenant** - Multi-tenant support and user management
- **MeUi.Entry** - Main application entry point and API configuration

### Layer Structure

Each module follows clean architecture principles:

- **Public** - External contracts and extensions
- **Private** - Internal implementation
  - **Domain** - Business entities and domain logic
  - **Application** - Use cases and business rules
  - **Infrastructure** - Data access and external services
  - **Endpoints** - API endpoints and controllers

## Features

- **JWT Authentication** - Secure token-based authentication
- **Password Authentication** - Traditional username/password login
- **OAuth Support** - Third-party authentication integration
- **API Key Authentication** - Service-to-service authentication
- **Multi-tenant Architecture** - Support for multiple tenants
- **Role-based Authorization** - Granular permission system
- **RESTful API** - FastEndpoints-based API with Swagger documentation
- **Database Migrations** - Entity Framework Core migrations
- **Structured Logging** - Serilog integration with file and console output

## Technology Stack

- **.NET 9** - Latest .NET framework
- **FastEndpoints** - High-performance API framework
- **Entity Framework Core** - ORM for data access
- **PostgreSQL** - Primary database
- **JWT Bearer** - Authentication tokens
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation

## Getting Started

### Prerequisites

- .NET 9 SDK
- PostgreSQL database
- Visual Studio 2022 or VS Code

### Configuration

1. **Database Configuration**
   Update `appsettings.Development.json` with your PostgreSQL connection details:

   ```json
   {
     "Postgresql": {
       "Host": "your-postgres-host",
       "Port": "5432",
       "Username": "your-username",
       "Password": "your-password",
       "Database": "your-database"
     }
   }
   ```

2. **JWT Configuration**
   Configure JWT settings in `appsettings.json`:
   ```json
   {
     "Jwt": {
       "Secret": "your-super-secret-jwt-key-with-at-least-32-characters",
       "Issuer": "YourApp.Issuer",
       "Audience": "YourApp.Audience",
       "AccessTokenExpirationMinutes": 15,
       "RefreshTokenExpirationDays": 7
     }
   }
   ```

### Installation & Setup

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd MeUi
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Run database migrations**

   ```bash
   dotnet ef database update --project src/MeUi.Entry
   ```

4. **Build the solution**

   ```bash
   dotnet build
   ```

5. **Run the application**
   ```bash
   dotnet run --project src/MeUi.Entry
   ```

The API will be available at `https://localhost:5001` (or the port specified in launchSettings.json).

## API Documentation

Once the application is running, you can access:

- **Swagger UI**: `https://localhost:5001/swagger`
- **OpenAPI Spec**: `https://localhost:5001/swagger/v1/swagger.json`

## Default Credentials

The system includes a default super admin user:

- **Email**: admin@mataelang.com
- **Password**: SuperAdmin123!

## Project Structure

```
src/
├── MeUi.Entry/                     # Main application entry point
├── MeUi.Shared/                    # Shared components
│   ├── MeUi.Shared.Application/    # Shared application logic
│   ├── MeUi.Shared.ApplicationContract/ # Shared contracts
│   ├── MeUi.Shared.Domain/         # Shared domain models
│   ├── MeUi.Shared.Endpoint/       # Shared endpoint utilities
│   └── MeUi.Shared.Infrastructure/ # Shared infrastructure
├── MeUi.Authentication/            # Authentication module
│   ├── MeUi.Authentication.Core/   # Core authentication
│   ├── MeUi.Authentication.Password/ # Password authentication
│   ├── MeUi.Authentication.OAuth/  # OAuth authentication
│   └── MeUi.Authentication.ApiKey/ # API key authentication
├── MeUi.Authorization/             # Authorization module
└── MeUi.Tenant/                    # Tenant management module
```

## Development

### Adding New Modules

1. Create the module structure following the established pattern
2. Add project references to the solution file
3. Register services in the appropriate extension class
4. Add database context if needed

### Database Migrations

```bash
# Add new migration
dotnet ef migrations add MigrationName --project src/MeUi.Entry

# Update database
dotnet ef database update --project src/MeUi.Entry
```

### Logging

The application uses Serilog for structured logging:

- Console output for development
- File logging with daily rotation
- Configurable log levels per namespace

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For questions and support, please open an issue in the repository or contact the development team.
