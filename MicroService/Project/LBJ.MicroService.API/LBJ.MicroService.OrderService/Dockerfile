FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

WORKDIR /app

EXPOSE 7000-8000

COPY .  /app

ENTRYPOINT ["dotnet", "LBJ.MicroService.OrderService.dll"]