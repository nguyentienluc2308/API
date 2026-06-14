# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy csproj and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy all source files and build
COPY . .
RUN dotnet publish -c Release -o /app

# Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Expose default HTTP ports
EXPOSE 80
EXPOSE 5115

ENV ASPNETCORE_URLS=http://+:5115
ENTRYPOINT ["dotnet", "CourseCatalog.Api.dll"]
