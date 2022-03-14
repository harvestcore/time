FROM mcr.microsoft.com/dotnet/sdk:3.1.417-alpine3.15 AS build
WORKDIR /source
COPY . .
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:3.1.23-alpine3.15
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "WorkTime.dll"]