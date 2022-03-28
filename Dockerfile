FROM mcr.microsoft.com/dotnet/sdk:6.0.201-alpine3.15 AS build
WORKDIR /source
COPY . .
RUN dotnet restore
RUN dotnet build --no-restore -c release
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0.3-alpine3.15
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "WorkTime.dll"]