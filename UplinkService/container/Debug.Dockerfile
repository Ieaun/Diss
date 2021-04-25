FROM mcr.microsoft.com/dotnet/aspnet:latest AS base
WORKDIR /app

EXPOSE 30003

COPY ["bin/Debug/net5.0","."]
ENTRYPOINT ["dotnet", "UplinkService.dll"]