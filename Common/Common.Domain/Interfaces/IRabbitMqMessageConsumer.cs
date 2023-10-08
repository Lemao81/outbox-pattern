using RabbitMQ.Client;

namespace Common.Domain.Interfaces;

public interface IRabbitMqMessageConsumer : IBasicConsumer
{
    void RegisterHandler(Func<string, string, Task> handler);
}
