using Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace webapi2.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ClusterController : Controller
    {
        private readonly ILogger<ClusterController> _logger;
        private readonly IMessageBus _bus;
        private readonly IDistributedCache _cache;

        private const string _cacheKey = "cluster_cache_key";

        public ClusterController(
            ILogger<ClusterController> logger,
            IMessageBus bus,
            IDistributedCache cache)
        {
            _logger = logger;
            _bus = bus;
            _cache = cache;
        }

        [HttpPost(Name = "Publish")]
        public IActionResult Publish([FromBody] PublishRequest request)
        {
            _bus.Publish(request.Message);

            return Ok(true);
        }

        [HttpPost(Name = "SetCache")]
        public async Task<IActionResult> SetCacheAsync([FromBody] SetCacheRequest request)
        {
            await _cache.SetAsync(_cacheKey, request.Value, CancellationToken.None);

            return Ok(true);
        }

        [HttpPost(Name = "GetCache")]
        public async Task<IActionResult> GetCacheAsync()
        {
            var cached = await _cache.GetAsync(_cacheKey, CancellationToken.None);

            return Ok(cached);
        }

        [HttpPost(Name = "DeleteCache")]
        public async Task<IActionResult> DeleteCacheAsync()
        {
            await _cache.RemoveAsync(_cacheKey, CancellationToken.None);

            return Ok(true);
        }

    }
}
