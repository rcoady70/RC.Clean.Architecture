#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Get base sdk .net core 6 runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy project files 
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RC.CA.Notification.WebApi/RC.CA.Notification.WebApi.csproj", "RC.CA.Notification.WebApi/"]
COPY ["RC.CA.Infrastructure.MessageBus/RC.CA.Infrastructure.MessageBus.csproj", "RC.CA.Infrastructure.MessageBus/"]
COPY ["RC.CA.SharedKernel/RC.CA.SharedKernel.csproj", "RC.CA.SharedKernel/"]

# Restore any dependencies
RUN dotnet restore "RC.CA.Notification.WebApi/RC.CA.Notification.WebApi.csproj"
COPY . .
WORKDIR "/src/RC.CA.Notification.WebApi"

# Build project
RUN dotnet build "RC.CA.Notification.WebApi.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "RC.CA.Notification.WebApi.csproj" -c Release -o /app/publish

# Generate runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# How to start run dot net with dll RC.CA.Notification.WebApi.dll 
ENTRYPOINT ["dotnet", "RC.CA.Notification.WebApi.dll"]