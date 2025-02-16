FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5008

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MerchShop.API/MerchShop.API.csproj", "MerchShop.API/"]
COPY ["MerchShop.Infrastructure/MerchShop.Infrastructure.csproj", "MerchShop.Infrastructure/"]
COPY ["MerchShop.Application/MerchShop.Application.csproj", "MerchShop.Application/"]
COPY ["MerchShop.Domain/MerchShop.Domain.csproj", "MerchShop.Domain/"] 
RUN dotnet restore "MerchShop.API/MerchShop.API.csproj"
COPY . .
WORKDIR "/src/MerchShop.API"
RUN dotnet build "MerchShop.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MerchShop.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN apt-get update && apt-get install -y postgresql-client

ENTRYPOINT ["dotnet", "MerchShop.API.dll"]