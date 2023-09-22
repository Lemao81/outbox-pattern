using Common.Domain.Models;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Models;

public class Outbox : EntityBase
{
    public OutboxEvent Event { get; set; }
    public Guid EntityId { get; set; }
}
