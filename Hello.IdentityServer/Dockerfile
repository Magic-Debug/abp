FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM base AS final
COPY .
ENTRYPOINT ["dotnet", "Hello.IdentityServer.dll"]
