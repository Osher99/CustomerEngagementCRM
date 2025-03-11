namespace CustomerEngagementDashboardApp.Services.CachingService
{
    public interface ICachingService
    {
        Task SetCacheAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null);
        Task<T?> GetCacheAsync<T>(string key);
        Task RemoveCacheAsync(string key);
    }

}
