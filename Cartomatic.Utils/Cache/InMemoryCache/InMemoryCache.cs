using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Cartomatic.Utils.Cache
{
    public class InMemoryCache<T> : AbstractCache<T>
    {
        private readonly ConcurrentDictionary<string, (T obj, long timeSet)> _cache;

        /// <summary>
        /// creates a new instance
        /// </summary>
        /// <param name="cacheTimeout">miliseconds</param>
        public InMemoryCache(int cacheTimeout)
            : base (CacheType.InMemory, cacheTimeout)
        {
            _cache = new ConcurrentDictionary<string, (T, long)>();
        }

        /// <summary>gets an object from cache</summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public override CachedItem<T> Get(string cacheKey)
        {
            var success = _cache.TryGetValue(cacheKey, out var cached);

            return new CachedItem<T>
            {
                Item = success ? cached.obj : default(T),
                TimedOut = success && DateTime.Now.Ticks - cached.timeSet >= CacheTimeoutTicks,
                Exists = success
            };
        }

        /// <summary>sets an object in cache</summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        public override void Set(string cacheKey, T obj)
        {
            CleanByKey(cacheKey);
            _cache.TryAdd(cacheKey, (obj, DateTime.Now.Ticks));
        }

        /// <summary>Cleans the entire cache</summary>
        public override void Clean()
        {
            _cache.Clear();
        }

        /// <summary>Cleans a cache item by key</summary>
        /// <param name="key"></param>
        public override void CleanByKey(string key)
        {
            _cache.TryRemove(key, out _);
        }
    }
}
