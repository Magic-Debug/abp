FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 4371

#Docker中修改时区
RUN echo "Asia/Shanghai"> /etc/timezone
RUN apt-get install tzdata
RUN  dpkg-reconfigure -f noninteractive tzdata

FROM base AS final
COPY ./ ./
ENTRYPOINT ["dotnet", "Hello.IdentityServer.dll"]
