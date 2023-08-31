using Common.API.Extensions;
using Common.Domain.Consts;
using Common.Domain.Interfaces;
using Common.Domain.Models;
using Common.Domain.Services;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Db;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging(ServiceNames.OrderService);

builder.Services.AddDbContext<OrderServiceDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddRabbitMqConnectionFactory();

builder.Services.AddScoped<IConnection>(sp => sp.GetRequiredService<ConnectionFactory>().CreateConnection());
builder.Services.AddScoped<IMessageProducer, RabbitMqMessageProducer>();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(RabbitMqOptions.SectionName));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderServiceDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
