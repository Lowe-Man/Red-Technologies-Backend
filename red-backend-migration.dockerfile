FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine3.16 AS build-env
WORKDIR /app

COPY ./API/*.csproj ./
COPY ./entrypoint.sh ./
RUN dotnet restore
RUN dotnet tool install --global dotnet-ef --version 3.1.26
RUN dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.1.26

COPY ./API/ ./
ENTRYPOINT ["sh", "entrypoint.sh"]
