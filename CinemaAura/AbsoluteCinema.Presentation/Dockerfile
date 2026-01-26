FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AbsoluteCinema.sln ./

COPY AbsoluteCinema.Presentation/AbsoluteCinema.Presentation.csproj AbsoluteCinema.Presentation/
COPY AbsoluteCinema.Application/AbsoluteCinema.Application.csproj AbsoluteCinema.Application/
COPY AbsoluteCinema.Domain/AbsoluteCinema.Domain.csproj AbsoluteCinema.Domain/
COPY AbsoluteCinema.Infrastructure/AbsoluteCinema.Infrastructure.csproj AbsoluteCinema.Infrastructure/

RUN dotnet restore AbsoluteCinema.Presentation/AbsoluteCinema.Presentation.csproj

COPY . .

WORKDIR /src/AbsoluteCinema.Presentation
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AbsoluteCinema.Presentation.dll"]