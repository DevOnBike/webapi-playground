namespace Messaging
{
    public interface IMessageBus
    {
        void Publish<T>(T message);
    }
}

