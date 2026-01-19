# Quick Start

Get the Real Estate Listing API running in minutes!

## Run the API

After completing the [installation](installation.md), start the API:

```bash
dotnet run --project src/RealEstateListing.API
```

## Access the API

Once running, the API is available at:

- **Swagger UI (HTTPS)**: https://localhost:7258/swagger
- **Swagger UI (HTTP)**: http://localhost:5101/swagger

## Make Your First Request

### Create a Listing

```bash
curl -X POST https://localhost:7258/api/listings \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Modern Downtown Apartment",
    "description": "Beautiful 2-bedroom apartment in the heart of the city",
    "price": 350000,
    "address": {
      "street": "123 Main Street",
      "city": "São Paulo",
      "state": "SP",
      "zipCode": "01310-100",
      "country": "Brazil"
    },
    "bedrooms": 2,
    "bathrooms": 1,
    "area": 75
  }'
```

### Get All Listings

```bash
curl https://localhost:7258/api/listings
```

### Get a Specific Listing

```bash
curl https://localhost:7258/api/listings/{id}
```

### Publish a Listing

```bash
curl -X PATCH https://localhost:7258/api/listings/{id}/publish
```

### Archive a Listing

```bash
curl -X PATCH https://localhost:7258/api/listings/{id}/archive
```

### Delete a Listing

```bash
curl -X DELETE https://localhost:7258/api/listings/{id}
```

## Live Demo

Don't want to run locally? Try our live demo:

🔗 **Production API**: https://ca-realestate-api.thankfulstone-3c733688.brazilsouth.azurecontainerapps.io/swagger/index.html

## Next Steps

- [API Endpoints](../api-endpoints/listings.md) — Full endpoint documentation
- [Architecture](../architecture/index.md) — Understand the project structure
- [Docker](../deployment/docker.md) — Run in a container
