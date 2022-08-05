namespace RC.CA.Infrastructure.Persistence.Cache
{
    public interface ICacheProvider
    {
        Task<T> GetFromCacheAsync<T>(string cacheKey);
        Task<T> AddToCacheAsync<T>(string cacheKey, T valueToCache, int expiresInMinutes = 5);
        void RemoveFromCache(string cacheKey);
    }
}
