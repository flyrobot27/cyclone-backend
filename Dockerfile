FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
EXPOSE 8080

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /src
COPY ["CYCLONE.API/CYCLONE.API.csproj", "CYCLONE.API/"]
COPY ["CYCLONE.Template/CYCLONE.Template.csproj", "CYCLONE.Template/"]
COPY ["CYCLONE.Console.Test/CYCLONE.ConsoleApp.Test.csproj", "CYCLONE.Console.Test/"]
CMD bash
RUN dotnet restore "CYCLONE.API/CYCLONE.API.csproj"
COPY . .
WORKDIR "/src/CYCLONE.API"
RUN dotnet build "CYCLONE.API.csproj" -a $TARGETARCH -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CYCLONE.API.csproj" -a $TARGETARCH -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CYCLONE.API.dll"]
