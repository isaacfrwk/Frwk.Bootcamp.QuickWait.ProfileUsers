#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Frwk.Bootcamp.QuickWait.ProfileUsers.Api/Frwk.Bootcamp.QuickWait.ProfileUsers.Api.csproj", "src/Frwk.Bootcamp.QuickWait.ProfileUsers.Api/"]
RUN dotnet restore "src/Frwk.Bootcamp.QuickWait.ProfileUsers.Api/Frwk.Bootcamp.QuickWait.ProfileUsers.Api.csproj"
COPY . .
WORKDIR "/src/src/Frwk.Bootcamp.QuickWait.ProfileUsers.Api"
RUN dotnet build "Frwk.Bootcamp.QuickWait.ProfileUsers.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Frwk.Bootcamp.QuickWait.ProfileUsers.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frwk.Bootcamp.QuickWait.ProfileUsers.Api.dll"]