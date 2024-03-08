#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PixelTest/PixelTest.Track.Api.Host.csproj", "PixelTest/"]
COPY ["PixelTest.Core/PixelTest.Core.csproj", "PixelTest.Core/"]
COPY ["PixelTest.Track.Services/PixelTest.Track.Services.csproj", "PixelTest.Track.Services/"]
COPY ["PixelTest.Storage.Api.Host.Contracts/PixelTest.Storage.Api.Host.Contracts.csproj", "PixelTest.Storage.Api.Host.Contracts/"]
COPY ["PixelTest.Track.Services.Contracts/PixelTest.Track.Services.Contracts.csproj", "PixelTest.Track.Services.Contracts/"]
RUN dotnet restore "./PixelTest/./PixelTest.Track.Api.Host.csproj"
COPY . .
WORKDIR "/src/PixelTest"
RUN dotnet build "./PixelTest.Track.Api.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PixelTest.Track.Api.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PixelTest.Track.Api.Host.dll"]