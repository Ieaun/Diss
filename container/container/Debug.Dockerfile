FROM mcr.microsoft.com/dotnet/aspnet:latest AS base
WORKDIR /app

EXPOSE 30000

COPY ["bin/Debug/net5.0","."]
ENTRYPOINT ["dotnet", "container.dll"]