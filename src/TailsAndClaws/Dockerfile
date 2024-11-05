FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

COPY ["src/TailsAndClaws/TailsAndClaws.csproj", "src/TailsAndClaws/"]
COPY ["src/TailsAndClaws.Application/TailsAndClaws.Application.csproj", "src/TailsAndClaws.Application/"]
COPY ["src/TailsAndClaws.Domain/TailsAndClaws.Domain.csproj", "src/TailsAndClaws.Domain/"]
COPY ["src/TailsAndClaws.Infrastructure/TailsAndClaws.Infrastructure.csproj", "src/TailsAndClaws.Infrastructure/"]
COPY ["src/AMSaiian.Shared.Web/AMSaiian.Shared.Web.csproj", "src/AMSaiian.Shared.Web/"]
COPY ["src/AMSaiian.Shared.Application/AMSaiian.Shared.Application.csproj", "src/AMSaiian.Shared.Application/"]
COPY ["src/AMSaiian.Shared.Infrastructure/AMSaiian.Shared.Infrastructure.csproj", "src/AMSaiian.Shared.Infrastructure/"]
COPY ["src/AMSaiian.Shared.Domain/AMSaiian.Shared.Domain.csproj", "src/AMSaiian.Shared.Domain/"]

RUN dotnet restore "src/TailsAndClaws/TailsAndClaws.csproj"
COPY . .
WORKDIR "/src/TailsAndClaws"
RUN dotnet build "TailsAndClaws.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS certs
ARG TC_CERT_PASSWORD

RUN mkdir -p /root/.aspnet/https && \
    dotnet dev-certs https -ep /root/.aspnet/https/tailsAndClaws.pfx -p ${TC_CERT_PASSWORD} && \
    dotnet dev-certs https --trust

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TailsAndClaws.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app

USER root
COPY --from=publish /app/publish .
COPY --from=certs /root/.aspnet/https/tailsAndClaws.pfx /https/tailsAndClaws.pfx

ENTRYPOINT ["dotnet", "TailsAndClaws.dll"]
