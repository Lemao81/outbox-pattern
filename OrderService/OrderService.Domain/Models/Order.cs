using OrderService.Domain.Enums;

namespace OrderService.Domain.Models;

public class Order : EntityBase
{
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public List<Product> Products { get; set; } = new();
}
