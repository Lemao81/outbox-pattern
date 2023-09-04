namespace Common.Domain.Models.Dtos;

public class OrderDto
{
    public List<ProductDto> Products { get; set; } = new();
}
