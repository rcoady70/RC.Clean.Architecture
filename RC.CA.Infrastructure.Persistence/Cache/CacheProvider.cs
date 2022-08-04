using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RC.CA.Infrastructure.Logging.Constants;

namespace RC.CA.Infrastructure.Persistence.Cache
{
    public class CacheProvider<T> : ICacheProvider<T>
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheProvider<T>> _logger;

        public CacheProvider(IMemoryCache memoryCache, ILogger<CacheProvider<T>> logger)
        {
            _cache = memoryCache;
            _logger = logger;
        }
        /// <summary>
        /// Get cached response of type T.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<T> GetFromCacheAsync(string cacheKey)
        {
            T cachedValue = default(T);
            bool isAvaiable = _cache.TryGetValue($"{typeof(T).Name}-{cacheKey}", out cachedValue);
            return cachedValue;
        }
        /// <summary>
        /// Add value to cache
        /// </summary>
        /// <param name="valueToCache"></param>
        /// <param name="cacheKey"></param>
        /// <param name="expiresInMinutes"></param>
        /// <returns></returns>
        public async Task<T> AddToCacheAsync(T valueToCache, string cacheKey, int expiresInMinutes = 5)
        {
            //Lock when adding in case the item has been added to cache since check was completed
            SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
            T cachedValue = default;
            try
            {
                cacheKey = $"{typeof(T).Name}-{cacheKey}";
                await semaphore.WaitAsync();
                cachedValue = await GetFromCacheAsync(cacheKey);
                if (cachedValue != null) return cachedValue;

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(expiresInMinutes),
                    SlidingExpiration = TimeSpan.FromMinutes(2),
                };
                _cache.Set(cacheKey, valueToCache, cacheEntryOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggerEvents.ErrorEvt, ex, @"Exception adding data to cache {errormessage} Cache key {cachekey} Cache value type {cachevalue} ",
                                 ex.Message,
                                 cacheKey, typeof(T).Name);
            }
            finally
            {
                semaphore.Release();
            }
            return cachedValue;
        }
        /// <summary>
        /// Remove from cache
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public void RemoveFromCache(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }
    }
}
