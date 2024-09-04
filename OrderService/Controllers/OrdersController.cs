using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IConfiguration _configuration;

    public OrdersController(ILogger<OrdersController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost]
    public IActionResult PlaceOrder([FromBody] Order order)
    {
        _logger.LogInformation($" ****** Order received: {order}");
        // Simulate saving the order to a database
        SaveOrder(order);

        // Publish an OrderCreated event
        PublishOrderCreatedEvent(order);

        return Ok(order);
    }

    private void SaveOrder(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order));
        }
        else
        {
            Console.WriteLine($"Order saved to Dbase: {order}");
        }
    }

    private void PublishOrderCreatedEvent(Order order)
    {
        // Get RabbitMQ settings from configuration
        var rabbitMQHost = _configuration["RabbitMQ:Host"];
        var rabbitMQPort = int.Parse(_configuration["RabbitMQ:Port"]);

        var factory = new ConnectionFactory() { HostName = rabbitMQHost, Port = rabbitMQPort };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "orderCreatedQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var json = JsonSerializer.Serialize(order);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: "orderCreatedQueue", basicProperties: null, body: body);

        Console.WriteLine($"orderCreatedQueue event published: {json}");

        _logger.LogInformation($"==> orderCreatedQueue event published: {json}");
    }
}