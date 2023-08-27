namespace OrderService.Domain.Models;

public class Order : EntityBase
{
    public List<OrderItem> Items { get; set; } = new();
}
