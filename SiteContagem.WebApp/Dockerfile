FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SiteContagem.WebApp/SiteContagem.WebApp.csproj", "SiteContagem.WebApp/"]
RUN dotnet restore "SiteContagem.WebApp/SiteContagem.WebApp.csproj"
COPY . .
WORKDIR "/src/SiteContagem.WebApp"
RUN dotnet build "SiteContagem.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SiteContagem.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SiteContagem.WebApp.dll"]
