#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/1-Applicaion/Frwk.Bootcamp.QuickWait.ProfileUsers.Api/Frwk.Bootcamp.QuickWait.ProfileUsers.Api.csproj", "src/1-Applicaion/Frwk.Bootcamp.QuickWait.ProfileUsers.Api/"]
COPY ["src/4-Infrastructure/Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure/Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure.csproj", "src/4-Infrastructure/Frwk.Bootcamp.QuickWait.ProfileUsers.Infrastructure/"]
COPY ["src/2-Domain/Frwk.Bootcamp.QuickWait.ProfileUsers.Domain/Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.csproj", "src/2-Domain/Frwk.Bootcamp.QuickWait.ProfileUsers.Domain/"]
COPY ["src/3-Services/Frwk.Bootcamp.QuickWait.ProfileUsers.Application/Frwk.Bootcamp.QuickWait.ProfileUsers.Application.csproj", "src/3-Services/Frwk.Bootcamp.QuickWait.ProfileUsers.Application/"]
RUN dotnet restore "src/1-Applicaion/Frwk.Bootcamp.QuickWait.ProfileUsers.Api/Frwk.Bootcamp.QuickWait.ProfileUsers.Api.csproj"
COPY . .
WORKDIR "/src/src/1-Applicaion/Frwk.Bootcamp.QuickWait.ProfileUsers.Api"
RUN dotnet build "Frwk.Bootcamp.QuickWait.ProfileUsers.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Frwk.Bootcamp.QuickWait.ProfileUsers.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frwk.Bootcamp.QuickWait.ProfileUsers.Api.dll"]