#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["e-Shop-Blogging/Volo.Blogging.App/Volo.Blogging.App.csproj", "e-Shop-Blogging/Volo.Blogging.App/"]
COPY ["framework/src/Volo.Abp.AutoMapper/Volo.Abp.AutoMapper.csproj", "framework/src/Volo.Abp.AutoMapper/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Admin.HttpApi/Volo.Blogging.Admin.HttpApi.csproj", "e-Shop-Blogging/src/Volo.Blogging.Admin.HttpApi/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Admin.Application.Contracts/Volo.Blogging.Admin.Application.Contracts.csproj", "e-Shop-Blogging/src/Volo.Blogging.Admin.Application.Contracts/"]
COPY ["e-Shop-Blogging/Volo.Blogging.App.EntityFrameworkCore/Volo.Blogging.App.EntityFrameworkCore.csproj", "e-Shop-Blogging/Volo.Blogging.App.EntityFrameworkCore/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.EntityFrameworkCore/Volo.Blogging.EntityFrameworkCore.csproj", "e-Shop-Blogging/src/Volo.Blogging.EntityFrameworkCore/"]
COPY ["modules/identity/src/Volo.Abp.Identity.Domain/Volo.Abp.Identity.Domain.csproj", "modules/identity/src/Volo.Abp.Identity.Domain/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Domain/Volo.Blogging.Domain.csproj", "e-Shop-Blogging/src/Volo.Blogging.Domain/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Domain.Shared/Volo.Blogging.Domain.Shared.csproj", "e-Shop-Blogging/src/Volo.Blogging.Domain.Shared/"]
COPY ["framework/src/Volo.Abp.Validation/Volo.Abp.Validation.csproj", "framework/src/Volo.Abp.Validation/"]
COPY ["framework/src/Volo.Abp.EntityFrameworkCore.SqlServer/Volo.Abp.EntityFrameworkCore.SqlServer.csproj", "framework/src/Volo.Abp.EntityFrameworkCore.SqlServer/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Admin.Web/Volo.Blogging.Admin.Web.csproj", "e-Shop-Blogging/src/Volo.Blogging.Admin.Web/"]
COPY ["framework/src/Volo.Abp.AspNetCore.Mvc.UI.Bundling/Volo.Abp.AspNetCore.Mvc.UI.Bundling.csproj", "framework/src/Volo.Abp.AspNetCore.Mvc.UI.Bundling/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.HttpApi/Volo.Blogging.HttpApi.csproj", "e-Shop-Blogging/src/Volo.Blogging.HttpApi/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Application/Volo.Blogging.Application.csproj", "e-Shop-Blogging/src/Volo.Blogging.Application/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Web/Volo.Blogging.Web.csproj", "e-Shop-Blogging/src/Volo.Blogging.Web/"]
COPY ["framework/src/Volo.Abp.AspNetCore.Mvc.UI.Widgets/Volo.Abp.AspNetCore.Mvc.UI.Widgets.csproj", "framework/src/Volo.Abp.AspNetCore.Mvc.UI.Widgets/"]
COPY ["framework/src/Volo.Abp.Caching.StackExchangeRedis/Volo.Abp.Caching.StackExchangeRedis.csproj", "framework/src/Volo.Abp.Caching.StackExchangeRedis/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Admin.Application/Volo.Blogging.Admin.Application.csproj", "e-Shop-Blogging/src/Volo.Blogging.Admin.Application/"]
COPY ["framework/src/Volo.Abp.Autofac/Volo.Abp.Autofac.csproj", "framework/src/Volo.Abp.Autofac/"]
RUN dotnet restore "e-Shop-Blogging/Volo.Blogging.App/Volo.Blogging.App.csproj"
COPY . .
WORKDIR "/src/e-Shop-Blogging/Volo.Blogging.App"
RUN dotnet build "Volo.Blogging.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Volo.Blogging.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Volo.Blogging.App.dll"]