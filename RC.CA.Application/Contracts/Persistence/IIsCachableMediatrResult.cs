namespace RC.CA.Application.Contracts.Persistence
{
    public interface IIsCachableMediatrResult
    {
        bool CacheSkip { get; }
        string CacheKey { get; }
        int CacheSlidingExpirationInMin { get; }
    }
}
