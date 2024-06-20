#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV CONNECTIONSTRINGS_DBPROJECT=""
ENV JWT_ISSUER="CovidLitSearch"
ENV JWT_AUDIENCE="User"
ENV JWT_SECRETKEY=""
ENV SMTP_HOST=""
ENV SMTP_PORT=-1
ENV SMTP_USERNAME=""
ENV SMTP_PASSWORD=""

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CovidLitSearch/CovidLitSearch.csproj", "CovidLitSearch/"]
RUN dotnet restore "./CovidLitSearch/CovidLitSearch.csproj"
COPY . .
WORKDIR "/src/CovidLitSearch"
RUN dotnet build "./CovidLitSearch.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CovidLitSearch.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["sh", "-c", "dotnet CovidLitSearch.dll ConnectionStrings:DBProject=$CONNECTIONSTRINGS_DBPROJECT Jwt:Issuer=$JWT_ISSUER Jwt:Audience=$JWT_AUDIENCE Jwt:Secret=$JWT_SECRETKEY Smtp:Host=$SMTP_HOST Smtp:Port=$SMTP_PORT Smtp:Username=$SMTP_USERNAME Smtp:Password=$SMTP_PASSWORD"]
