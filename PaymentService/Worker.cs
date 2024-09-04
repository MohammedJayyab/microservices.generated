using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PaymentService;

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
            _channel.QueueDeclare(queue: "orderProcessedCompletedQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error initializing RabbitMQ: {ex.Message}");
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PaymentService Worker started and waiting for OrderProcessedCompleted events...");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"OrderProcessedCompleted event received: {message}");

            // Simulate payment processing
            var paymentProcessed = ProcessPayment(message);

            if (paymentProcessed)
            {
                PublishOrderPaymentProcessedCompletedEvent(message);
            }
        };

        // Start consuming messages from the orderProcessedCompletedQueue
        _channel.BasicConsume(queue: "orderProcessedCompletedQueue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    private bool ProcessPayment(string orderMessage)
    {
        _logger.LogInformation("Processing payment...");
        Thread.Sleep(2000);  // Simulate time taken to process payment
        _logger.LogInformation("Payment processed successfully.");

        return true;  // Simulate a successful payment processing
    }

    private void PublishOrderPaymentProcessedCompletedEvent(string orderMessage)
    {
        var paymentMessage = $"OrderPaymentProcessedCompleted for order: {orderMessage}";
        var body = Encoding.UTF8.GetBytes(paymentMessage);

        // Declare the queue for the processed event
        _channel.QueueDeclare(queue: "orderPaymentProcessedCompletedQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.BasicPublish(exchange: "", routingKey: "orderPaymentProcessedCompletedQueue", basicProperties: null, body: body);

        _logger.LogInformation("OrderPaymentProcessedCompleted event published.");
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}