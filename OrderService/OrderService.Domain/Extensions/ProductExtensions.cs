using Common.Domain.Models.Dtos;
using OrderService.Domain.Models;

namespace OrderService.Domain.Extensions;

public static class ProductExtensions
{
    public static ProductDto MapToDto(this Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Category = product.Category.ToString(),
        Price = product.Price
    };
}
