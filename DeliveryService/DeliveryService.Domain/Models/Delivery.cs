using Common.Domain.Models;
using DeliveryService.Domain.Enums;

namespace DeliveryService.Domain.Models;

public class Delivery : EntityBase
{
    public Guid OrderId { get; set; }
    public DeliveryStatus Status { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
