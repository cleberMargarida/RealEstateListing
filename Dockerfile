FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and project files
COPY RealEstateListing.slnx .
COPY src/RealEstateListing.Domain/RealEstateListing.Domain.csproj src/RealEstateListing.Domain/
COPY src/RealEstateListing.Application/RealEstateListing.Application.csproj src/RealEstateListing.Application/
COPY src/RealEstateListing.Infrastructure/RealEstateListing.Infrastructure.csproj src/RealEstateListing.Infrastructure/
COPY src/RealEstateListing.API/RealEstateListing.API.csproj src/RealEstateListing.API/

# Restore
RUN dotnet restore src/RealEstateListing.API/RealEstateListing.API.csproj

# Copy source code
COPY src/ src/

# Build
WORKDIR /src/src/RealEstateListing.API
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealEstateListing.API.dll"]
