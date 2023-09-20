using System.Text;
using System.Text.Json;
using Common.Domain.Interfaces;
using Common.Domain.Models.Messages;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Common.Domain.Services;

public class RabbitMqMessageProducer : IMessageProducer
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqMessageProducer> _logger;

    public RabbitMqMessageProducer(IConnection connection, ILogger<RabbitMqMessageProducer> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public Task<bool> ProduceMessageAsync<TMessage, TData>(TMessage message) where TMessage : MessageBase<TData>
    {
        try
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(message.Topic, durable: true, exclusive: false);

            var messageJson = JsonSerializer.Serialize(message);
            var bytes = Encoding.UTF8.GetBytes(messageJson);
            channel.BasicPublish("", message.Topic, body: bytes);

            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{Message}", exception.Message);

            return Task.FromResult(false);
        }
    }
}
