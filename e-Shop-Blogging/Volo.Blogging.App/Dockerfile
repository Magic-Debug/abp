#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["blogging/Volo.Blogging.App/Volo.Blogging.App.csproj", "blogging/Volo.Blogging.App/"]
COPY ["framework/src/Volo.Abp.AutoMapper/Volo.Abp.AutoMapper.csproj", "framework/src/Volo.Abp.AutoMapper/"]
COPY ["blogging/src/Volo.Blogging.Admin.HttpApi/Volo.Blogging.Admin.HttpApi.csproj", "blogging/src/Volo.Blogging.Admin.HttpApi/"]
COPY ["blogging/src/Volo.Blogging.Admin.Application.Contracts/Volo.Blogging.Admin.Application.Contracts.csproj", "blogging/src/Volo.Blogging.Admin.Application.Contracts/"]
COPY ["blogging/src/Volo.Blogging.Application.Contracts.Shared/Volo.Blogging.Application.Contracts.Shared.csproj", "blogging/src/Volo.Blogging.Application.Contracts.Shared/"]
COPY ["blogging/src/Volo.Blogging.Domain.Shared/Volo.Blogging.Domain.Shared.csproj", "blogging/src/Volo.Blogging.Domain.Shared/"]
COPY ["framework/src/Volo.Abp.Validation/Volo.Abp.Validation.csproj", "framework/src/Volo.Abp.Validation/"]
COPY ["framework/src/Volo.Abp/Volo.Abp.csproj", "framework/src/Volo.Abp/"]
COPY ["blogging/Volo.Blogging.App.EntityFrameworkCore/Volo.Blogging.App.EntityFrameworkCore.csproj", "blogging/Volo.Blogging.App.EntityFrameworkCore/"]
COPY ["blogging/src/Volo.Blogging.EntityFrameworkCore/Volo.Blogging.EntityFrameworkCore.csproj", "blogging/src/Volo.Blogging.EntityFrameworkCore/"]
COPY ["modules/identity/src/Volo.Abp.Identity.Domain/Volo.Abp.Identity.Domain.csproj", "modules/identity/src/Volo.Abp.Identity.Domain/"]
COPY ["blogging/src/Volo.Blogging.Domain/Volo.Blogging.Domain.csproj", "blogging/src/Volo.Blogging.Domain/"]
COPY ["framework/src/Volo.Abp.EntityFrameworkCore.SqlServer/Volo.Abp.EntityFrameworkCore.SqlServer.csproj", "framework/src/Volo.Abp.EntityFrameworkCore.SqlServer/"]
COPY ["blogging/src/Volo.Blogging.Admin.Web/Volo.Blogging.Admin.Web.csproj", "blogging/src/Volo.Blogging.Admin.Web/"]
COPY ["framework/src/Volo.Abp.AspNetCore.Mvc.UI.Bundling/Volo.Abp.AspNetCore.Mvc.UI.Bundling.csproj", "framework/src/Volo.Abp.AspNetCore.Mvc.UI.Bundling/"]
COPY ["blogging/src/Volo.Blogging.HttpApi/Volo.Blogging.HttpApi.csproj", "blogging/src/Volo.Blogging.HttpApi/"]
COPY ["blogging/src/Volo.Blogging.Application.Contracts/Volo.Blogging.Application.Contracts.csproj", "blogging/src/Volo.Blogging.Application.Contracts/"]
COPY ["blogging/src/Volo.Blogging.Application/Volo.Blogging.Application.csproj", "blogging/src/Volo.Blogging.Application/"]
COPY ["blogging/src/Volo.Blogging.Web/Volo.Blogging.Web.csproj", "blogging/src/Volo.Blogging.Web/"]
COPY ["framework/src/Volo.Abp.AspNetCore.Mvc.UI.Widgets/Volo.Abp.AspNetCore.Mvc.UI.Widgets.csproj", "framework/src/Volo.Abp.AspNetCore.Mvc.UI.Widgets/"]
COPY ["blogging/src/Volo.Blogging.Admin.Application/Volo.Blogging.Admin.Application.csproj", "blogging/src/Volo.Blogging.Admin.Application/"]
COPY ["framework/src/Volo.Abp.Autofac/Volo.Abp.Autofac.csproj", "framework/src/Volo.Abp.Autofac/"]
RUN dotnet restore "blogging/Volo.Blogging.App/Volo.Blogging.App.csproj"
COPY . .
WORKDIR "/src/blogging/Volo.Blogging.App"
RUN dotnet build "Volo.Blogging.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Volo.Blogging.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Volo.Blogging.App.dll"]