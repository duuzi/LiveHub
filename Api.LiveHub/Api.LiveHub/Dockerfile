FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

COPY . .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "Api.LiveHub.dll"]
