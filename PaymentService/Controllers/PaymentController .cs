using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(ILogger<PaymentController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest paymentRequest)
    {
        // Simulate payment processing
        bool paymentSuccess = await SimulatePaymentProcessing(paymentRequest);

        if (paymentSuccess)
        {
            _logger.LogInformation($" ** ***  ***  ********** Payment for OrderId {paymentRequest.OrderId} processed successfully.");
            return Ok(paymentRequest);
        }
        else
        {
            _logger.LogError($"Payment for OrderId {paymentRequest.OrderId} failed.");
            return BadRequest("Payment failed.");
        }
    }

    private async Task<bool> SimulatePaymentProcessing(PaymentRequest paymentRequest)
    {
        // Logic to process payment
        // In a real-world scenario, this would involve calling a payment gateway
        Task.Delay(2000).Wait(); // Simulate payment processing delay
        await Task.CompletedTask;
        return true; // Assume payment is always successful
    }
}