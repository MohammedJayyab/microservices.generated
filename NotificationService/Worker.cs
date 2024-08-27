using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationService Worker started and waiting for OrderPaymentProcessedCompleted events...");

            var factory = new ConnectionFactory() { HostName = "localhost" };

            // Create a persistent connection
            IConnection _connection = factory.CreateConnection();
            IModel _channel = _connection.CreateModel();

            // Declare the queue that this worker will consume
            _channel.QueueDeclare(queue: "orderPaymentProcessedCompletedQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"OrderPaymentProcessedCompleted event received: {message}");

                // Simulate sending a notification
                _logger.LogInformation($"Notification sent: Order with details: {message} has been successfully processed and paid.");
            };

            // Start consuming messages from the orderPaymentProcessedCompletedQueue
            _channel.BasicConsume(queue: "orderPaymentProcessedCompletedQueue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}