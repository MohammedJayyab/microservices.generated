# Microservices Architecture with .NET and RabbitMQ

Welcome to this guide on setting up a simple microservices architecture using .NET, RabbitMQ, and Docker. In this tutorial, you'll learn how to set up and run a system composed of three main services: `OrderService`, `PaymentService`, and `NotificationService`. These services will communicate with each other using RabbitMQ as the message broker.

# Scene 1: Introduction

Our architecture is made up of three services:

- **OrderService:** Handles order creation.
  - Listens for `OrderCreated` events.
  - Processes the order and publishes `OrderProcessedCompleted` events.

- **PaymentService:** Manages payment processing.
  - Listens for `OrderProcessedCompleted` events.
  - Processes payments and publishes `OrderPaymentProcessedCompleted` events.

- **NotificationService:** Sends notifications once orders are processed and paid.
  - Listens for `OrderPaymentProcessedCompleted` events.
  - Logs a message confirming successful order processing and payment.

# Scene 2: Prerequisites

Before we get started, make sure you have the following installed:

- [.NET SDK 6.x](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker](https://www.docker.com/get-started)

# Scene 3: Getting Started

Follow these steps to set up and run the services:

## 1. Clone the Repository

```bash
git clone https://github.com/yourusername/microservices-architecture.git
cd microservices-architecture
```
## 2. Install and Run RabbitMQ with Docker
To set up RabbitMQ, run it in a Docker container with the following command:

```bash
docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management

```
## 3. Running the Services

```bash
cd OrderService
dotnet run

cd PaymentService
dotnet run

cd NotificationService
dotnet run
```

## 4. Testing using  Postman
```bash
   curl -X POST http://localhost:5000/api/orders \
-H "Content-Type: application/json" \
-d '{
  "OrderId": "1234",
  "ProductName": "Laptop",
  "Quantity": 1,
  "Price": 1500
}'

```


