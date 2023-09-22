using Common.API.Extensions;
using Common.Domain.Consts;
using Common.Domain.Models;
using DeliveryService.API.HostedServices;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging(ServiceNames.DeliveryService);

builder.Services.AddHostedService<MessageConsumingHostedService>();

builder.Services.AddRabbitMqConnection();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(RabbitMqOptions.SectionName));

var app = builder.Build();

app.Run();
