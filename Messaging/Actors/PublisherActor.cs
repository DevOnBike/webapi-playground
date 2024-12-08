using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Event;
using Messaging.Messages;

namespace Messaging.Actors
{
    public class PublisherActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        public PublisherActor()
        {
            Receive<string>(message =>
            {
                _log.Info("publisher receive = {0}", message);

                var notification = new Notification(Sender.ToString(), message);
                var mediator = DistributedPubSub.Get(Context.System).Mediator;

                mediator.Tell(new Publish(AkkaMessaging.PubSubTopic, notification));
            });


        }

        protected override void PreStart()
        {
            _log.Info("publisher actor PreStart ...");

            base.PreStart();
        }

        protected override void PostStop()
        {
            _log.Info("publisher actor PostStop ...");

            base.PostStop();
        }

        public static Props Prop()
        {
            return Props.Create<PublisherActor>();
        }
    }
}

