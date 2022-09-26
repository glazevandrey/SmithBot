#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR usr/src/app
EXPOSE 85
EXPOSE 444

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR usr/src
COPY ["SmithBot/SmithBot.csproj", "SmithBot/"]
RUN dotnet restore "SmithBot/SmithBot.csproj"
COPY . .
WORKDIR "usr/src/SmithBot"
RUN dotnet build "SmithBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmithBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR usr/app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmithBot.dll"]