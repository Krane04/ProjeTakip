# 1. Aşama: Build (Derleme)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyasını kopyala ve restore et
COPY ["Erkan_aktunc_web.csproj", "./"]
RUN dotnet restore "Erkan_aktunc_web.csproj"

# Diğer dosyaları kopyala
COPY . .
WORKDIR "/src/."
RUN dotnet build "Erkan_aktunc_web.csproj" -c Release -o /app/build

# Yayınla (Publish)
FROM build AS publish
RUN dotnet publish "Erkan_aktunc_web.csproj" -c Release -o /app/publish

# 2. Aşama: Run (Çalıştırma)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Render port ayarı
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Erkan_aktunc_web.dll"]
