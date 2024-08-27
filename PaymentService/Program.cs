using Microsoft.Extensions.Hosting;
using PaymentService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

Console.WriteLine("* *** *** *** Payment Service Started");

var host = builder.Build();
host.Run();