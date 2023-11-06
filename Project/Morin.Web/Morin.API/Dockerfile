
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8989

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .

ENTRYPOINT ["dotnet", "Morin.API.dll"]

#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["Morin.API/Morin.API.csproj", "Morin.API/"]
#RUN dotnet restore "Morin.API/Morin.API.csproj"
#COPY . .
#WORKDIR "/src/Morin.API"
#RUN dotnet build "Morin.API.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Morin.API.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Morin.API.dll"]