FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Orbit.Api/Orbit.Api.csproj", "Orbit.Api/"]
COPY ["Orbit.Application/Orbit.Application.csproj", "Orbit.Application/"]
COPY ["Orbit.Domain.Core/Orbit.Domain.Core.csproj", "Orbit.Domain.Core/"]
COPY ["Orbit.Infra.Persistence/Orbit.Infra.Persistence.csproj", "Orbit.Infra.Persistence/"]
COPY ["Orbit.Domain/Orbit.Domain.csproj", "Orbit.Domain/"]
COPY ["Orbit.Domain.Game/Orbit.Domain.Game.csproj", "Orbit.Domain.Game/"]
COPY ["Orbit.Infra.CrossCutting.IoC/Orbit.Infra.CrossCutting.IoC.csproj", "Orbit.Infra.CrossCutting.IoC/"]
COPY ["Orbit.Infra.FileUpload/Orbit.Infra.FileUpload.csproj", "Orbit.Infra.FileUpload/"]
COPY ["Orbit.Infra.CrossCutting.Identity/Orbit.Infra.CrossCutting.Identity.csproj", "Orbit.Infra.CrossCutting.Identity/"]
COPY ["Orbit.Infra.CrossCutting.Bus/Orbit.Infra.CrossCutting.Bus.csproj", "Orbit.Infra.CrossCutting.Bus/"]
RUN dotnet restore "Orbit.Api/Orbit.Api.csproj"
COPY . .
WORKDIR "/src/Orbit.Api"
RUN dotnet build "Orbit.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Orbit.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Orbit.Api.dll"]