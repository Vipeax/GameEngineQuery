using System;
using ChivStatus.Constants;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ChivStatus.Helpers
{
    public class Cache
    {
        private static readonly Lazy<ConnectionMultiplexer> connection =
            new Lazy<ConnectionMultiplexer>(
                () =>
                        ConnectionMultiplexer.Connect(Security.RedisCacheKey));

        public static T GetOrStoreInCache<T>(string key, Func<T> action) where T : class
        {
            var cacheDb = connection.Value.GetDatabase();
            try
            {
                if (cacheDb.KeyExists(key))
                {
                    var serializedCacheObj = cacheDb.StringGet(key);

                    if (serializedCacheObj.IsNullOrEmpty == false)
                    {
                        return JsonConvert.DeserializeObject<T>(serializedCacheObj);
                    }
                }
            }
            catch
            {
            }

            T result = action();

            if (result != null)
            {
                try
                {
                    cacheDb.StringSet(key, JsonConvert.SerializeObject(result), new TimeSpan(0, 0, 3));
                }
                catch
                {
                }
            }

            return result;
        }
    }
}