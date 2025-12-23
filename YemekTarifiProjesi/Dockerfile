# Build aþamasý
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Klasör yapýsýndan baðýmsýz olarak csproj dosyasýný kopyala
COPY **/*.csproj ./
RUN dotnet restore

# Tüm dosyalarý kopyala ve yayýnla
COPY . .
RUN dotnet publish -c Release -o out

# Çalýþtýrma aþamasý
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Projenin DLL adýný buraya tam yazmalýsýn
ENTRYPOINT ["dotnet", "YemekTarifiProjesi.dll"]