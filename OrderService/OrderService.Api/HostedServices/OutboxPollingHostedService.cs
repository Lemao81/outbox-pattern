using Common.Domain.Models.Dtos;
using Common.Domain.Models.Messages;
using Common.Domain.Services;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Db;
using OrderService.Domain.Enums;
using OrderService.Domain.Extensions;
using OrderService.Domain.Models;

namespace OrderService.API.HostedServices;

public class OutboxPollingHostedService : BackgroundService
{
    private readonly ILogger<OutboxPollingHostedService> _logger;
    private readonly IMessageProducer _messageProducer;
    private readonly IDbContextFactory<OrderServiceDbContext> _dbContextFactory;

    public OutboxPollingHostedService(ILogger<OutboxPollingHostedService> logger, IMessageProducer messageProducer,
        IDbContextFactory<OrderServiceDbContext> dbContextFactory)
    {
        _logger = logger;
        _messageProducer = messageProducer;
        _dbContextFactory = dbContextFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PollOutboxEventsAsync(stoppingToken);
                    await Task.Delay(5000, stoppingToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "{Message}", exception.Message);
                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{Message}", exception.Message);
        }
    }

    private async Task PollOutboxEventsAsync(CancellationToken stoppingToken)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync(stoppingToken);
        var outboxEvents = await dbContext.Outboxes!.ToListAsync(stoppingToken);
        foreach (var outboxEvent in outboxEvents)
        {
            switch (outboxEvent.Event)
            {
                case OutboxEvent.OrderCreated:
                    await HandleOrderCreatedEventAsync(outboxEvent, dbContext, stoppingToken);
                    break;
                default:
                    throw new ArgumentException($"Value '{outboxEvent.Event}' out of range");
            }
        }
    }

    private async Task HandleOrderCreatedEventAsync(Outbox outboxEvent, OrderServiceDbContext dbContext, CancellationToken stoppingToken)
    {
        var order = await dbContext.Orders!.FirstOrDefaultAsync(o => o.Id == outboxEvent.EntityId, stoppingToken);
        if (order is not null)
        {
            var message = new OrderCreatedMessage(order.MapToDto());
            await _messageProducer.ProduceMessageAsync<OrderCreatedMessage, OrderDto>(message);
            _logger.LogInformation("Outbox event '{EventId}' sent | Entity id '{EntityId}'", outboxEvent.Id, outboxEvent.EntityId);
        }

        dbContext.Outboxes!.Remove(outboxEvent);
        await dbContext.SaveChangesAsync(stoppingToken);
    }
}
