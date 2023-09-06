namespace Common.Domain.Models.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }

    public List<ProductDto> Products { get; set; } = new();
}
