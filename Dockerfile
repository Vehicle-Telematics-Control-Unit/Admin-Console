#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

FROM registry.digitalocean.com/vehicle-plus/aspnet-angular-builder:latest AS build
WORKDIR /src
COPY ["Admin-Console.csproj", "."]
RUN dotnet restore "./Admin-Console.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Admin-Console.csproj" -c Release -o /app/build

RUN dotnet publish "Admin-Console.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .