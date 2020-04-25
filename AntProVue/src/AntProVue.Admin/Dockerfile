FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/AntProVue.Admin/AntProVue.Admin.csproj", "src/AntProVue.Admin/"]
COPY ["src/AntProVue.Admin.EntityFramework.Shared/AntProVue.Admin.EntityFramework.Shared.csproj", "src/AntProVue.Admin.EntityFramework.Shared/"]
COPY ["src/AntProVue.Admin.EntityFramework.SqlServer/AntProVue.Admin.EntityFramework.SqlServer.csproj", "src/AntProVue.Admin.EntityFramework.SqlServer/"]
COPY ["src/AntProVue.Admin.EntityFramework.PostgreSQL/AntProVue.Admin.EntityFramework.PostgreSQL.csproj", "src/AntProVue.Admin.EntityFramework.PostgreSQL/"]
COPY ["src/AntProVue.Admin.EntityFramework.MySql/AntProVue.Admin.EntityFramework.MySql.csproj", "src/AntProVue.Admin.EntityFramework.MySql/"]
RUN dotnet restore "src/AntProVue.Admin/AntProVue.Admin.csproj"
COPY . .
WORKDIR "/src/src/AntProVue.Admin"
RUN dotnet build "AntProVue.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AntProVue.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "AntProVue.Admin.dll"]