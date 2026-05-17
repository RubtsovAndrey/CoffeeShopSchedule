# 1. Берем за основу официальный Linux-образ
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env

# 2. Создаем папку /App внутри контейнера
WORKDIR /App

# 3. Копируем всё внутрь
COPY . ./

# 4. Явно указываем, ГДЕ именно лежат тесты
RUN dotnet test CoffeeShop.Tests/CoffeeShop.Tests.csproj