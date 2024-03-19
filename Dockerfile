# Use the .NET SDK image to build the application for amd architecture
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln ./
COPY DataManager/*.csproj ./DataManager/
COPY TelegramBot/*.csproj ./TelegramBot/
RUN dotnet restore

# Copy the source code and build the application
COPY . .
RUN dotnet build -c Release -o out

# Build the final runtime image
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Set the entry point for the application
ENTRYPOINT ["dotnet", "TelegramBot.dll"]