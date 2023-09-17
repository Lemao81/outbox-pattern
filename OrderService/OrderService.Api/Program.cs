using System.Net.Sockets;
using Common.API.Extensions;
using Common.Domain.Consts;
using Common.Domain.Interfaces;
using Common.Domain.Models;
using Common.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OrderService.API.Dto;
using OrderService.API.HostedServices;
using OrderService.Domain.Db;
using OrderService.Domain.Extensions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Services;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging(ServiceNames.OrderService);

builder.Services.AddDbContextFactory<OrderServiceDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.AddHostedService<OutboxPollingHostedService>();

builder.Services.AddRabbitMqConnectionFactory();

builder.Services.AddSingleton<IConnection>(sp =>
{
    var connectionFactory = sp.GetRequiredService<ConnectionFactory>();

    return Policy.Handle<BrokerUnreachableException>().WaitAndRetry(5, _ => TimeSpan.FromSeconds(2)).Execute(() => connectionFactory.CreateConnection());
});
builder.Services.AddSingleton<IMessageProducer, RabbitMqMessageProducer>();

builder.Services.AddScoped<IOrderCrudService, OrderCrudService>();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(RabbitMqOptions.SectionName));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderServiceDbContext>();
    Policy.Handle<SocketException>().Or<PostgresException>(e => e.Message.Contains("system is starting up")).WaitAndRetry(10, _ => TimeSpan.FromSeconds(2))
        .Execute(() =>
        {
            Console.WriteLine("Try migrate database");
            dbContext.Database.Migrate();
        });
}

app.MapPost("api/orders", async ([FromBody] AddOrderRequest request, [FromServices] IOrderCrudService orderCrudService) =>
{
    if (request.ProductIds?.Any() != true)
    {
        return Results.BadRequest("An order requires products added");
    }

    var order = await orderCrudService.CreateAsync(request.ProductIds);

    return order is not null ? Results.Created($"api/orders/{order.Id}", order.MapToDto()) : Results.StatusCode(500);
});

app.Run();
