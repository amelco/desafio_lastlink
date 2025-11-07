# FROM mcr.microsoft.com/dotnet/aspnet:8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY ["./desafio.csproj", "."]
RUN dotnet restore desafio.csproj

COPY . .

RUN dotnet build desafio.csproj -c Release -o /app/build
RUN dotnet publish desafio.csproj -c Release -o /app/publish
