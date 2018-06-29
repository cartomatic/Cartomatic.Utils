using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cartomatic.Utils.Filtering
{
    public static partial class ReadFilterExtensions
    {
        /// <summary>
        /// Deserializes ExtJs read filter json to a list of ReadFilter
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<ReadFilter> ExtJsJsonFiltersToReadFilters(this string json)
        {
            return string.IsNullOrEmpty(json) ?
                new List<ReadFilter>() :
                JsonConvert.DeserializeObject<List<ReadFilter>>(json);
        }
    }
}
