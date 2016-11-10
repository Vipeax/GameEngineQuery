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
            "localhost:6379,ssl=False,abortConnect=False";
#endif
    }
}
