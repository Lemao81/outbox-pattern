namespace Common.Domain.Models.Messages;

public abstract class MessageBase<T>
{
    protected MessageBase(string topic, T data)
    {
        Topic = topic;
        Data = data;
    }

    public string Topic { get; set; }
    public T Data { get; set; }
}
