using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase

{
    private ILogger<OrdersController> _logger;

    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
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
        var factory = new ConnectionFactory() { HostName = "localhost" };
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