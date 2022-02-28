#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV RABBITMQ_HOST=rabbitmq
ENV RABBITMQ_PORT=31672

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["CartApi.csproj", "."]
RUN dotnet restore "./CartApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CartApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CartApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CartApi.dll"]