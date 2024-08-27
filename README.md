# Microservices Architecture with .NET and RabbitMQ

This repository demonstrates a simple microservices architecture using .NET, RabbitMQ, and Docker. The system consists of three main services: `OrderService`, `PaymentService`, and `NotificationService`. These services communicate with each other using RabbitMQ as the message broker.

## Services Overview

1. **OrderService**
   - Handles order creation.
   - Listens for `OrderCreated` events and processes the order.
   - Publishes `OrderProcessedCompleted` events.

2. **PaymentService**
   - Listens for `OrderProcessedCompleted` events.
   - Processes payments and publishes `OrderPaymentProcessedCompleted` events.

3. **NotificationService**
   - Listens for `OrderPaymentProcessedCompleted` events.
   - Sends a notification (logs a message) indicating that the order has been successfully processed and paid.

## Prerequisites

- [.NET SDK 6.x](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker](https://www.docker.com/get-started)

## Getting Started
```bash
 1. Clone the Repository


git clone https://github.com/yourusername/microservices-architecture.git
cd microservices-architecture

# 2. Install and Run RabbitMQ with Docker
docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management

# 3. Running the Services
cd OrderService
dotnet run


cd PaymentService
dotnet run

cd NotificationService
dotnet run

# 4. Testing:
curl -X POST http://localhost:5000/api/orders \
-H "Content-Type: application/json" \
-d '{
  "OrderId": "1234",
  "ProductName": "Laptop",
  "Quantity": 1,
  "Price": 1500
}'
