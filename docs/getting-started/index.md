# Getting Started

Welcome to the **Real Estate Listing API** documentation! This guide will help you get up and running quickly.

## What is Real Estate Listing API?

Real Estate Listing API is a modern, opinionated REST API for managing real estate property listings. Built with **C#** and **ASP.NET Core (.NET 10)**, it uses **EF Core** with **SQL Server** for persistence.

## Key Features

- **CRUD Operations**: Create, read, update, and delete property listings
- **State Transitions**: Publish and archive listings with business rule validation
- **Swagger/OpenAPI**: Interactive API documentation
- **EF Core Migrations**: Database versioning and schema management
- **Exception Handling**: Centralized middleware for consistent error responses
- **Unit & Integration Tests**: Comprehensive test coverage with xUnit
- **CI/CD Pipeline**: Automated builds, tests, and deployments via GitHub Actions
- **Docker Support**: Containerized deployment
- **Azure Container Apps**: Cloud-native deployment

## Architecture

The application follows a **Clean Architecture** pattern with four distinct layers:

- **Presentation Layer** (`RealEstateListing.API`): Controllers, Middleware, ViewModels, Swagger
- **Application Layer** (`RealEstateListing.Application`): Services, DTOs, Exceptions
- **Domain Layer** (`RealEstateListing.Domain`): Entities, Value Objects, Repositories
- **Infrastructure Layer** (`RealEstateListing.Infrastructure`): EF Core DbContext, Migrations, Repos

## Next Steps

- [Installation](installation.md) — Set up your development environment
- [Quick Start](quick-start.md) — Run the API in minutes
- [API Reference](../api/index.md) — Explore the full API documentation
