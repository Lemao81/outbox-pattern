namespace Common.Domain.Models.Dtos;

public class OrderDto
{
    public List<OrderItemDto> Items { get; set; } = new();
}
