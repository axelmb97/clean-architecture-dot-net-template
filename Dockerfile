FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base 
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 
ARG BUILD_CONFIG=Release
WORKDIR /src

COPY ["CleanArchitecture.Template.sln", "./"]

COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infraestructure/Infraestructure.csproj", "Infraestructure/"]
COPY ["Presentation/Presentation.csproj", "Presentation/"]

RUN dotnet restore "Presentation/Presentation.csproj"

COPY . .

WORKDIR "/src/Presentation"
RUN dotnet build Presentation.csproj -c $BUILD_CONFIG -o /app/build

FROM build AS publish
ARG BUILD_CONFIG=Release
RUN dotnet publish Presentation.csproj -c $BUILD_CONFIG -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]