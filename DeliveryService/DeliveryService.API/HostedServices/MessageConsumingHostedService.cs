using Common.Domain.Consts;
using Common.Domain.Services;
using RabbitMQ.Client;

namespace DeliveryService.API.HostedServices;

public class MessageConsumingHostedService : IHostedService
{
    private readonly IConnection _connection;
    private readonly ILogger<MessageConsumingHostedService> _logger;

    public MessageConsumingHostedService(IConnection connection,
        ILogger<MessageConsumingHostedService> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(Topics.OrderCreated, durable: true, exclusive: false);
            var consumer = new RabbitMqMessageConsumer(channel);
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
}
