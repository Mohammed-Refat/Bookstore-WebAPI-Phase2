# 📚 Bookstore REST API

> A production-ready RESTful API built with ASP.NET Core 8, demonstrating Clean Architecture, JWT authentication, FluentValidation, rate limiting, and integration testing.

---

## 🎯 What This Project Demonstrates

This project was built as part of a structured .NET Backend Engineering roadmap. It showcases the patterns and practices expected in professional .NET backend roles:

- **Clean Architecture** — strict separation of concerns across 4 layers
- **JWT Authentication** — stateless, token-based security with HS256 signing
- **FluentValidation** — declarative, testable input validation
- **Global Exception Handling** — RFC 7807 Problem Details standard
- **Rate Limiting** — sliding window algorithm (ASP.NET Core 7+ built-in)
- **Integration Testing** — full HTTP pipeline tested with `WebApplicationFactory`

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 |
| Language | C# 12 |
| Authentication | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Validation | FluentValidation 12 |
| API Docs | Swashbuckle / Swagger UI |
| Testing | xUnit + FluentAssertions + `WebApplicationFactory` |
| Architecture | Clean Architecture (Domain / Application / Infrastructure / API) |

---

## 🏗️ Architecture

```
Bookstore.sln
├── src/
│   ├── Bookstore.Domain          → Entities, repository interfaces — zero dependencies
│   ├── Bookstore.Application     → Use cases, DTOs, validators, service interfaces
│   ├── Bookstore.Infrastructure  → In-memory repository (EF Core in Phase 3)
│   └── Bookstore.API             → Controllers, middleware, JWT, Program.cs
└── tests/
    └── Bookstore.Tests           → Integration tests (5 passing)
```

**Dependency rule:** Domain ← Application ← Infrastructure / API. The Domain never references any outer layer.

---

## 🔑 Key Features

### ✅ Clean Architecture
Four strictly separated projects with unidirectional dependencies. Business logic in the Application layer has zero knowledge of HTTP or databases.

### ✅ JWT Authentication
All `/api/books` endpoints are protected. Clients must authenticate via `POST /api/auth/login` and include the returned Bearer token in subsequent requests.

### ✅ RFC 7807 Problem Details
Every error — validation failure, not found, server error — returns a consistent, standard-compliant JSON shape:
```json
{
  "title": "Not Found",
  "status": 404,
  "detail": "Book with id 'abc' was not found.",
  "instance": "/api/books/abc"
}
```

### ✅ Sliding Window Rate Limiting
10 requests per 10-second sliding window per client. Exceeding the limit returns `429 Too Many Requests` immediately with no queuing.

### ✅ Integration Tests
5 tests covering the full HTTP pipeline — no mocks, real DI container, real middleware:
- `GET /api/books` returns `200 OK`
- `POST /api/books` with valid data returns `201 Created`
- `POST /api/books` with invalid data returns `400 Bad Request`
- `GET /api/books/{id}` with non-existent id returns `404 Not Found`
- `DELETE /api/books/{id}` returns `204 No Content`

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or VS Code

### Run Locally

```bash
git clone https://github.com/mohammed-refat/bookstore-api.git
cd bookstore-api

dotnet restore
dotnet build
dotnet run --project src/Bookstore.API
```

Swagger UI will be available at:
```
https://localhost:{port}/swagger
```

### Authenticate in Swagger

1. `POST /api/auth/login` with:
```json
{
  "username": "admin",
  "password": "password123"
}
```
2. Copy the returned token
3. Click **Authorize 🔓** in Swagger UI
4. Enter: `Bearer {your-token}`
5. All protected endpoints are now accessible

---

## 🧪 Running Tests

```bash
dotnet test
```

Expected output:
```
Passed! - Failed: 0, Passed: 5, Skipped: 0
```

---

## 📡 API Endpoints

| Method | Endpoint | Auth Required | Description |
|---|---|---|---|
| POST | `/api/auth/login` | ❌ | Get JWT token |
| GET | `/api/books` | ✅ | Get all books |
| GET | `/api/books/{id}` | ✅ | Get book by ID |
| POST | `/api/books` | ✅ | Create a book |
| PUT | `/api/books/{id}` | ✅ | Update a book |
| DELETE | `/api/books/{id}` | ✅ | Delete a book |

---

## 📁 Project Structure — Key Files

```
src/Bookstore.Domain/
├── Entities/Book.cs                          # Encapsulated entity with private setters
└── Interfaces/IBookRepository.cs             # Repository contract — no implementation details

src/Bookstore.Application/
├── DTOs/                                     # BookDto, CreateBookRequest, UpdateBookRequest
├── Services/BookService.cs                   # Core business logic — depends only on interfaces
└── Validators/                               # FluentValidation validators

src/Bookstore.Infrastructure/
└── Repositories/InMemoryBookRepository.cs    # In-memory implementation of IBookRepository

src/Bookstore.API/
├── Controllers/BooksController.cs            # HTTP layer — delegates to IBookService
├── Controllers/AuthController.cs             # Login endpoint — issues JWT tokens
├── Middleware/GlobalExceptionHandler.cs      # Catches all exceptions → Problem Details
└── Program.cs                                # Composition root — DI, middleware pipeline
```

---

## 🔮 Roadmap

This project is part of a 5-phase .NET Backend Engineering roadmap. Upcoming additions:

- **Phase 3** — Replace in-memory repository with EF Core + PostgreSQL
- **Phase 3** — Add database migrations and Repository + Unit of Work pattern
- **Phase 4** — CQRS with MediatR, Clean Architecture refinement
- **Phase 5** — Docker, CI/CD with GitHub Actions, OpenTelemetry

---

## 👨‍💻 Author

**Mohamed Refat**
- GitHub: [@mohammed-refat](https://github.com/mohammed-refat)
- Built as part of a structured .NET Backend Engineering roadmap targeting junior .NET Developer roles

---

## 📄 License

MIT
