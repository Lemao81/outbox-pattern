using System.Text;
using System.Text.Json;
using Common.Domain.Models;
using Common.Domain.Models.Messages;
using Common.Domain.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Common.Domain.Interfaces;

public class RabbitMqMessageProducer : IMessageProducer
{
    private readonly RabbitMqOptions _options;

    public RabbitMqMessageProducer(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
    }

    public Task ProduceMessageAsync<TMessage, TData>(TMessage message) where TMessage : MessageBase<TData>
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            VirtualHost = "/"
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        channel.QueueDeclare(message.Topic, durable: true);

        var messageJson = JsonSerializer.Serialize(message);
        var bytes = Encoding.UTF8.GetBytes(messageJson);
        channel.BasicPublish("", message.Topic, body: bytes);

        return Task.CompletedTask;
    }
}
