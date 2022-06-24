FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["DockerTest1/DockerTest1.csproj", "DockerTest1/"]
RUN dotnet restore "DockerTest1/DockerTest1.csproj"
COPY . .
WORKDIR "/src/DockerTest1"
RUN dotnet build "DockerTest1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerTest1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerTest1.dll"]