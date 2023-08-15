FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ProtectVpnWeb.Api/ProtectVpnWeb.Api.csproj", "ProtectVpnWeb.Api/"]
RUN dotnet restore "ProtectVpnWeb.Api/ProtectVpnWeb.Api.csproj"
COPY . .
WORKDIR "/src/ProtectVpnWeb.Api"
RUN dotnet build "ProtectVpnWeb.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProtectVpnWeb.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProtectVpnWeb.Api.dll"]
