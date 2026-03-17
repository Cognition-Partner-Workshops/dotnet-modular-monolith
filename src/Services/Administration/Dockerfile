FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files for restore
COPY ["Services/Administration/CompanyName.MyMeetings.Services.Administration.API/CompanyName.MyMeetings.Services.Administration.API.csproj", "Services/Administration/CompanyName.MyMeetings.Services.Administration.API/"]
COPY ["Modules/Administration/Application/CompanyName.MyMeetings.Modules.Administration.Application.csproj", "Modules/Administration/Application/"]
COPY ["Modules/Administration/Domain/CompanyName.MyMeetings.Modules.Administration.Domain.csproj", "Modules/Administration/Domain/"]
COPY ["Modules/Administration/Infrastructure/CompanyName.MyMeetings.Modules.Administration.Infrastructure.csproj", "Modules/Administration/Infrastructure/"]
COPY ["Modules/Administration/IntegrationEvents/CompanyName.MyMeetings.Modules.Administration.IntegrationEvents.csproj", "Modules/Administration/IntegrationEvents/"]
COPY ["Modules/Meetings/IntegrationEvents/CompanyName.MyMeetings.Modules.Meetings.IntegrationEvents.csproj", "Modules/Meetings/IntegrationEvents/"]
COPY ["Modules/Registrations/IntegrationEvents/CompanyName.MyMeetings.Modules.Registrations.IntegrationEvents.csproj", "Modules/Registrations/IntegrationEvents/"]
COPY ["Modules/UserAccess/IntegrationEvents/CompanyName.MyMeetings.Modules.UserAccess.IntegrationEvents.csproj", "Modules/UserAccess/IntegrationEvents/"]
COPY ["BuildingBlocks/Infrastructure/CompanyName.MyMeetings.BuildingBlocks.Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Domain/CompanyName.MyMeetings.BuildingBlocks.Domain.csproj", "BuildingBlocks/Domain/"]
COPY ["BuildingBlocks/Application/CompanyName.MyMeetings.BuildingBlocks.Application.csproj", "BuildingBlocks/Application/"]
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Directory.Build.targets", "Directory.Build.targets"]

RUN dotnet restore "Services/Administration/CompanyName.MyMeetings.Services.Administration.API/CompanyName.MyMeetings.Services.Administration.API.csproj"

COPY . .
WORKDIR "/src/Services/Administration/CompanyName.MyMeetings.Services.Administration.API"
RUN dotnet build "CompanyName.MyMeetings.Services.Administration.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CompanyName.MyMeetings.Services.Administration.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "CompanyName.MyMeetings.Services.Administration.API.dll"]
