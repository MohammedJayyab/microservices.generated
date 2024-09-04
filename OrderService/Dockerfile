# Use the official .NET Core SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container for building the app
WORKDIR /app

# Copy the csproj and restore any dependencies (via 'dotnet restore')
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application files
COPY . ./

# Build the app
RUN dotnet publish -c Release -o /out

# Use the official .NET Core runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container for running the app
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /out .

# Environment variables to configure RabbitMQ host and port
# Set RabbitMQ host to the container name 'rabbitmq' within the network
ENV RABBITMQ__HOST=rabbitmq
ENV RABBITMQ__PORT=5672

# Set the environment variable to configure Kestrel to listen on port 5090
ENV ASPNETCORE_URLS=http://+:5090

# Expose port 5090 for external access
EXPOSE 5090

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "OrderService.dll"]
