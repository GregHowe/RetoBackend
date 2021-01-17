#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RetoBackend.Api/RetoBackend.Api.csproj", "RetoBackend.Api/"]
RUN dotnet restore "RetoBackend.Api/RetoBackend.Api.csproj"
COPY . .
WORKDIR "/src/RetoBackend.Api"
RUN dotnet build "RetoBackend.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RetoBackend.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RetoBackend.Api.dll"]
