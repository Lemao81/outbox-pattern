using OrderService.Domain.Models;

namespace OrderService.Domain.Interfaces;

public interface IOrderCrudService
{
    Task<Order?> CreateAsync(IEnumerable<Guid> order);
}
