FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["DotNetCoreWebApi.csproj", "./DotNetCoreWebApi/"]
RUN dotnet restore "DotNetCoreWebApi/DotNetCoreWebApi.csproj"
COPY . .
WORKDIR /src/DotNetCoreWebApi
RUN dotnet build "DotNetCoreWebApi.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "DotNetCoreWebApi.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetCoreWebApi.dll"]