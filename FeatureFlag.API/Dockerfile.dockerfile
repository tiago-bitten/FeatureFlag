# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FeatureFlag.API/FeatureFlag.API.csproj", "FeatureFlag.API/"]
COPY ["FeatureFlag.Dominio/FeatureFlag.Dominio.csproj", "FeatureFlag.Dominio/"]
COPY ["FeatureFlag.Aplicacao/FeatureFlag.Aplicacao.csproj", "FeatureFlag.Aplicacao/"]
COPY ["FeatureFlag.Repositorio/FeatureFlag.Repositorio.csproj", "FeatureFlag.Repositorio/"]
COPY ["FeatureFlag.Shared/FeatureFlag.Shared.csproj", "FeatureFlag.Shared/"]
COPY ["FeatureFlag.Tests/FeatureFlag.Tests.csproj", "FeatureFlag.Tests/"]
RUN dotnet restore "FeatureFlag.API/FeatureFlag.API.csproj"
COPY . .
WORKDIR /src/FeatureFlag.API
RUN dotnet build -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "FeatureFlag.API.dll"]
