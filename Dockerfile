FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PhotoAPI.sln", "."]
COPY ["./PhotoAPI/PhotoAPI.csproj", "./PhotoAPI/"]
COPY ["./PhotoAPITests/PhotoAPITests.csproj", "./PhotoAPITests/"]

RUN dotnet restore
COPY . .
WORKDIR "/src/."
RUN dotnet build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "PhotoAPI.dll" ]
