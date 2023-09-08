using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Db;
using OrderService.Domain.Enums;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;

namespace OrderService.Domain.Services;

public class OrderCrudService : IOrderCrudService
{
    private readonly OrderServiceDbContext _dbContext;
    private readonly ILogger<OrderCrudService> _logger;

    public OrderCrudService(OrderServiceDbContext dbContext, ILogger<OrderCrudService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Order?> CreateAsync(IEnumerable<Guid> productIds)
    {
        var fetchProducts = await _dbContext.Products!.Where(p => productIds.Contains(p.Id)).ToListAsync();
        if (!fetchProducts.Any()) throw new Exception("No products found with given ids");

        var order = new Order
        {
            Status = OrderStatus.Pending,
            TotalAmount = fetchProducts.Sum(p => p.Price),
            Products = fetchProducts
        };

        var outbox = new Outbox
        {
            Event = OutboxEvent.OrderCreated
        };

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            order = (await _dbContext.Orders!.AddAsync(order)).Entity;
            outbox.EntityId = order.Id;
            await _dbContext.Outboxes!.AddAsync(outbox);

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            _logger.LogInformation("Order '{OrderId}' created", order.Id);

            return order;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "{Message}", exception.Message);
            await transaction.RollbackAsync();

            return null;
        }
    }
}
