FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PhotoAPI.sln" "."]
COPY ["./PhotoAPI/PhotoAPI.csproj", "./PhotoAPi/"]
COPY ["./PhotoAPITests/PhotoAPITests.csproj", "./PhotoAPITests/."]

RUN dotnet restore
COPY . .
WORKDIR "/src/."
RUN dotnet build

FROM build AS publish
RUN dotnet public -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publihs .
ENTRYPOINT [ "dotnet", "PhotoAPI.dll" ]
