using Akka.Actor;
using Akka.Hosting;
using Messaging.Actors;
using Messaging.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Petabridge.Cmd.Host;

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
            sc.AddScoped<IDistributedCache, DistributedCache>();

            sc.AddAkka("devonbike-cluster", (akkaBuilder, di) =>
            {
                var iConfiguration = di.GetRequiredService<IConfiguration>();
                var akkaConfig = iConfiguration.GetRequiredSection(SectionName);
                var akkaOptions = di.GetRequiredService<IOptions<AkkaOptions>>();
                var cmdEnabled = akkaOptions.Value.CmdEnabled;

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
                        if (cmdEnabled)
                        {
                            PetabridgeCmd.Get(system).Start();
                        }

                        var publisher = registry.Get<PublisherActor>();
                        /*
                        system.Scheduler.ScheduleTellRepeatedly(
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(30),
                            publisher,
                            "Hello Akka",
                            ActorRefs.NoSender); // todo: remove one day ;)
                        */
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

