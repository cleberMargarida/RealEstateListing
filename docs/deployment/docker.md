# Docker Deployment

This guide covers running the Real Estate Listing API in a Docker container.

## Prerequisites

- [Docker](https://www.docker.com/) installed on your machine

## Build the Image

Build the Docker image from the repository root:

```bash
docker build -t realestatelisting:local .
```

## Run the Container

```bash
docker run -p 5000:8080 -p 5001:8081 \
  -e ConnectionStrings__SqlServer="<your-connection-string>" \
  realestatelisting:local
```

Replace `<your-connection-string>` with your SQL Server connection string.

## Access the API

Once running, access the API at:

- **HTTP**: http://localhost:5000/swagger
- **HTTPS**: https://localhost:5001/swagger

## Environment Variables

| Variable | Description | Required |
|----------|-------------|----------|
| `ConnectionStrings__SqlServer` | SQL Server connection string | Yes |

> **Note**: Use double underscores (`__`) for nested configuration in environment variables.

## Docker Hub

Pre-built images are available on Docker Hub:

```bash
docker pull clebermargarida/realestatelisting:latest
```

### Available Tags

- `latest` — Most recent release
- `x.y.z` — Specific version (e.g., `1.0.0`)
- `x.y` — Minor version (e.g., `1.0`)

## Docker Compose Example

Create a `docker-compose.yml` for local development:

```yaml
version: '3.8'

services:
  api:
    image: clebermargarida/realestatelisting:latest
    ports:
      - "5000:8080"
    environment:
      - ConnectionStrings__SqlServer=Server=db;Database=RealEstateListing;User Id=sa;Password=Your_password123;TrustServerCertificate=True
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Your_password123
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql

volumes:
  sqldata:
```

Run with:

```bash
docker-compose up -d
```

## Troubleshooting

### Container fails to start

1. Verify the connection string environment variable is set correctly
2. Check that SQL Server is accessible from the container
3. Review container logs: `docker logs <container-id>`

### Database connection issues

- Ensure the SQL Server allows remote connections
- For Docker-to-Docker communication, use the service name (not `localhost`)
- Verify firewall rules allow the connection
