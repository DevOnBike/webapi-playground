namespace Messaging
{
    public class NoOpMessageBus : IMessageBus
    {
        public void Publish<T>(T message)
        {

        }

    }
}

