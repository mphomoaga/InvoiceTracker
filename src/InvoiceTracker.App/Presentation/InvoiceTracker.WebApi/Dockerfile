# Runtime base image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

# Configure ASP.NET Core to listen on all container interfaces
ENV ASPNETCORE_HTTP_PORTS=44358
ENV ASPNETCORE_URLS=http://+:44358

EXPOSE 44358


# Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY . .

# Restore NuGet packages
RUN dotnet restore \
    "Presentation/InvoiceTracker.WebApi/InvoiceTracker.WebApi.csproj" \
    --source "https://api.nuget.org/v3/index.json"


# Publish image
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish \
    "Presentation/InvoiceTracker.WebApi/InvoiceTracker.WebApi.csproj" \
    --configuration ${BUILD_CONFIGURATION} \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false


# Final runtime image
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

# Run as the built-in non-root .NET container user
USER $APP_UID

ENTRYPOINT ["dotnet", "InvoiceTracker.WebApi.dll"]