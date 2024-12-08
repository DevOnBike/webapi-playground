using Akka.Actor;
using Akka.Hosting;
using Messaging.Actors;

namespace Messaging
{
    public class MessageBus : IMessageBus
    {
        private readonly IActorRef _actor;

        public MessageBus(IRequiredActor<PublisherActor> actor)
        {
            _actor = actor.ActorRef;
        }
        
        public void Publish<T>(T message)
        {
            _actor.Tell(message);
        }

    }
}
