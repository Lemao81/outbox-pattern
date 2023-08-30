using Common.Domain.Consts;
using Common.Domain.Models.Dtos;

namespace Common.Domain.Models.Messages;

public class OrderCreatedMessage : MessageBase<OrderDto>
{
    public OrderCreatedMessage(OrderDto data) : base(Topics.OrderCreated, data)
    {
    }
}
