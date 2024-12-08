using Akka.Actor;
using Akka.Hosting;
using Messaging.Actors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging
{
    public static class AkkaServiceCollectionExtensions
    {
        private const string SectionName = "Akka";

        public static void AddAkkaNet(this IServiceCollection sc, IConfiguration configuration)
        {
            if (!IsEnabled(configuration))
            {
                sc.AddScoped<IMessageBus, NoOpMessageBus>();
                return;
            }

            sc.AddScoped<IMessageBus, MessageBus>();

            sc.AddAkka("cluster", (akkaBuilder, ioc) =>
            {
                var iConfiguration = ioc.GetRequiredService<IConfiguration>();
                var akkaConfig = iConfiguration.GetRequiredSection(SectionName);

                akkaBuilder
                    .AddHocon(akkaConfig, HoconAddMode.Replace)
                    .WithActors((system, registry) =>
                    {
                        var publisherActor = system.ActorOf(PublisherActor.Prop(), "publisher");
                        var subscriber = system.ActorOf(SubscriberActor.Prop(), "subscriber");

                        // var id = subscriber.Path.Uid;

                        registry.TryRegister<PublisherActor>(publisherActor, true);
                        registry.TryRegister<SubscriberActor>(subscriber, true);

                    }).AddStartup((system, registry) =>
                    {
                        var publisher = registry.Get<PublisherActor>();

                        system.Scheduler.ScheduleTellRepeatedly(
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(30),
                            publisher,
                            "Hello Akka",
                            ActorRefs.NoSender); // todo: remove one day ;)
                    });
            });
        }

        private static bool IsEnabled(IConfiguration configuration)
        {
            var section = configuration.GetSection(SectionName);

            if (!section.Exists())
            {
                return false;
            }

            var enabledOption = configuration[$"{SectionName}:enabled"];

            bool.TryParse(enabledOption, out var enabled);

            return enabled;
        }
    }
}

