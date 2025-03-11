using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CustomerEngagementDashboardApp.Services.CachingService
{
    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _cache;

        public CachingService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null)
        {
            var jsonData = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions();

            if (absoluteExpirationRelativeToNow.HasValue)
            {
                options.SetAbsoluteExpiration(absoluteExpirationRelativeToNow.Value);
            }
            else
            {
                options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            }

            await _cache.SetStringAsync(key, jsonData, options);
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var jsonData = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(jsonData))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
