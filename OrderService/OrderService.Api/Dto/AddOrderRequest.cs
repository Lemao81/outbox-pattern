namespace OrderService.API.Dto;

public class AddOrderRequest
{
    public IEnumerable<Guid>? ProductIds { get; set; }
}
