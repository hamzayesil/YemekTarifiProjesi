# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Klasör yapısından bağımsız olarak csproj dosyasını kopyala
COPY **/*.csproj ./
RUN dotnet restore

# Tüm dosyaları kopyala ve yayınla
COPY . .
RUN dotnet publish -c Release -o out

# Çalıştırma aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Projenin DLL adını buraya tam yazmalısın
ENTRYPOINT ["dotnet", "YemekTarifiProjesi.dll"]
