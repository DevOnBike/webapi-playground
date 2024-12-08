using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Event;
using Messaging.Messages;

namespace Messaging.Actors
{
    public class SubscriberActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        public SubscriberActor()
        {
            Subscribe();
            
            Receive<SubscribeAck>(ack =>
            {
                _log.Info("subscriber actor ACK", ack.Subscribe.Group);

                if (ack.Subscribe.Topic == AkkaMessaging.PubSubTopic && ack.Subscribe.Ref.Equals(Self))
                {
                    Become(Ready);
                }

            });
        }

        public static Props Prop()
        {
            return Props.Create<SubscriberActor>();
        }

        private void Ready()
        {
            Receive<string>(message =>
            {
                _log.Info("subscriber actor got {0}", message);
            });

            ReceiveAsync<Notification>(message =>
            {
                _log.Info("subscriber actor got notification {0}", message);
                return Task.CompletedTask;
            });
        }

        private void Subscribe()
        {
            var mediator = DistributedPubSub.Get(Context.System).Mediator;
            mediator.Tell(new Subscribe(AkkaMessaging.PubSubTopic, Self));
        }
    }
}

