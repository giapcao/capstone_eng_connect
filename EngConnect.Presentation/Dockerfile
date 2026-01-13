FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EngConnect.Presentation/EngConnect.Presentation.csproj", "EngConnect.Presentation/"]
COPY ["EngConnect.Application/EngConnect.Application.csproj", "EngConnect.Application/"]
COPY ["EngConnect.Domain/EngConnect.Domain.csproj", "EngConnect.Domain/"]
COPY ["EngConnect.BuildingBlocks/EngConnect.BuildingBlocks.csproj", "EngConnect.BuildingBlocks/"]
COPY ["EngConnect.Infrastructure/EngConnect.Infrastructure.csproj", "EngConnect.Infrastructure/"]
RUN dotnet restore "EngConnect.Presentation/EngConnect.Presentation.csproj"
COPY . .
WORKDIR "/src/EngConnect.Presentation"
RUN dotnet build "./EngConnect.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EngConnect.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

USER root
RUN apt-get update \
 && apt-get install -y --no-install-recommends curl \
 && rm -rf /var/lib/apt/lists/*
USER $APP_UID

COPY --from=publish /app/publish .
HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=3 \
  CMD curl -fsS http://127.0.0.1:8080/status || exit 1
ENTRYPOINT ["dotnet", "EngConnect.Presentation.dll"]
