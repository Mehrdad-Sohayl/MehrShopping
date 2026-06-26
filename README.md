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

Configuration is managed through `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MehrShopping;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "PersonalInfoClient": {
    "BaseAddress": "https://localhost:7120/",
    "Timeout": 10
  }
}
```

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

3. **Apply database migrations**
   ```bash
   cd src/MehrShopping.Api
   dotnet ef database update
   ```

4. **Run the application**
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
