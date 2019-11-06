using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Cartomatic.Utils.Cache
{
    public class CachedItem<T>
    {
        /// <summary>
        /// Item retrieved from cache
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// Whether or not an item exists in cache
        /// </summary>
        public bool Exists { get; set; }

        /// <summary>
        /// Whether or not the cached item has timed out
        /// </summary>
        public bool TimedOut { get; set; }

        /// <summary>
        /// Whether or not the cached item is valid
        /// </summary>
        public bool Valid => Exists && !TimedOut;
    }
}
