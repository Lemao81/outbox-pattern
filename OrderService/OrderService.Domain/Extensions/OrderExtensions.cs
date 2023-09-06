using Common.Domain.Models.Dtos;
using OrderService.Domain.Models;

namespace OrderService.Domain.Extensions;

public static class OrderExtensions
{
    public static OrderDto MapToDto(this Order order) => new()
    {
        Id = order.Id,
        Status = order.Status.ToString(),
        TotalAmount = order.TotalAmount,
        Products = order.Products.Select(p => p.MapToDto()).ToList()
    };
}
