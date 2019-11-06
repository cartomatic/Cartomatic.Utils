using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Cartomatic.Utils.Cache
{
    public class RedisCache<T> : AbstractCache<T>
    {
        public RedisCache(int cacheTimeout)
            : base (CacheType.Redis, cacheTimeout)
        {
        }

        /// <summary>gets an object from cache</summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public override CachedItem<T> Get(string cacheKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>sets an object in cache</summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        public override void Set(string cacheKey, T obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>Cleans the entire cache</summary>
        public override void Clean()
        {
            throw new NotImplementedException();
        }

        /// <summary>Cleans a cache item by key</summary>
        /// <param name="key"></param>
        public override void CleanByKey(string key)
        {
            throw new NotImplementedException();
        }
    }
}
