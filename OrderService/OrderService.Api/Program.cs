using Common.API.Extensions;
using Common.Domain.Consts;
using Common.Domain.Interfaces;
using Common.Domain.Models;
using Common.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Dto;
using OrderService.Domain.Db;
using OrderService.Domain.Extensions;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging(ServiceNames.OrderService);

builder.Services.AddDbContext<OrderServiceDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddRabbitMqConnectionFactory();

builder.Services.AddScoped<IConnection>(sp => sp.GetRequiredService<ConnectionFactory>().CreateConnection());
builder.Services.AddScoped<IMessageProducer, RabbitMqMessageProducer>();
builder.Services.AddScoped<IOrderCrudService, OrderCrudService>();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(RabbitMqOptions.SectionName));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderServiceDbContext>();
    dbContext.Database.Migrate();
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
