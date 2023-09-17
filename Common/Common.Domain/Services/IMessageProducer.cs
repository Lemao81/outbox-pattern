using Common.Domain.Models.Messages;

namespace Common.Domain.Services;

public interface IMessageProducer
{
    Task<bool> ProduceMessageAsync<TMessage, TData>(TMessage message) where TMessage : MessageBase<TData>;
}
