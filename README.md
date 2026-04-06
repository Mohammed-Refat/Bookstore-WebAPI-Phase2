# Bookstore Management API - Production-Ready Implementation

This project is a high-performance, production-ready Bookstore REST API developed as part of **Phase 2** of the .NET Backend Roadmap. It strictly follows **Clean Architecture** principles and implements modern industry standards for security, validation, and testing.

## 🏗 Architectural Overview
The solution is organized into four distinct layers to ensure separation of concerns and maintainability:
* **Domain:** Contains Enterprise entities, Value Objects, and Domain logic.
* **Application:** Handles Business Logic, DTOs, Mapping, and Request Validation (FluentValidation).
* **Infrastructure:** Manages Data Persistence (EF Core / SQL Server) and External Services.
* **API:** The entry point (ASP.NET Core) handling Controllers, Middleware, and Dependency Injection.

## 🚀 Key Technical Implementations

### 1. Security & Resilience
* **JWT Authentication:** Secure endpoint access using JSON Web Tokens.
* **Rate Limiting:** Implemented a **Sliding Window** strategy per user to prevent API abuse and ensure fair resource distribution.
* **Global Error Handling:** A centralized middleware that transforms all application exceptions into the **RFC 7807 (Problem Details)** standard for consistent API responses.

### 2. Validation & Quality Assurance
* **FluentValidation:** Robust request validation integrated directly into the API pipeline.
* **Integration Testing:** High-coverage tests using **WebApplicationFactory** to simulate real-world API requests and database interactions.

### 3. API Standards
* **RESTful Design:** Proper use of HTTP verbs and status codes.
* **Swagger/OpenAPI:** Comprehensive documentation for API consumers.

## 🛠 Tech Stack
* **Framework:** .NET 8/9 (ASP.NET Core)
* **ORM:** Entity Framework Core
* **Database:** SQL Server
* **Testing:** xUnit & WebApplicationFactory
* **Validation:** FluentValidation

## ⚙️ Setup & Execution

1. **Clone & Restore:**
   \git clone https://github.com/Mohammed-Refat/Bookstore-WebAPI-Phase2.git\
   \dotnet restore\

2. **Database Configuration:**
   Update the connection string in \ppsettings.json\ and run:
   \dotnet ef database update\

3. **Running Tests:**
   Execute the integration test suite:
   \dotnet test\

4. **Launch API:**
   \dotnet run --project src/Bookstore.API\

---
*Developed by Mohammed Refat - .NET Backend Engineer*
