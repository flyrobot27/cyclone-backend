FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CYCLONE.API/CYCLONE.API.csproj", "CYCLONE.API/"]
COPY ["CYCLONE.API/CYCLONE.Template.csproj", "CYCLONE.Template/"]
COPY ["CYCLONE.API/CYCLONE.Console.Test.csproj", "CYCLONE.Console.Test/"]
RUN dotnet restore "CYCLONE.API/CYCLONE.API.csproj"
COPY . .
WORKDIR "/src/CYCLONE.API"
RUN dotnet build "CYCLONE.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CYCLONE.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CYCLONE.API.dll"]
