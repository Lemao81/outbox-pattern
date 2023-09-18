using Common.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Common.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqConnection(this IServiceCollection services)
    {
        services.AddSingleton<IConnection>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
            var connectionFactory = new ConnectionFactory
            {
                HostName = options.HostName,
                UserName = options.UserName,
                Password = options.Password,
                VirtualHost = "/"
            };

            return Policy.Handle<BrokerUnreachableException>().WaitAndRetry(5, _ => TimeSpan.FromSeconds(2))
                .Execute(() => connectionFactory.CreateConnection());
        });

        return services;
    }
}
