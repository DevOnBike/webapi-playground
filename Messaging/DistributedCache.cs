using Akka.Actor;
using Akka.Cluster;
using Akka.DistributedData;

namespace Messaging
{
    public class DistributedCache : IDistributedCache
    {
        private readonly ActorSystem _system;

        private static readonly IWriteConsistency _writeConsistency = 
            new WriteAll(TimeSpan.FromSeconds(3));

        public DistributedCache(ActorSystem systemActor)
        {
            _system = systemActor;
        }

        public Task SetAsync(string key, string value, CancellationToken token)
        {
            var dd = _system.DistributedData();
            var cluster = Cluster.Get(_system);
            var keySet = new ORSetKey<string>(key);
            var valueSet = ORSet.Create<string>(cluster.SelfUniqueAddress, value);

            return dd.UpdateAsync(keySet, valueSet, _writeConsistency, token);
        }

        public async Task<string> GetAsync(string key, CancellationToken token)
        {
            var dd = _system.DistributedData();
            var cluster = Cluster.Get(_system);
            var readConsistency = new Akka.DistributedData.ReadMajority(TimeSpan.FromSeconds(3));
            var keySet = new ORSetKey<string>(key);
            var result = await dd.GetAsync(keySet, readConsistency, token);

            return string.Empty;

        }

        public Task RemoveAsync(string key, CancellationToken token)
        {
            var dd = _system.DistributedData();
            var cluster = Cluster.Get(_system);
            var readConsistency = new Akka.DistributedData.ReadMajority(TimeSpan.FromSeconds(3));
            var keySet = new ORSetKey<string>(key);

            return dd.DeleteAsync(keySet, _writeConsistency, token);
        }
    }
}

