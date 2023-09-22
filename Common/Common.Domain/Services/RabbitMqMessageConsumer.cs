using System.Text;
using System.Text.Json;
using Common.Domain.Consts;
using Common.Domain.Interfaces;
using Common.Domain.Models.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Domain.Services;

public class RabbitMqMessageConsumer : EventingBasicConsumer, IRabbitMqMessageConsumer
{
    public RabbitMqMessageConsumer(IModel model) : base(model)
    {
        Received += async (_, args) =>
        {
            var bodyBytes = args.Body.ToArray();
            var bodyString = Encoding.UTF8.GetString(bodyBytes);
            
            switch (args.RoutingKey)
            {
                case Topics.OrderCreated:
                    await HandleOrderCreatedMessageAsync(bodyString);
                    break;
                default: 
                    throw new ArgumentException($"No handler for routing key {args.RoutingKey}");
            }
        };
    }

    private static async Task HandleOrderCreatedMessageAsync(string bodyString)
    {
        var message = JsonSerializer.Deserialize<OrderCreatedMessage>(bodyString);
    }
}
