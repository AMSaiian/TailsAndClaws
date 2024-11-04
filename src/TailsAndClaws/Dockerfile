FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TailsAndClaws/TailsAndClaws.csproj", "src/TailsAndClaws/"]
RUN dotnet restore "src/TailsAndClaws/TailsAndClaws.csproj"
COPY . .
WORKDIR "/src/src/TailsAndClaws"
RUN dotnet build "TailsAndClaws.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS certs
ARG TC_CERT_PASSWORD

RUN mkdir -p /root/.aspnet/https && \
    dotnet dev-certs https -ep /root/.aspnet/https/tailsAndClaws.pfx -p ${TC_CERT_PASSWORD} && \
    dotnet dev-certs https --trust

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TailsAndClaws.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

USER root
COPY --from=publish /app/publish .
COPY --from=certs /root/.aspnet/https/tailsAndClaws.pfx /https/tailsAndClaws.pfx

ENTRYPOINT ["dotnet", "TailsAndClaws.dll"]
