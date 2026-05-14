<div align="center">

<img src="https://img.shields.io/badge/version-1.0.0-blue?style=for-the-badge" />
<img src="https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet" />
<img src="https://img.shields.io/badge/Architecture-CQRS-orange?style=for-the-badge" />
<img src="https://img.shields.io/badge/Pattern-Clean%20Architecture-green?style=for-the-badge" />
<img src="https://img.shields.io/badge/Status-Active-brightgreen?style=for-the-badge" />

# 📦 BaridikExpress — Delivery Platform

> **Smart & Reliable Shipping Platform** — Empowering delivery companies to manage orders, track shipments, and coordinate logistics with high efficiency.

</div>

---

## 🗂️ Table of Contents

- [📌 Overview](#-overview)
- [🏗️ Architecture](#️-architecture)
- [⚙️ CQRS Pattern Deep Dive](#️-cqrs-pattern-deep-dive)
- [🚀 Tech Stack](#-tech-stack)
- [📁 Project Structure](#-project-structure)
- [🔧 Getting Started](#-getting-started)
- [📦 NuGet Packages](#-nuget-packages)
- [🌐 API Endpoints](#-api-endpoints)
- [🔐 Authentication & Authorization](#-authentication--authorization)
- [🗃️ Database & Migrations](#️-database--migrations)
- [🧪 Testing](#-testing)
- [🤝 Contributing](#-contributing)

---

## 📌 Overview

**BaridikExpress** is a robust, scalable delivery management platform built for logistics and shipping companies. It provides a comprehensive API to manage:

- 🚚 **Shipment Lifecycle** — Create, track, update, and complete deliveries
- 👤 **Customer Management** — Senders and receivers with full address handling
- 🏢 **Branch Operations** — Multi-branch support with zone management
- 🧾 **Order Processing** — Real-time order status and history
- 🔔 **Notifications** — Automated SMS/Email updates for customers

The system is designed with **reliability**, **scalability**, and **clean separation of concerns** at its core using the CQRS pattern powered by **MediatR**.

---

## 🏗️ Architecture

BaridikExpress follows **Clean Architecture** principles with a strict layered separation:

```
┌─────────────────────────────────────────────────────────────┐
│                        Presentation Layer                    │
│                    (API Controllers / Minimal API)           │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                      Application Layer                       │
│         (CQRS Commands / Queries / Handlers / DTOs)          │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                       Domain Layer                           │
│          (Entities / Value Objects / Domain Events)          │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                   Infrastructure Layer                       │
│     (EF Core / Repositories / External Services / Cache)     │
└─────────────────────────────────────────────────────────────┘
```

### Design Principles Applied

| Principle | Implementation |
|-----------|---------------|
| **Single Responsibility** | Each Command/Query has one dedicated Handler |
| **Open/Closed** | New features via new Commands, no modifying existing |
| **Dependency Inversion** | All dependencies injected via interfaces |
| **Separation of Concerns** | Read model vs Write model fully separated |

---

## ⚙️ CQRS Pattern Deep Dive

**CQRS (Command Query Responsibility Segregation)** separates read and write operations into distinct models, improving performance, scalability, and maintainability.

### How We Implement CQRS

```
User Request
     │
     ▼
  Controller
     │
     ├──── Write Operation ──► Command ──► CommandHandler ──► Database (Write)
     │
     └──── Read Operation  ──► Query  ──►  QueryHandler  ──► Database (Read)
```

### Command Example — Create Shipment

```csharp
// Command
public record CreateShipmentCommand(
    string SenderName,
    string ReceiverName,
    string DestinationAddress,
    decimal Weight,
    ShipmentType Type
) : IRequest<CreateShipmentResult>;

// Handler
public class CreateShipmentCommandHandler 
    : IRequestHandler<CreateShipmentCommand, CreateShipmentResult>
{
    private readonly IShipmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateShipmentCommandHandler(
        IShipmentRepository repository, 
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateShipmentResult> Handle(
        CreateShipmentCommand command, 
        CancellationToken cancellationToken)
    {
        var shipment = Shipment.Create(
            command.SenderName,
            command.ReceiverName,
            command.DestinationAddress,
            command.Weight,
            command.Type
        );

        await _repository.AddAsync(shipment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateShipmentResult(shipment.Id, shipment.TrackingCode);
    }
}
```

### Query Example — Get Shipment by Tracking Code

```csharp
// Query
public record GetShipmentByTrackingQuery(string TrackingCode) 
    : IRequest<ShipmentDto>;

// Handler
public class GetShipmentByTrackingQueryHandler 
    : IRequestHandler<GetShipmentByTrackingQuery, ShipmentDto>
{
    private readonly IShipmentReadRepository _readRepository;

    public async Task<ShipmentDto> Handle(
        GetShipmentByTrackingQuery query, 
        CancellationToken cancellationToken)
    {
        var shipment = await _readRepository
            .GetByTrackingCodeAsync(query.TrackingCode, cancellationToken);

        if (shipment is null)
            throw new ShipmentNotFoundException(query.TrackingCode);

        return shipment.ToDto();
    }
}
```

### Pipeline Behaviors (MediatR)

We use MediatR Pipeline Behaviors to add cross-cutting concerns:

```
Request ──► ValidationBehavior ──► LoggingBehavior ──► Handler ──► Response
```

| Behavior | Responsibility |
|----------|---------------|
| `ValidationBehavior` | Auto-validates using FluentValidation before handler runs |
| `LoggingBehavior` | Logs request/response with timing |
| `TransactionBehavior` | Wraps Commands in DB transactions automatically |
| `CachingBehavior` | Caches Query results with configurable TTL |

---

## 🚀 Tech Stack

### Backend

| Technology | Version | Purpose |
|------------|---------|---------|
| **.NET** | 9.0 | Core Framework |
| **ASP.NET Core** | 9.0 | Web API |
| **Entity Framework Core** | 9.x | ORM & Database Access |
| **MediatR** | 12.x | CQRS Mediator Pattern |
| **FluentValidation** | 11.x | Input Validation |
| **AutoMapper** | 13.x | Object-to-Object Mapping |
| **Serilog** | 3.x | Structured Logging |
| **Swagger / Swashbuckle** | 6.x | API Documentation |

### Database & Caching

| Technology | Purpose |
|------------|---------|
| **SQL Server / PostgreSQL** | Primary Database |
| **Redis** | Distributed Caching |
| **EF Core Migrations** | Schema Management |

### Security & Auth

| Technology | Purpose |
|------------|---------|
| **ASP.NET Core Identity** | User Management |
| **JWT Bearer Tokens** | Stateless Authentication |
| **Role-Based Authorization** | Access Control |

---

## 📁 Project Structure

```
BaridikExpress/

├── 📁 src/
│   ├── 📁 BaridikExpress.API/                    # Presentation Layer
│   │   ├── Controllers/
│   │   │   ├── ShipmentsController.cs
│   │   │   ├── OrdersController.cs
│   │   │   └── CustomersController.cs
│   │   ├── Middlewares/
│   │   │   ├── ExceptionHandlingMiddleware.cs
│   │   │   └── RequestLoggingMiddleware.cs
│   │   └── Program.cs
│   │
│   ├── 📁 BaridikExpress.Application/            # Application Layer (CQRS)
│   │   ├── Features/
│   │   │   ├── Shipments/
│   │   │   │   ├── Commands/
│   │   │   │   │   ├── CreateShipment/
│   │   │   │   │   ├── UpdateShipmentStatus/
│   │   │   │   │   └── CancelShipment/
│   │   │   │   └── Queries/
│   │   │   │       ├── GetShipmentByTracking/
│   │   │   │       └── GetShipmentsList/
│   │   │   ├── Orders/
│   │   │   └── Customers/
│   │   ├── Behaviors/
│   │   │   ├── ValidationBehavior.cs
│   │   │   ├── LoggingBehavior.cs
│   │   │   └── TransactionBehavior.cs
│   │   ├── Common/
│   │   │   ├── Interfaces/
│   │   │   └── Mappings/
│   │   └── DependencyInjection.cs
│   │
│   ├── 📁 BaridikExpress.Domain/                 # Domain Layer
│   │   ├── Entities/
│   │   │   ├── Shipment.cs
│   │   │   ├── Order.cs
│   │   │   └── Customer.cs
│   │   ├── Enums/
│   │   │   ├── ShipmentStatus.cs
│   │   │   └── ShipmentType.cs
│   │   ├── Events/
│   │   │   └── ShipmentStatusChangedEvent.cs
│   │   └── Exceptions/
│   │       ├── ShipmentNotFoundException.cs
│   │       └── InvalidShipmentStatusException.cs
│   │
│   └── 📁 BaridikExpress.Infrastructure/         # Infrastructure Layer
│       ├── Persistence/
│       │   ├── ApplicationDbContext.cs
│       │   ├── Configurations/
│       │   │   ├── ShipmentConfiguration.cs
│       │   │   └── OrderConfiguration.cs
│       │   ├── Repositories/
│       │   │   ├── ShipmentRepository.cs
│       │   │   └── OrderRepository.cs
│       │   └── Migrations/
│       ├── Caching/
│       │   └── RedisCacheService.cs
│       ├── Notifications/
│       │   ├── SmsService.cs
│       │   └── EmailService.cs
│       └── DependencyInjection.cs
│
├── 📁 tests/
│   ├── BaridikExpress.Application.Tests/
│   │   ├── Shipments/
│   │   │   ├── CreateShipmentCommandHandlerTests.cs
│   │   │   └── GetShipmentQueryHandlerTests.cs
│   │   └── Orders/
│   └── BaridikExpress.API.Tests/
│       └── Integration/
│
├── 📄 .gitignore
├── 📄 docker-compose.yml
├── 📄 BaridikExpress.sln
└── 📄 README.md
```

---

## 🔧 Getting Started

### Prerequisites

Make sure you have the following installed:

```
✅ .NET SDK 9.0+          → https://dotnet.microsoft.com/download
✅ SQL Server / PostgreSQL → or use Docker (see below)
✅ Redis                   → or use Docker
✅ Git
```

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/Dr-CodE-Software-Company/baridik-express-project-back-end.git
cd baridik-express-project-back-end
```

### 2️⃣ Configure Environment

Copy the example settings and update with your values:

```bash
cp src/BaridikExpress.API/appsettings.example.json src/BaridikExpress.API/appsettings.Development.json
```

Update `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BaridikExpressDb;Trusted_Connection=True;",
    "Redis": "localhost:6379"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-here",
    "Issuer": "baridikexpress-api",
    "Audience": "baridikexpress-clients",
    "ExpiryMinutes": 60
  },
  "Serilog": {
    "MinimumLevel": "Information"
  }
}
```

### 3️⃣ Run with Docker (Recommended)

```bash
docker-compose up -d
```

This spins up:
- 🟦 SQL Server on port `1433`
- 🟥 Redis on port `6379`
- 🌐 API on port `5000` / `5001`

### 4️⃣ Apply Database Migrations

```bash
cd src/BaridikExpress.API
dotnet ef database update
```

### 5️⃣ Run the Application

```bash
dotnet run --project src/BaridikExpress.API
```

API will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger`

---

## 📦 NuGet Packages

### Application Layer

```xml
<!-- CQRS Mediator -->
<PackageReference Include="MediatR" Version="12.4.1" />
<PackageReference Include="MediatR.Extensions.Microsoft.DependencyInjection" Version="11.1.0" />

<!-- Validation -->
<PackageReference Include="FluentValidation" Version="11.11.0" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.11.0" />

<!-- Mapping -->
<PackageReference Include="AutoMapper" Version="13.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
```

### Infrastructure Layer

```xml
<!-- Entity Framework Core -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.4" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.4" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.4" />

<!-- Caching -->
<PackageReference Include="StackExchange.Redis" Version="2.8.16" />
<PackageReference Include="Microsoft.Extensions.Caching.StackExchangeRedis" Version="9.0.4" />

<!-- Logging -->
<PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
<PackageReference Include="Serilog.Sinks.Console" Version="6.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="6.0.0" />
```

### API Layer

```xml
<!-- Swagger -->
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.9.0" />

<!-- Authentication -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.4" />

<!-- Health Checks -->
<PackageReference Include="AspNetCore.HealthChecks.SqlServer" Version="9.0.0" />
<PackageReference Include="AspNetCore.HealthChecks.Redis" Version="9.0.0" />
```

---

## 🌐 API Endpoints

### Shipments

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/shipments` | Create new shipment | ✅ Required |
| `GET` | `/api/shipments/{id}` | Get shipment by ID | ✅ Required |
| `GET` | `/api/shipments/track/{code}` | Track by code (public) | ❌ Public |
| `PUT` | `/api/shipments/{id}/status` | Update shipment status | ✅ Admin |
| `DELETE` | `/api/shipments/{id}` | Cancel shipment | ✅ Required |
| `GET` | `/api/shipments` | List all shipments | ✅ Admin |

### Orders

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/orders` | Create new order | ✅ Required |
| `GET` | `/api/orders/{id}` | Get order details | ✅ Required |
| `GET` | `/api/orders/customer/{customerId}` | Customer order history | ✅ Required |
| `PUT` | `/api/orders/{id}` | Update order | ✅ Required |

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/auth/register` | Register new user |
| `POST` | `/api/auth/login` | Login & get JWT token |
| `POST` | `/api/auth/refresh` | Refresh access token |

---

## 🔐 Authentication & Authorization

We use **JWT Bearer Authentication** with role-based access control.

### Roles

| Role | Permissions |
|------|-------------|
| `SuperAdmin` | Full system access |
| `BranchManager` | Manage branch shipments & staff |
| `Courier` | Update delivery status |
| `Customer` | Track own shipments |

### Getting a Token

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@baridikexpress.com", "password": "your-password"}'
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresAt": "2025-01-15T12:00:00Z",
  "refreshToken": "..."
}
```

---

## 🗃️ Database & Migrations

### Create a New Migration

```bash
dotnet ef migrations add MigrationName \
  --project src/BaridikExpress.Infrastructure \
  --startup-project src/BaridikExpress.API
```

### Apply Migrations

```bash
dotnet ef database update \
  --project src/BaridikExpress.Infrastructure \
  --startup-project src/BaridikExpress.API
```

### Rollback Migration

```bash
dotnet ef database update PreviousMigrationName \
  --project src/BaridikExpress.Infrastructure \
  --startup-project src/BaridikExpress.API
```

---

## 🧪 Testing

### Run All Tests

```bash
dotnet test
```

### Run with Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Test Structure

```
tests/
├── Unit Tests          → Handler logic, Domain rules
├── Integration Tests   → API endpoints, DB operations
└── Architecture Tests  → Layer dependency validation (NetArchTest)
```

---

## 🤝 Contributing

1. Fork the repo
2. Create your feature branch: `git checkout -b feature/AmazingFeature`
3. Commit your changes: `git commit -m 'feat: add amazing feature'`
4. Push to the branch: `git push origin feature/AmazingFeature`
5. Open a Pull Request

### Commit Convention

We follow **Conventional Commits**:

```
feat:     New feature
fix:      Bug fix
docs:     Documentation changes
refactor: Code refactor
test:     Adding tests
chore:    Build/tooling changes
```

---

<div align="center">

**Built with ❤️ by the BaridikExpress Team**

*Delivering excellence, one shipment at a time.*

</div>
