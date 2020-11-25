#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY LoadTest/source/LoadTest/LoadTest.csproj LoadTest/source/LoadTest/

RUN dotnet restore "LoadTest/source/LoadTest/LoadTest.csproj"
COPY . .
WORKDIR "/src/LoadTest/source/LoadTest"
RUN dotnet build "LoadTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoadTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoadTest.dll"]

