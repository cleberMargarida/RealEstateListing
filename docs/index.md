---
_disableToc: true
_disableAffix: true
---

<style>
.hero { text-align: center; padding: 40px 20px; }
.hero h1 { font-size: 2.5em; margin-bottom: 10px; }
.hero p { font-size: 1.2em; color: #666; margin-bottom: 30px; }
.badges { margin-bottom: 30px; }
.badges img { margin: 0 5px; }
.features { display: flex; flex-wrap: wrap; justify-content: center; gap: 20px; margin: 40px 0; }
.feature { flex: 1 1 250px; max-width: 300px; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px; text-align: center; }
.cta-buttons { margin: 30px 0; }
.cta-buttons a { display: inline-block; padding: 12px 24px; margin: 0 10px; border-radius: 6px; text-decoration: none; font-weight: bold; }
.cta-primary { background-color: #0078d4; color: white !important; }
.cta-secondary { background-color: #f0f0f0; color: #333 !important; }
</style>

<div class="hero">

# 🏠 Real Estate Listing API

A modern, opinionated REST API for managing real estate property listings.

<div class="badges">

[![Build Status](https://github.com/cleberMargarida/real-estate-listing/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/cleberMargarida/real-estate-listing/actions/workflows/ci-cd.yml)
[![codecov](https://codecov.io/gh/cleberMargarida/real-estate-listing/graph/badge.svg)](https://codecov.io/gh/cleberMargarida/real-estate-listing)
[![Docker](https://img.shields.io/docker/v/clebermargarida/realestatelisting?label=docker&logo=docker)](https://hub.docker.com/r/clebermargarida/realestatelisting)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/cleberMargarida/real-estate-listing/blob/master/LICENSE)

</div>

<div class="cta-buttons">
  <a href="getting-started/index.md" class="cta-primary">Get Started</a>
  <a href="api/index.md" class="cta-secondary">API Reference</a>
</div>

</div>

## Quick Start

```bash
git clone https://github.com/cleberMargarida/real-estate-listing.git
cd real-estate-listing
dotnet run --project src/RealEstateListing.API
# Open https://localhost:7258/swagger
```

## Live Demo

🔗 **Production API**: https://ca-realestate-api.thankfulstone-3c733688.brazilsouth.azurecontainerapps.io/swagger/index.html

<div class="features">
  <div class="feature">
    <h3>🏗️ Clean Architecture</h3>
    <p>Follows Clean Architecture principles with clear separation of concerns across four layers.</p>
  </div>
  <div class="feature">
    <h3>📝 Swagger Documentation</h3>
    <p>Interactive API documentation with OpenAPI/Swagger UI.</p>
  </div>
  <div class="feature">
    <h3>🐳 Docker Ready</h3>
    <p>Containerized deployment with pre-built images on Docker Hub.</p>
  </div>
  <div class="feature">
    <h3>☁️ Azure Deployment</h3>
    <p>Automated deployment to Azure Container Apps via GitHub Actions.</p>
  </div>
</div>
