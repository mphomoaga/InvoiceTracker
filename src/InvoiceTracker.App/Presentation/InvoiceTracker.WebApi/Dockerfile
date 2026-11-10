# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

USER $APP_UID
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=44358

EXPOSE 44358


# Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY . .

# Restore NuGet packages
RUN dotnet restore \
    "Presentation/InvoiceTracker.WebApi/InvoiceTracker.WebApi.csproj" \
    --source https://api.nuget.org/v3/index.json

# Build the application
RUN dotnet build \
    "Presentation/InvoiceTracker.WebApi/InvoiceTracker.WebApi.csproj" \
    --configuration $BUILD_CONFIGURATION \
    --no-restore


# Publish image
FROM build AS publish

RUN dotnet publish \
    "Presentation/InvoiceTracker.WebApi/InvoiceTracker.WebApi.csproj" \
    --configuration $BUILD_CONFIGURATION \
    --output /app/out \
    --no-restore \
    /p:UseAppHost=false


# Final runtime image
FROM base AS final

WORKDIR /app

COPY --from=publish /app/out .

ENTRYPOINT ["dotnet", "InvoiceTracker.WebApi.dll"]