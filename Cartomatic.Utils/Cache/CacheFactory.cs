using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.Cache
{
    public class CacheFactory
    {
        /// <summary>
        /// creates a new instance o cache object 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheType"></param>
        /// <param name="cacheTimeout">miliseconds</param>
        /// <returns></returns>
        public static ICache<T> CreateCache<T>(CacheType cacheType, int cacheTimeout)
        {
            switch (cacheType)
            {
                default:
                    return new InMemoryCache<T>(cacheTimeout);

                case CacheType.Redis:
                    return new RedisCache<T>(cacheTimeout);
            }
        }
    }
}
