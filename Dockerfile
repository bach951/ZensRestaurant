## https://hub.docker.com/_/microsoft-dotnet
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /app
#
## copy csproj and restore as distinct layers
#COPY *.sln .
#COPY ZensRestaurant/*.csproj ./ZensRestaurant/
#COPY Repository/*.csproj ./Repository/
#COPY Service/*.csproj ./Service/
#RUN dotnet restore
#
## copy everything else and build app
#COPY /. ./app/
#WORKDIR /app
#RUN dotnet publish -c release -o /app --no-restore
#
## final stage/image
#FROM mcr.microsoft.com/dotnet/aspnet:6.0
#WORKDIR /app
#COPY --from=build /app ./
#ENTRYPOINT ["dotnet", "zensrestaurant.dll"]
#

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/."
RUN dotnet restore "./ZensRestaurant.sln"
RUN dotnet build "ZensRestaurant.sln" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/ZensRestaurant"
RUN dotnet publish "ZensRestaurant.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZensRestaurant.dll"]