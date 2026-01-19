# Installation

This guide covers the prerequisites and setup steps to get the Real Estate Listing API running on your machine.

## Prerequisites

Before you begin, ensure you have the following installed:

### Required

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/sql-server) (or LocalDB for local development)

### Optional

- [Docker](https://www.docker.com/) — For containerized deployment
- [EF Core CLI Tools](https://learn.microsoft.com/ef/core/cli/dotnet) — For migrations

## Install EF Core Tools

If you plan to work with database migrations, install the EF Core CLI tools globally:

```bash
dotnet tool install --global dotnet-ef
```

## Clone the Repository

```bash
git clone https://github.com/cleberMargarida/real-estate-listing.git
cd real-estate-listing
```

## Restore Dependencies

```bash
dotnet restore
```

## Configure Database Connection

Update the connection string in `src/RealEstateListing.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "SqlServer": "Server=(localdb)\\MSSQLLocalDB;Database=RealEstateListing;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

> **Note**: In Development mode, database migrations are applied automatically on startup.

## Build the Solution

```bash
dotnet build --configuration Release
```

## Next Steps

- [Quick Start](quick-start.md) — Run the API and make your first request
