FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WireguardWeb.Api/WireguardWeb.Api.csproj", "WireguardWeb.Api/"]
RUN dotnet restore "WireguardWeb.Api/WireguardWeb.Api.csproj"
COPY . .
WORKDIR "/src/WireguardWeb.Api"
RUN dotnet build "WireguardWeb.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WireguardWeb.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WireguardWeb.Api.dll"]
