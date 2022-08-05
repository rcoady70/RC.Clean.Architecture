using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RC.CA.Domain.Entities.Club;
using RC.CA.Infrastructure.Persistence.Cache;

namespace RC.CA.WebApi.UnitTests.Caching
{
    public class Caching
    {
        private readonly IOptions<MemoryCacheOptions> _cacheOptions;
        private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _memoryCache;
        private readonly CacheProvider _cache;
        public Caching()
        {
            _cacheOptions = Options.Create<MemoryCacheOptions>(new MemoryCacheOptions());
            _memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache(_cacheOptions);
            _cache = new CacheProvider(_memoryCache, Mock.Of<ILogger<CacheProvider>>());
        }

        [Theory]
        [InlineData("Key2", null)]
        public async Task CacheProvider_GetFromCacheAsync_Failed(string key, string expectedResult)
        {
            string result = await _cache.GetFromCacheAsync<string>(key);
            Assert.Equal(result, expectedResult);
        }
        [Theory]
        [InlineData("KeyOK1", "Value 1", "Value 1")]
        [InlineData("KeyOK2", "Value 2", "Value 2")]
        public async Task CacheProvider_GetFromCacheAsync_OK(string key, string cacheValue, string expectedResult)
        {

            await _cache.AddToCacheAsync<string>(key, cacheValue);
            string result = await _cache.GetFromCacheAsync<string>(key);

            Assert.Equal(result, expectedResult);
        }
        [Theory]
        [CachingTestData]
        public async Task CacheProvider_GetFromCacheAsyncComplexObject_OK(string key, Member cacheValue, Member expectedResult)
        {

            await _cache.AddToCacheAsync(key, cacheValue);
            var result = await _cache.GetFromCacheAsync<Member>(key);

            Assert.Equal(result.Id, expectedResult.Id);
            Assert.Equal(result.Experiences[0].Id, expectedResult.Experiences[0].Id);

        }
    }
}
