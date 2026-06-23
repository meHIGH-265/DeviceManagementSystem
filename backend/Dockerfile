FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

WORKDIR /src/DeviceManagementSystem

RUN dotnet restore DeviceManagementSystem.slnx
RUN dotnet publish DeviceManagementSystem.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app .

EXPOSE 8080

ENTRYPOINT ["dotnet", "DeviceManagementSystem.dll"]