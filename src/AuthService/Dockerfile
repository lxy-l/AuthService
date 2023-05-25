#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
ARG ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT=Production
COPY .  /app
ENTRYPOINT ["dotnet", "AuthService.dll"]