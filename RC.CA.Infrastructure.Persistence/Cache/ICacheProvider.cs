namespace RC.CA.Infrastructure.Persistence.Cache
{
    public interface ICacheProvider<T>
    {
        Task<T> GetFromCacheAsync(string cacheKey);
        Task<T> AddToCacheAsync(T valueToCache, string cacheKey, int expiresInMinutes = 5);
        void RemoveFromCache(string cacheKey);
    }
}
