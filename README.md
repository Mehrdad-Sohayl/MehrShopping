# 🛒 MehrShopping API

A backend e-commerce system built with **.NET 8**, **Domain-Driven Design (DDD)**, **CQRS**, and **Entity Framework Core** that manages customers, products, and invoices with integration to an external PersonalInfoService.

## ✨ Features

### 👥 Customer Management
- Register customers via external PersonalInfoService integration
- Update customer information
- Retrieve customer data
- Validate customer existence

### 📦 Product Management
- Register new products
- Delete products
- Manage product catalog

### 🧾 Invoice Management
- Create invoices with multiple items
- Validate customer and product availability
- Automatically decrease product stock

### 🌐 External Service Integration
- HTTP client integration with PersonalInfoService
- Configurable endpoint and timeout settings

## 🏛️ Architecture

The system implements **Domain-Driven Design (DDD)** with a **layered architecture**:

```text
┌───────────────────┐
│       API         │  ASP.NET Core MVC
└─────────┬─────────┘
          │
          ▼
┌───────────────────┐
│   Application     │  CQRS pattern with dedicated handlers
└─────────┬─────────┘
          │
          ▼
┌───────────────────┐
│      Domain       │  Core business logic and entities
└─────────┬─────────┘
          │
          ▼
┌───────────────────┐
│ Infrastructure    │  Data access and external services
└───────────────────┘
```

## 📂 Project Structure

```
MehrShopping/
├── src/
│   ├── MehrShopping.Api/               # ASP.NET Core Web API
│   ├── MehrShopping.Application/       # CQRS handlers and application logic
│   ├── MehrShopping.Domain/            # Domain entities, value objects, exceptions
│   ├── MehrShopping.Infrastructure/    # Repositories, EF Core, external clients
│   └── MehrShopping.sln               # Solution file
├── PersonalInfoService/               # External service dependency
└── README.md                          # This file
```

## 🎯 Domain Layer

### Key Domain Concepts
- **Customer**: Contains personal information and national code
- **Product**: Manages product details and stock
- **Invoice**: Represents a purchase with multiple items
- **Value Objects**: Includes `Name`, `NationalCode`, `Quantity`

### Business Rules
- Customers must have a valid national code
- Products cannot have negative stock quantity
- Invoices require valid customer and product references

## ⚡ Application Layer

Uses **CQRS** pattern with direct handler invocation (no MediatR):

### Commands
- `RegisterCustomerCommand`
- `UpdateCustomerCommand`
- `RegisterProductCommand`
- `DeleteProductCommand`
- `CreateInvoiceCommand`

### Queries
- `GetInvoiceListQuery`
- `GetCustomerQuery`
- `GetProductQuery`

## 🗄️ Infrastructure Layer

### Data Access
- **Entity Framework Core** with SQL Server
- **Unit Of Work pattern** for transaction management
- **Repository pattern** for data access

### External Services
- **PersonalInfoServiceClient**: HTTP client for customer data

## 🔌 API Endpoints

| Module    | Endpoint               | Method | Description                     |
|-----------|------------------------|--------|---------------------------------|
| Customer  | `/api/Customer`        | POST   | Register a new customer         |
| Customer  | `/api/Customer`        | PUT    | Update customer information     |
| Product   | `/api/Product`         | POST   | Register a new product          |
| Product   | `/api/Product`         | DELETE | Delete a product                |
| Invoice   | `/api/Invoice`         | POST   | Create a new invoice            |
| Invoice   | `/api/Invoice`         | GET    | Get invoice list                |

## 📖 API Documentation

Swagger/OpenAPI documentation is available at `/swagger` when the application is running.

## ⚙️ Configuration

`appsettings.json` holds only **non-sensitive** settings. Database credentials are deliberately **not committed** — supply them per environment:

### Local development (User Secrets)

```bash
# MehrShopping.Api
cd src/MehrShopping.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=MehrShoppingDb;User Id=sa;Password=<your-password>;Encrypt=False;"

# PersonalInfo.Api
cd PersonalInfoService/PersonalInfo.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=PersonalInfoDb;User Id=sa;Password=<your-password>;Encrypt=False;"
```

User secrets are stored locally (`%LOCALAPPDATA%\Microsoft\UserSecrets` on Windows) and are only read in the `Development` environment.

### Deployment (environment variables or a secrets manager)

```powershell
# PowerShell (Windows)
$env:ConnectionStrings__DefaultConnection = "Server=...;Database=...;User Id=...;Password=...;Encrypt=True;"
```

```bash
# bash (Linux/macOS)
export ConnectionStrings__DefaultConnection="Server=...;Database=...;User Id=...;Password=...;Encrypt=True;"
```

> ⚠️ **Never commit real credentials.** If a secret is ever committed by mistake, rotate it immediately (a new password on the SQL Server). Removing the value from the file does **not** erase it from git history — the password itself must be changed.

## 🔐 Secret Scanning (pre-commit hook)

This repository includes a **pre-commit hook powered by [gitleaks](https://gitleaks.io)** that automatically scans staged changes for secrets (API keys, passwords, tokens, connection strings, private keys, etc.) before every commit.

### How it works
- On `git commit`, the hook runs `gitleaks protect --staged` against the staged changes.
- If a potential secret is detected, the commit is **blocked** with details.
- Built-in rules cover 100+ secret types (AWS, GitHub, Azure, Slack, private keys, etc.).
- A custom rule (`db-connection-password`) catches hardcoded database credentials in connection strings.

### Setup (one-time per clone)
```bash
# 1. Install gitleaks
# Windows (Scoop):
scoop install gitleaks
# Windows (Chocolatey):
choco install gitleaks
# macOS:
brew install gitleaks
# Linux / manual: https://github.com/gitleaks/gitleaks#installation

# 2. Enable the hook (points to .githooks/ directory in this repo)
git config core.hooksPath .githooks
```

> **Note:** If `gitleaks` is not on your PATH, the hook will warn but **not block** the commit — install it to activate protection.

### False positives
If a legitimate value is flagged, add an allowlist entry to `.gitleaks.toml` under the appropriate rule (see [gitleaks allowlist docs](https://github.com/gitleaks/gitleaks#allowlisting)).

## 🛠️ Build & Run

### Prerequisites
- .NET 8 SDK
- SQL Server (for database)

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/Mehrdad-Sohayl/MehrShopping.git
   cd MehrShopping
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Configure the database connection string**
   ```bash
   cd src/MehrShopping.Api
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=MehrShoppingDb;User Id=sa;Password=<your-password>;Encrypt=False;"
   ```
   Configure `PersonalInfo.Api` the same way (see the [Configuration](#-configuration) section).

4. **Apply database migrations**
   ```bash
   cd src/MehrShopping.Api
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   cd src/MehrShopping.Api
   dotnet run
   ```

The application will be available at `https://localhost:7120` (or configured port).

## 🧪 Testing

Unit tests are available in the `MehrShopping.Test` project. Run tests with:

```bash
cd src/MehrShopping.Test
dotnet test
```

## 🚀 Technology Stack

| Category         | Technology            |
|------------------|-----------------------|
| Platform         | .NET 8                |
| Web Framework    | ASP.NET Core MVC      |
| Database         | SQL Server            |
| ORM              | Entity Framework Core |
| Architecture     | DDD, CQRS             |
| API Documentation| Swagger/OpenAPI       |

## 📄 License

MIT License

## 📧 Contact

For issues and contributions, please use the GitHub issue tracker.
