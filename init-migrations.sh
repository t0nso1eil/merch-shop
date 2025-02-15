#!/bin/bash

echo "Waiting for PostgreSQL to be ready..."
until pg_isready -h postgres -p 5432; do
  sleep 2
done

echo "Applying migrations..."
dotnet ef migrations add init --startup-project MerchShop.API --project MerchShop.Infrastructure
dotnet ef database update --startup-project MerchShop.API --project MerchShop.Infrastructure

echo "Starting the application..."
dotnet MerchShop.API.dll
