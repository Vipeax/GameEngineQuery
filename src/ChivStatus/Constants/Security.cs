namespace ChivStatus.Constants
{
    internal static class Security
    {
#if DEBUG
        internal const string RedisCacheKey =
            "localhost:6379,ssl=False,abortConnect=False";
#endif
#if RELEASE
        internal const string RedisCacheKey =
            "chivstatus.redis.cache.windows.net:6380,password=8ZWfGCkyy/PtVIJvO1VzAlu+IcKg0naKLkO3jh7bdcU=,ssl=True,abortConnect=False";
#endif
#if AVALAN
        internal const string RedisCacheKey =
            "localhost:6379,ssl=False,abortConnect=False";
#endif
    }
}
