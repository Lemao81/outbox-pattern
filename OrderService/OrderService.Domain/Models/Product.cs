using OrderService.Domain.Enums;

namespace OrderService.Domain.Models;

public class Product : EntityBase
{
    public ProductCategory Category { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<Order> Orders { get; set; } = new();
}
