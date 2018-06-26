using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.Cache
{
    /// <summary>
    /// Provides facilities for caching user configuration
    /// </summary>
    public abstract class AbstractCache<T> : ICache<T>
    {
        protected AbstractCache()
            : this (CacheType.InMemory, 5000)
        { }

        /// <summary>
        /// creates a new instance
        /// </summary>
        /// <param name="cacheType"></param>
        /// <param name="cacheTimeout">Miliseconds</param>
        protected AbstractCache(CacheType cacheType, int cacheTimeout)
        {
            CacheType = cacheType;
            CacheTimeout = cacheTimeout;
        }
        
        /// <summary>
        /// cache timeout in miliseconds
        /// </summary>
        public int CacheTimeout { get; }

        /// <summary>
        /// cache timeout expressed in ticks
        /// </summary>
        protected virtual long CacheTimeoutTicks => TimeSpan.TicksPerMillisecond * CacheTimeout;

        public CacheType CacheType { get; }

        public abstract CachedItem<T> Get(string cacheKey);

        public abstract void Set(string cacheKey, T obj);

        public abstract void Clean();

        public abstract void CleanByKey(string key);
    }
}
