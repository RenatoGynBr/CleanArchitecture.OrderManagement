# -------------------------
# Build stage
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copy project files first
# This improves Docker layer caching during restore.

COPY ["CleanArchitecture.OrderManagement.API/CleanArchitecture.OrderManagement.API.csproj", "CleanArchitecture.OrderManagement.API/"]
COPY ["CleanArchitecture.OrderManagement.Application/CleanArchitecture.OrderManagement.Application.csproj", "CleanArchitecture.OrderManagement.Application/"]
COPY ["CleanArchitecture.OrderManagement.Domain/CleanArchitecture.OrderManagement.Domain.csproj", "CleanArchitecture.OrderManagement.Domain/"]
COPY ["CleanArchitecture.OrderManagement.Infrastructure/CleanArchitecture.OrderManagement.Infrastructure.csproj", "CleanArchitecture.OrderManagement.Infrastructure/"]

# Restore dependencies
RUN dotnet restore \
    "CleanArchitecture.OrderManagement.API/CleanArchitecture.OrderManagement.API.csproj"

# Copy the remaining source code
COPY . .

WORKDIR "/src/CleanArchitecture.OrderManagement.API"

# Publish application
RUN dotnet publish \
    "CleanArchitecture.OrderManagement.API.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# -------------------------
# Runtime stage
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

# Create directory used by SQLite
RUN mkdir -p /app/data

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CleanArchitecture.OrderManagement.API.dll"]