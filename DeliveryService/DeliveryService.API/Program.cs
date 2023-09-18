using Common.API.Extensions;
using Common.Domain.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging(ServiceNames.DeliveryService);

builder.Services.AddRabbitMqConnection();

var app = builder.Build();

app.Run();
