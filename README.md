<div align="center">
 
<img src="https://img.shields.io/badge/version-1.0.0-blue?style=for-the-badge" />
<img src="https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet" />
<img src="https://img.shields.io/badge/Architecture-CQRS-orange?style=for-the-badge" />
<img src="https://img.shields.io/badge/Pattern-Clean%20Architecture-green?style=for-the-badge" />
<img src="https://img.shields.io/badge/Status-Active-brightgreen?style=for-the-badge" />

<br/><br/>

<h1>📦 BaridikExpress — Delivery Platform</h1>

<p><strong>Smart & Reliable Shipping Platform</strong> — Empowering delivery companies to manage orders, track shipments, and coordinate logistics with high efficiency.</p>

</div>

---

## 🗂️ Table of Contents

- [📌 Overview](#-overview)
- [👥 Team](#-team)
- [🏗️ Architecture](#️-architecture)
- [⚙️ CQRS Pattern](#️-cqrs-pattern-deep-dive)
- [🚀 Tech Stack](#-tech-stack)
- [📁 Project Structure](#-project-structure)
- [🔧 Getting Started](#-getting-started)
- [📦 NuGet Packages](#-nuget-packages)
- [🌐 API Endpoints](#-api-endpoints)
- [🔐 Authentication & Authorization](#-authentication--authorization)
- [🗃️ Database & Migrations](#️-database--migrations)
- [🤝 Contributing](#-contributing)

---

## 📌 Overview

**BaridikExpress** is a robust, scalable delivery management platform built for logistics and shipping companies. It provides a comprehensive API to manage:

| Module | Description |
|--------|-------------|
| 🚚 **Delivery Management** | Register drivers, approve, track, and manage deliveries |
| 👤 **Customer Management** | Full customer lifecycle with contacts, addresses, and accounts |
| 🔐 **Auth & Identity** | JWT-based authentication with role & permission system |
| 🗺️ **Location & Geography** | Countries, Governments, Cities, Villages hierarchy |
| ⚙️ **System Management** | Policies, terms, social media links, FAQs, and more |
| 🛵 **Delivery Types & Services** | Configurable delivery types with pricing and services |

---

## 👥 Team

<div align="center">

| 👤 Name | 🎯 Role |
|--------|--------|
| **Omar Ayman** | Backend Team Lead |
| **Mohammed Khaled** | Backend Developer |
| **Ahmed Momtaz** | Backend Developer |

</div>

---

## 🏗️ Architecture

BaridikExpress follows **Clean Architecture** principles with a strict layered separation:

```
┌─────────────────────────────────────────────────────────────┐
│                     Presentation Layer                       │
│                 (API Controllers / Minimal API)              │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                     Application Layer                        │
│         (CQRS Commands / Queries / Handlers / DTOs)          │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                      Domain Layer                            │
│          (Entities / Value Objects / Domain Events)          │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                   Infrastructure Layer                       │
│     (EF Core / Repositories / External Services / Cache)     │
└─────────────────────────────────────────────────────────────┘
```

### Design Principles

| Principle | Implementation |
|-----------|----------------|
| **Single Responsibility** | Each Command/Query has one dedicated Handler |
| **Open/Closed** | New features via new Commands, no modifying existing |
| **Dependency Inversion** | All dependencies injected via interfaces |
| **Separation of Concerns** | Read model vs Write model fully separated |

---

## ⚙️ CQRS Pattern Deep Dive

**CQRS (Command Query Responsibility Segregation)** separates read and write operations into distinct models.

```
User Request
     │
     ▼
  Controller
     │
     ├──── Write ──► Command ──► CommandHandler ──► Database (Write)
     │
     └──── Read  ──► Query  ──►  QueryHandler  ──► Database (Read)
```

### Pipeline Behaviors (MediatR)

```
Request ──► ValidationBehavior ──► LoggingBehavior ──► Handler ──► Response
```

| Behavior | Responsibility |
|----------|----------------|
| `ValidationBehavior` | Auto-validates using FluentValidation before handler runs |
| `LoggingBehavior` | Logs request/response with timing |

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
| **Swagger / Swashbuckle** | 6.x | API Documentation |

### Database & Security

| Technology | Purpose |
|------------|---------|
| **SQL Server** | Primary Database |
| **EF Core Migrations** | Schema Management |
| **ASP.NET Core Identity** | User Management |
| **JWT Bearer Tokens** | Stateless Authentication |
| **Role & Permission-Based Authorization** | Fine-grained Access Control |

---

## 📁 Project Structure

```
BaridikExpress/
│
├── 📁 BaridikExpress.API/
│   ├── Controllers/
│   │   ├── AuthModule/
│   │   ├── CustomerModule/
│   │   ├── DeliveryModule/
│   │   └── SystemManagement/
│   ├── Middlewares/
│   │   └── ExceptionHandlingMiddleware.cs
│   └── Program.cs
│
├── 📁 BaridikExpress.Application/
│   ├── Features/
│   │   ├── Auth/
│   │   ├── Customer/
│   │   ├── DeliveryType/
│   │   ├── Services/
│   │   └── SystemManagement/
│   ├── Behaviors/
│   │   └── ValidationBehavior.cs
│   ├── Interfaces/
│   │   └── IApplicationDbContext.cs
│   └── Common/
│
├── 📁 BaridikExpress.Domain/
│   ├── Entities/
│   │   ├── AuthModule/
│   │   ├── Customers/
│   │   ├── DeliveryModule/
│   │   ├── Location/
│   │   └── SystemManagment/
│   └── Enums/
│
└── 📁 BaridikExpress.Infrastructure/
    ├── Persistence/
    │   ├── ApplicationDbContext.cs
    │   ├── Configurations/
    │   └── Migrations/
    ├── Data/
    │   └── Seeder/
    └── DependencyInjection.cs
```

---

## 🔧 Getting Started

### Prerequisites

```
✅ .NET SDK 9.0+
✅ SQL Server
✅ Git
```

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/Dr-CodE-Software-Company/baridik-express-project-back-end.git
cd baridik-express-project-back-end
```

### 2️⃣ Configure appsettings

Update `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BaridikExpressDB;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-here",
    "Issuer": "baridikexpress-api",
    "Audience": "baridikexpress-clients",
    "ExpiryMinutes": 60
  },
  "App": {
    "TimeZoneId": "Egypt Standard Time"
  }
}
```

### 3️⃣ Apply Database Migrations

```bash
dotnet ef database update \
  --project BaridikExpress.Infrastructure \
  --startup-project BaridikExpress.API
```

### 4️⃣ Run the Application

```bash
dotnet run --project BaridikExpress.API
```

> 🌐 Swagger UI: `https://localhost:7240/swagger`

---

## 📦 NuGet Packages

### Application Layer

```xml
<PackageReference Include="MediatR" Version="12.4.1" />
<PackageReference Include="FluentValidation" Version="11.11.0" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.11.0" />
```

### Infrastructure Layer

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.4" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.4" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.4" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.4" />
```

### API Layer

```xml
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.9.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.4" />
```

---

## 🌐 API Endpoints

The API is split into multiple Swagger groups:

### 🔐 Auth API

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/v1/auth/Auth/register` | Register new user |
| `POST` | `/api/v1/auth/Auth/Login` | Login & get JWT token |
| `POST` | `/api/v1/auth/Auth/refresh-token` | Refresh access token |
| `POST` | `/api/v1/auth/Auth/logout` | Logout |
| `POST` | `/api/v1/auth/Auth/confirm-email` | Confirm email |
| `POST` | `/api/v1/auth/Auth/resend-verification` | Resend verification |
| `POST` | `/api/v1/auth/Auth/forgot-password` | Forgot password |
| `POST` | `/api/v1/auth/Auth/verify-reset-otp` | Verify OTP |
| `POST` | `/api/v1/auth/Auth/reset-password` | Reset password |
| `POST` | `/api/v1/auth/Auth/change-password` | Change password |
| `GET` | `/api/v1/auth/Auth/me` | Get current user profile |
| `GET` | `/api/v1/auth/Auth/me/permissions` | Get current user permissions |
| `POST` | `/api/v1/auth/Auth/validate-token` | Validate token |

### 👤 Sub Admin Employees

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/SubAdminEmployee` | Get all |
| `POST` | `/api/SubAdminEmployee` | Create |
| `PUT` | `/api/SubAdminEmployee` | Update |
| `DELETE` | `/api/SubAdminEmployee` | Delete list |
| `GET` | `/api/SubAdminEmployee/{id}` | Get by ID |
| `PATCH` | `/api/SubAdminEmployee/toggle-status/{id}` | Toggle status |

### 👥 Customers

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/v1/Customer/Create` | Create customer |
| `PUT` | `/api/v1/Customer/Update/{id}` | Update customer |
| `GET` | `/api/v1/Customer/GetAll` | Get all customers |
| `GET` | `/api/v1/Customer/GetById/{id}` | Get by ID |
| `PATCH` | `/api/v1/Customer/ToggleStatus/{id}` | Toggle status |
| `DELETE` | `/api/v1/Customer/DeleteList` | Delete list |

### 🚚 Delivery

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/v1/delivery/Delivery/RegisterDriver` | Driver self-registration |
| `POST` | `/api/v1/delivery/Delivery/CreateByAdmin` | Create driver by admin |
| `PATCH` | `/api/v1/delivery/Delivery/ApproveDelivery/{id}` | Approve driver |
| `GET` | `/api/v1/delivery/Delivery/GetAll` | Get all deliveries |
| `GET` | `/api/v1/delivery/Delivery/GetById/{id}` | Get by ID |

### 🛵 Delivery Types

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/v1/DeliveryType/Create` | Create delivery type |
| `PUT` | `/api/v1/DeliveryType/Update/{id}` | Update |
| `GET` | `/api/v1/DeliveryType/GetAll` | Get all |
| `GET` | `/api/v1/DeliveryType/GetById/{id}` | Get by ID |
| `PATCH` | `/api/v1/DeliveryType/ToggleStatus/{id}` | Toggle status |
| `DELETE` | `/api/v1/DeliveryType/DeleteList` | Delete list |

### 🛠️ Services

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/v1/Service/Create` | Create service |
| `PUT` | `/api/v1/Service/Update/{id}` | Update |
| `GET` | `/api/v1/Service/GetAll` | Get all |
| `GET` | `/api/v1/Service/GetById/{id}` | Get by ID |
| `PATCH` | `/api/v1/Service/ToggleStatus/{id}` | Toggle status |
| `DELETE` | `/api/v1/Service/DeleteList` | Delete list |

### 🗺️ Location & Geography

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET/POST/PUT/DELETE` | `/api/Country` | Countries CRUD |
| `PATCH` | `/api/Country/toggle-status/{id}` | Toggle status |
| `GET/POST/PUT/DELETE` | `/api/Government` | Governments CRUD |
| `PATCH` | `/api/Government/toggle-status/{id}` | Toggle status |
| `GET/POST/PUT/DELETE` | `/api/City` | Cities CRUD |
| `PATCH` | `/api/City/toggle-status/{id}` | Toggle status |
| `GET/POST/PUT/DELETE` | `/api/Village` | Villages CRUD |
| `PATCH` | `/api/Village/toggle-status/{id}` | Toggle status |

### 📋 Career Fields & Vehicles

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET/POST/PUT/DELETE` | `/api/v1/CareerFields` | Career Fields CRUD |
| `PATCH` | `/api/v1/CareerFields/{id}/toggle-status` | Toggle status |
| `GET` | `/api/v1/CareerFields/export` | Export |
| `POST` | `/api/v1/CareerFields/upload` | Upload Excel |
| `GET/POST/PUT/DELETE` | `/api/v1/Vehicles` | Vehicles CRUD |
| `PATCH` | `/api/v1/Vehicles/{id}/toggle-status` | Toggle status |

### ⚙️ System Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET/PUT` | `/api/v1/Help` | Help content |
| `GET/PUT` | `/api/v1/TermsAndConditions` | Terms & Conditions |
| `GET/PUT` | `/api/v1/PrivacyPolicy` | Privacy Policy |
| `GET/PUT` | `/api/v1/ShippingPolicy` | Shipping Policy |
| `GET/PUT` | `/api/v1/SalesAndPurchasePolicy` | Sales & Purchase Policy |
| `GET/PUT` | `/api/v1/CustomerRegistration` | Customer Registration Terms |
| `GET/PUT` | `/api/v1/DeliveryDriverRegistrationTerms` | Driver Registration Terms |

---

## 🔐 Authentication & Authorization

### Roles

| Role | Description |
|------|-------------|
| `SuperAdmin` | Full system access |
| `Admin` | Manage platform data |
| `SubAdmin` | Limited admin access |

### Getting a Token

```bash
curl -X POST https://localhost:7240/api/v1/auth/Auth/Login \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@baridikexpress.com", "password": "your-password"}'
```

```json
{
  "isSuccess": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "...",
    "expiresAt": "2025-01-15T12:00:00Z"
  }
}
```

### Using the Token

```bash
curl -H "Authorization: Bearer {token}" https://localhost:7240/api/v1/Customer/GetAll
```

### Language Support

```
Accept-Language: ar   →  Arabic responses
Accept-Language: en   →  English responses (default)
```

---

## 🗃️ Database & Migrations

```bash
# Create new migration
dotnet ef migrations add MigrationName \
  --project BaridikExpress.Infrastructure \
  --startup-project BaridikExpress.API

# Apply migrations
dotnet ef database update \
  --project BaridikExpress.Infrastructure \
  --startup-project BaridikExpress.API

# Rollback migration
dotnet ef database update PreviousMigrationName \
  --project BaridikExpress.Infrastructure \
  --startup-project BaridikExpress.API
```

---

## 🤝 Contributing

```bash
git checkout -b feature/AmazingFeature
git commit -m 'feat: add amazing feature'
git push origin feature/AmazingFeature
# Then open a Pull Request
```

### Commit Convention

| Prefix | Usage |
|--------|-------|
| `feat:` | New feature |
| `fix:` | Bug fix |
| `docs:` | Documentation changes |
| `refactor:` | Code refactor |
| `test:` | Adding tests |
| `chore:` | Build/tooling changes |

---

<div align="center">

**Built with ❤️ by the BaridikExpress Team**

*Omar Ayman · Mohammed Khaled · Ahmed Momtaz*

*Delivering excellence, one shipment at a time. 🚀*

</div>
