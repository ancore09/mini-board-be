# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /src

# Copy the entire source code first
COPY . .

# Restore dependencies
RUN dotnet restore

# Build the application
RUN dotnet build -c Release

# Publish the API project
RUN dotnet publish MiniBoard.Api/MiniBoard.Api.csproj -c Release -o /app/publish

# Use the official .NET 9.0 runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Expose port 8080
EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "MiniBoard.Api.dll"]