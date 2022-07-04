FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build-env
WORKDIR /app

COPY ./API/*.csproj ./
RUN dotnet restore
COPY ./API ./
RUN dotnet publish -c Development -o /app


FROM mcr.microsoft.com/dotnet/aspnet:3.1-focal
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "/app/bin/Development/netcoreapp3.1/API.dll"]
