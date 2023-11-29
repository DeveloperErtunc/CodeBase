#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CodeBase.API/CodeBase.API.csproj", "CodeBase.API/"]
COPY ["CodeBase.Services/CodeBase.Services.csproj", "CodeBase.Services/"]
COPY ["CodeBase.Helpers/CodeBase.Helpers.csproj", "CodeBase.Helpers/"]
COPY ["CodeBase.Models/CodeBase.Models.csproj", "CodeBase.Models/"]
RUN dotnet restore "CodeBase.API/CodeBase.API.csproj"
COPY . .
WORKDIR "/src/CodeBase.API"
RUN dotnet build "CodeBase.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeBase.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeBase.API.dll"]