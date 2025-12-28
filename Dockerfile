FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Erkan_aktunc_web/Erkan_aktunc_web.csproj", "Erkan_aktunc_web/"]

RUN dotnet restore "Erkan_aktunc_web/Erkan_aktunc_web.csproj"

COPY . .

WORKDIR "/src/Erkan_aktunc_web"

RUN dotnet build "Erkan_aktunc_web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Erkan_aktunc_web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Erkan_aktunc_web.dll"]
