FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AirportDistances.HOST/AirportDistances.HOST.csproj", "AirportDistances.HOST/"]
COPY ["AirportDistances.Business/AirportDistances.Business.csproj", "AirportDistances.Business/"]
COPY ["AirportDistances.Infrastructure/AirportDistances.Infrastructure.csproj", "AirportDistances.Infrastructure/"]
RUN dotnet restore "AirportDistances.HOST/AirportDistances.HOST.csproj"
COPY . .
WORKDIR "/src/AirportDistances.HOST"
RUN dotnet build "AirportDistances.HOST.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AirportDistances.HOST.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AirportDistances.HOST.dll"]
