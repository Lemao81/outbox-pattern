using Common.Domain.Models.Messages;

namespace Common.Domain.Interfaces;

public interface IMessageProducer
{
    Task<bool> ProduceMessageAsync<TMessage, TData>(TMessage message) where TMessage : MessageBase<TData>;
}
