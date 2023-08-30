using Common.Domain.Models;
using Common.Domain.Models.Messages;

namespace Common.Domain.Services;

public interface IMessageProducer
{
    Task ProduceMessageAsync<TMessage, TData>(TMessage message) where TMessage : MessageBase<TData>;
}
