# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
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
