using Akka.Configuration.Hocon;

namespace Messaging
{
    public interface IDistributedCache
    {
        Task SetAsync(string key, string value, CancellationToken token);
        Task<string> GetAsync(string key, CancellationToken token);
        Task RemoveAsync(string key, CancellationToken token);
    }
}

