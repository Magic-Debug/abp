#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Blogging.Client/Blogging.Client.csproj", "src/Blogging.Client/"]
RUN dotnet restore "src/Blogging.Client/Blogging.Client.csproj"
COPY . .
WORKDIR "/src/src/Blogging.Client"
RUN dotnet build "Blogging.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blogging.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blogging.Client.dll"]