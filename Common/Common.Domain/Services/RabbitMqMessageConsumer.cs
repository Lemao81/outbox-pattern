using System.Text;
using Common.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Domain.Services;

public class RabbitMqMessageConsumer : EventingBasicConsumer, IRabbitMqMessageConsumer
{
    private readonly List<Func<string, string, Task>> _messageHandler = new();

    public RabbitMqMessageConsumer(IModel model) : base(model)
    {
        Received += async (_, args) =>
        {
            var bodyBytes = args.Body.ToArray();
            var bodyString = Encoding.UTF8.GetString(bodyBytes);

            foreach (var handler in _messageHandler)
            {
                await handler(args.RoutingKey, bodyString);
            }
        };
    }

    public void RegisterHandler(Func<string, string, Task> handler)
    {
        _messageHandler.Add(handler);
    }
}
