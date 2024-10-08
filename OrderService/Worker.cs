﻿using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OrderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            _logger.LogInformation("Initializing RabbitMQ...");
            _logger.LogInformation($"RabbitMQ Host: {_configuration["RabbitMQ:Host"]}");
            _logger.LogInformation($"RabbitMQ Port: {_configuration["RabbitMQ:Port"]}");
            _logger.LogInformation($" ------- ");
            try
            {
                // Get RabbitMQ settings from configuration
                var rabbitMQHost = _configuration["RabbitMQ:Host"];
                var rabbitMQPort = int.Parse(_configuration["RabbitMQ:Port"]);

                var factory = new ConnectionFactory() { HostName = rabbitMQHost, Port = rabbitMQPort };

                // Create a persistent connection
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                // Declare the queue that this worker will consume
                _channel.QueueDeclare(queue: "orderCreatedQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error initializing RabbitMQ: {ex.Message}");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OrderService Worker started and waiting for OrderCreated events...");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"OrderCreated event received: {message}");

                // Simulate order processing
                var orderProcessed = ProcessOrder(message);

                if (orderProcessed)
                {
                    PublishOrderProcessedCompletedEvent(message);
                }
            };

            // Start consuming messages from the orderCreatedQueue
            _channel.BasicConsume(queue: "orderCreatedQueue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private bool ProcessOrder(string orderMessage)
        {
            _logger.LogInformation("Processing order...");
            Thread.Sleep(2000);  // Simulate time taken to process order
            _logger.LogInformation("Order processed successfully.");
            return true;  // Simulate successful order processing
        }

        private void PublishOrderProcessedCompletedEvent(string orderMessage)
        {
            var processedMessage = $"OrderProcessedCompleted: {orderMessage}";
            var body = Encoding.UTF8.GetBytes(processedMessage);

            // Declare the queue for the processed event
            _channel.QueueDeclare(queue: "orderProcessedCompletedQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicPublish(exchange: "", routingKey: "orderProcessedCompletedQueue", basicProperties: null, body: body);

            _logger.LogInformation($"OrderProcessedCompleted event published: {processedMessage}");
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}