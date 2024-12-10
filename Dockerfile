# Use .NET 8.0 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use .NET 8.0 SDK for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AoiCryptoAPI.csproj", "./"]
RUN dotnet restore "AoiCryptoAPI.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "AoiCryptoAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "AoiCryptoAPI.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AoiCryptoAPI.dll"]
