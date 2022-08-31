#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5424
EXPOSE 5426

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["e-Shop-Blogging/Volo.Blogging.App/Volo.Blogging.App.csproj", "e-Shop-Blogging/Volo.Blogging.App/"]
COPY ["framework/src/Volo.Abp.AutoMapper/Volo.Abp.AutoMapper.csproj", "framework/src/Volo.Abp.AutoMapper/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Admin.HttpApi/Volo.Blogging.Admin.HttpApi.csproj", "e-Shop-Blogging/src/Volo.Blogging.Admin.HttpApi/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Admin.Application.Contracts/Volo.Blogging.Admin.Application.Contracts.csproj", "e-Shop-Blogging/src/Volo.Blogging.Admin.Application.Contracts/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Application.Contracts.Shared/Volo.Blogging.Application.Contracts.Shared.csproj", "e-Shop-Blogging/src/Volo.Blogging.Application.Contracts.Shared/"]
COPY ["framework/src/Volo.Abp.Localization.Abstractions/Volo.Abp.Localization.Abstractions.csproj", "framework/src/Volo.Abp.Localization.Abstractions/"]
COPY ["framework/src/Volo.Abp.Core/Volo.Abp.Core.csproj", "framework/src/Volo.Abp.Core/"]
COPY ["framework/src/Volo.Abp.VirtualFileSystem/Volo.Abp.VirtualFileSystem.csproj", "framework/src/Volo.Abp.VirtualFileSystem/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Domain.Shared/Volo.Blogging.Domain.Shared.csproj", "e-Shop-Blogging/src/Volo.Blogging.Domain.Shared/"]
COPY ["framework/src/Volo.Abp.Validation/Volo.Abp.Validation.csproj", "framework/src/Volo.Abp.Validation/"]
COPY ["framework/src/Volo.Abp.Localization/Volo.Abp.Localization.csproj", "framework/src/Volo.Abp.Localization/"]
COPY ["framework/src/Volo.Abp.Ddd.Application.Contracts/Volo.Abp.Ddd.Application.Contracts.csproj", "framework/src/Volo.Abp.Ddd.Application.Contracts/"]
COPY ["framework/src/Volo.Abp.Authorization.Abstractions/Volo.Abp.Authorization.Abstractions.csproj", "framework/src/Volo.Abp.Authorization.Abstractions/"]
COPY ["framework/src/Volo.Abp.MultiTenancy/Volo.Abp.MultiTenancy.csproj", "framework/src/Volo.Abp.MultiTenancy/"]
COPY ["framework/src/Volo.Abp.Security/Volo.Abp.Security.csproj", "framework/src/Volo.Abp.Security/"]
COPY ["framework/src/Volo.Abp.Data/Volo.Abp.Data.csproj", "framework/src/Volo.Abp.Data/"]
COPY ["framework/src/Volo.Abp/Volo.Abp.csproj", "framework/src/Volo.Abp/"]
COPY ["framework/src/Volo.Abp.Authorization/Volo.Abp.Authorization.csproj", "framework/src/Volo.Abp.Authorization/"]
COPY ["framework/src/Volo.Abp.AspNetCore.SignalR/Volo.Abp.AspNetCore.SignalR.csproj", "framework/src/Volo.Abp.AspNetCore.SignalR/"]
COPY ["framework/src/Volo.Abp.AspNetCore/Volo.Abp.AspNetCore.csproj", "framework/src/Volo.Abp.AspNetCore/"]
COPY ["e-Shop-Blogging/Volo.Blogging.App.EntityFrameworkCore/Volo.Blogging.App.EntityFrameworkCore.csproj", "e-Shop-Blogging/Volo.Blogging.App.EntityFrameworkCore/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.EntityFrameworkCore/Volo.Blogging.EntityFrameworkCore.csproj", "e-Shop-Blogging/src/Volo.Blogging.EntityFrameworkCore/"]
COPY ["framework/src/Volo.Abp.EntityFrameworkCore.MySQL/Volo.Abp.EntityFrameworkCore.MySQL.csproj", "framework/src/Volo.Abp.EntityFrameworkCore.MySQL/"]
COPY ["modules/identity/src/Volo.Abp.Identity.Domain/Volo.Abp.Identity.Domain.csproj", "modules/identity/src/Volo.Abp.Identity.Domain/"]
COPY ["framework/src/Volo.Abp.Ddd.Domain/Volo.Abp.Ddd.Domain.csproj", "framework/src/Volo.Abp.Ddd.Domain/"]
COPY ["framework/src/Volo.Abp.EventBus/Volo.Abp.EventBus.csproj", "framework/src/Volo.Abp.EventBus/"]
COPY ["framework/src/Volo.Abp.BackgroundWorkers/Volo.Abp.BackgroundWorkers.csproj", "framework/src/Volo.Abp.BackgroundWorkers/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Domain/Volo.Blogging.Domain.csproj", "e-Shop-Blogging/src/Volo.Blogging.Domain/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Admin.Web/Volo.Blogging.Admin.Web.csproj", "e-Shop-Blogging/src/Volo.Blogging.Admin.Web/"]
COPY ["framework/src/Volo.Abp.AspNetCore.Mvc.UI.Bundling/Volo.Abp.AspNetCore.Mvc.UI.Bundling.csproj", "framework/src/Volo.Abp.AspNetCore.Mvc.UI.Bundling/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.HttpApi/Volo.Blogging.HttpApi.csproj", "e-Shop-Blogging/src/Volo.Blogging.HttpApi/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Application.Contracts/Volo.Blogging.Application.Contracts.csproj", "e-Shop-Blogging/src/Volo.Blogging.Application.Contracts/"]
COPY ["framework/src/Volo.Abp.Ddd.Application/Volo.Abp.Ddd.Application.csproj", "framework/src/Volo.Abp.Ddd.Application/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Application/Volo.Blogging.Application.csproj", "e-Shop-Blogging/src/Volo.Blogging.Application/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Web/Volo.Blogging.Web.csproj", "e-Shop-Blogging/src/Volo.Blogging.Web/"]
COPY ["framework/src/Volo.Abp.AspNetCore.Mvc.UI.Widgets/Volo.Abp.AspNetCore.Mvc.UI.Widgets.csproj", "framework/src/Volo.Abp.AspNetCore.Mvc.UI.Widgets/"]
COPY ["framework/src/Volo.Abp.Caching.StackExchangeRedis/Volo.Abp.Caching.StackExchangeRedis.csproj", "framework/src/Volo.Abp.Caching.StackExchangeRedis/"]
COPY ["e-Shop-Blogging/src/Volo.Blogging.Admin.Application/Volo.Blogging.Admin.Application.csproj", "e-Shop-Blogging/src/Volo.Blogging.Admin.Application/"]
COPY ["framework/src/Volo.Abp.Autofac/Volo.Abp.Autofac.csproj", "framework/src/Volo.Abp.Autofac/"]
COPY ["framework/src/Volo.Abp.Castle.Core/Volo.Abp.Castle.Core.csproj", "framework/src/Volo.Abp.Castle.Core/"]
COPY ["framework/src/Volo.Abp.BlobStoring.Minio/Volo.Abp.BlobStoring.Minio.csproj", "framework/src/Volo.Abp.BlobStoring.Minio/"]
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