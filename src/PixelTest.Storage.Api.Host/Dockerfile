#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PixelTest.Storage.Api.Host/PixelTest.Storage.Api.Host.csproj", "PixelTest.Storage.Api.Host/"]
COPY ["PixelTest.Core/PixelTest.Core.csproj", "PixelTest.Core/"]
COPY ["PixelTest.Storage.Api.Host.Contracts/PixelTest.Storage.Api.Host.Contracts.csproj", "PixelTest.Storage.Api.Host.Contracts/"]
COPY ["PixelTest.Storage.Services.Contracts/PixelTest.Storage.Services.Contracts.csproj", "PixelTest.Storage.Services.Contracts/"]
COPY ["PixelTest.Storage.Services/PixelTest.Storage.Services.csproj", "PixelTest.Storage.Services/"]
RUN dotnet restore "./PixelTest.Storage.Api.Host/./PixelTest.Storage.Api.Host.csproj"
COPY . .
WORKDIR "/src/PixelTest.Storage.Api.Host"
RUN dotnet build "./PixelTest.Storage.Api.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PixelTest.Storage.Api.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PixelTest.Storage.Api.Host.dll"]