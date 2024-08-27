using NotificationService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

Console.WriteLine("* *** *** *** Notification service started");

var host = builder.Build();
host.Run();