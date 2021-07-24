#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Pokemon/Pokemon.csproj", "/Pokemon/Pokemon.csproj"]
COPY ["Pokemon.Services/Pokemon.Services.csproj", "/Pokemon.Services"]
RUN dotnet restore /Pokemon/Pokemon.csproj
COPY . .
WORKDIR "/src"
RUN dotnet build "Pokemon/Pokemon.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pokemon/Pokemon.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pokemon.dll"]