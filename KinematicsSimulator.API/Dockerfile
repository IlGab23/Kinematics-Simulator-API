#----- Ambiente di esecuzione ------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

USER $APP_UID

WORKDIR /app

EXPOSE 8080
EXPOSE 8081
#----- Ambiente di esecuzione ------------------------------------------------

#----- Ambiente di preparazione e compilazione -------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["KinematicsSimulator.API/KinematicsSimulator.API.csproj", "KinematicsSimulator.API/"]
COPY ["KinematicsSimulator.Application/KinematicsSimulator.Application.csproj", "KinematicsSimulator.Application/"]
COPY ["KinematicsSimulator.Domain/KinematicsSimulator.Domain.csproj", "KinematicsSimulator.Domain/"]
COPY ["KinematicsSimulator.Infrastructure/KinematicsSimulator.Infrastructure.csproj", "KinematicsSimulator.Infrastructure/"]

RUN dotnet restore "./KinematicsSimulator.API/KinematicsSimulator.API.csproj"

COPY . .

WORKDIR "/src/KinematicsSimulator.API"

RUN dotnet build "./KinematicsSimulator.API.csproj" -c $BUILD_CONFIGURATION -o /app/build
#----- Ambiente di preparazione e compilazione -------------------------------

#----- Ambiente di publish ---------------------------------------------------
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "./KinematicsSimulator.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#----- Ambiente di publish ---------------------------------------------------

#----- Ambiente finale -------------------------------------------------------
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "KinematicsSimulator.API.dll"]