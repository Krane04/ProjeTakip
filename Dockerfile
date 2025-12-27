FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. DEĞİŞİKLİK: Klasör adını ekle
COPY ["Erkan_aktunc_web/Erkan_aktunc_web.csproj", "Erkan_aktunc_web/"]

# 2. DEĞİŞİKLİK: Klasör adını buraya da ekle
RUN dotnet restore "Erkan_aktunc_web/Erkan_aktunc_web.csproj"

COPY . .

# 3. DEĞİŞİKLİK: Çalışma dizinini o klasörün içine kaydırıyoruz!
WORKDIR "/src/Erkan_aktunc_web"

# 4. DEĞİŞİKLİK: Artık klasördeyiz, dosya adını direkt yazabiliriz ama garanti olsun
RUN dotnet build "Erkan_aktunc_web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Erkan_aktunc_web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Erkan_aktunc_web.dll"]
