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

COPY ["Kinematics-Simulator.API/Kinematics-Simulator.API.csproj", "Kinematics-Simulator.API/"]
COPY ["Kinematics-Simulator.Application/Kinematics-Simulator.Application.csproj", "Kinematics-Simulator.Application/"]
COPY ["Kinematics-Simulator.Domain/Kinematics-Simulator.Domain.csproj", "Kinematics-Simulator.Domain/"]
COPY ["Kinematics-Simulator.Infrastructure/Kinematics-Simulator.Infrastructure.csproj", "Kinematics-Simulator.Infrastructure/"]

RUN dotnet restore "./Kinematics-Simulator.API/Kinematics-Simulator.API.csproj"

COPY . .

WORKDIR "/src/Kinematics-Simulator.API"

RUN dotnet build "./Kinematics-Simulator.API.csproj" -c $BUILD_CONFIGURATION -o /app/build
#----- Ambiente di preparazione e compilazione -------------------------------

#----- Ambiente di publish ---------------------------------------------------
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "./Kinematics-Simulator.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#----- Ambiente di publish ---------------------------------------------------

#----- Ambiente finale -------------------------------------------------------
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Kinematics-Simulator.API.dll"]