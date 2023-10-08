using System.Text.Json;
using Common.Domain.Consts;
using Common.Domain.Models.Messages;
using Common.Domain.Services;
using DeliveryService.Domain.Db;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Models;
using RabbitMQ.Client;

namespace DeliveryService.API.HostedServices;

public class MessageConsumingHostedService : IHostedService
{
    private readonly IConnection _connection;
    private readonly ILogger<MessageConsumingHostedService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MessageConsumingHostedService(IConnection connection,
        ILogger<MessageConsumingHostedService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _connection = connection;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(Topics.OrderCreated, durable: true, exclusive: false);
            var consumer = new RabbitMqMessageConsumer(channel);
            consumer.RegisterHandler(HandleMessageAsync);
            channel.BasicConsume(Topics.OrderCreated, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{Message}", exception.Message);

            return Task.CompletedTask;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task HandleMessageAsync(string routingKey, string bodyString)
    {
        switch (routingKey)
        {
            case Topics.OrderCreated:
                await HandleOrderCreatedMessageAsync(bodyString);
                break;
            default:
                throw new ArgumentException($"No handler for routing key {routingKey}");
        }
    }

    private async Task HandleOrderCreatedMessageAsync(string bodyString)
    {
        var message = JsonSerializer.Deserialize<OrderCreatedMessage>(bodyString);
        if (message is null) return;

        using var scope = _serviceScopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();

        if (dbContext.Deliveries is null) return;

        if (dbContext.Deliveries.Any(d => d.OrderId == message.Data.Id)) return;

        await dbContext.Deliveries.AddAsync(new Delivery
        {
            OrderId = message.Data.Id,
            Status = DeliveryStatus.Pending
        });

        _logger.LogInformation("New delivery created for order {OrderId}", message.Data.Id);
    }
}
