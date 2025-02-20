# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.sln .
COPY VatRegistration.Core/*.csproj ./VatRegistration.Core/
COPY VatRegistration.Infrastructure/*.csproj ./VatRegistration.Infrastructure/
COPY VatRegistration.API/*.csproj ./VatRegistration.API/
RUN dotnet restore

# Copy all files and build
COPY VatRegistration.Core/. ./VatRegistration.Core/
COPY VatRegistration.Infrastructure/. ./VatRegistration.Infrastructure/
COPY VatRegistration.API/. ./VatRegistration.API/
WORKDIR /app/VatRegistration.API
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# Configure for Azure App Service logs
ENV DOTNET_LOGGING__CONSOLE__DISABLECOLORS=true

ENTRYPOINT ["dotnet", "VatRegistration.API.dll"]