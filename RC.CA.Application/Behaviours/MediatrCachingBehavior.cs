using System.Text;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Settings;

namespace RC.CA.Application.Behaviours;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
                                                    where TRequest : MediatR.IRequest<TResponse>
                                                    where TResponse : ICAResult
{
    private readonly IDistributedCache _cache;
    private readonly ILogger _logger;
    private readonly CacheSettings _cacheSettings;

    public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger, IOptions<CacheSettings> cacheSettings)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cacheSettings = cacheSettings.Value;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (request is IIsCachableMediatrResult cacheableQuery && _cacheSettings.Enabled)
        {
            TResponse response;
            if (cacheableQuery.CacheSkip) return await next();

            var cachedResponse = await _cache.GetAsync(cacheableQuery.CacheKey, cancellationToken);
            if (cachedResponse != null)
            {
                response = Encoding.Default.GetString(cachedResponse).FromJsonExt<TResponse>();
                response.IsCachedResult = true;
                _logger.LogInformation($"Cache hit -> '{cacheableQuery.CacheKey}'.");
            }
            else
            {
                response = await GetResponseAndAddToCache(cancellationToken, next, cacheableQuery);
            }
            return response;
        }
        else
        {
            return await next();
        }
    }
    /// <summary>
    /// Get response and add to cache
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="next"></param>
    /// <param name="cacheableQuery"></param>
    /// <returns></returns>
    private async Task<TResponse> GetResponseAndAddToCache(CancellationToken cancellationToken,
                                                           RequestHandlerDelegate<TResponse> next,
                                                           IIsCachableMediatrResult cacheableQuery)
    {
        TResponse response = await next();
        var slidingExpiration = cacheableQuery.CacheSlidingExpirationInMin == null ? TimeSpan.FromMinutes(_cacheSettings.SlidingExpiration) : TimeSpan.FromMinutes(cacheableQuery.CacheSlidingExpirationInMin);
        var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };
        var serializedData = Encoding.Default.GetBytes(response.ToJsonExt());
        await _cache.SetAsync(cacheableQuery.CacheKey, serializedData, options, cancellationToken);
        return response;
    }
}
