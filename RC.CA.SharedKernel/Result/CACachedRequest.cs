namespace RC.CA.SharedKernel.Result
{
    [Obsolete("Used multiple mediatr pipelines")]
    public class CACachedRequest<T>
    {
        public string CacheKey { get; private set; }
        public int CacheExpiryInMinutes { get; private set; }
        public T Request { get; private set; }
        public CACachedRequest(string cacheKey, int expiry, T Value)
        {
            Request = Value;
            cacheKey = cacheKey ?? string.Empty;
            CacheExpiryInMinutes = expiry;
        }
        public static CACachedRequest<T> Cache(T value, string key, int cacheExpiryInMinutes)
        {
            return new CACachedRequest<T>(key, cacheExpiryInMinutes, value);
        }

    }
}
