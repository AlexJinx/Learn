FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

WORKDIR /app

EXPOSE 5100

COPY .  /app

ENTRYPOINT ["dotnet", "LBJ.MicroService.APIGateway.dll"]