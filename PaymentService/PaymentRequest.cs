namespace PaymentService;

public class PaymentRequest
{
    public string OrderId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
}