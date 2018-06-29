using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cartomatic.Utils.Sorting
{
    public static partial class ReadSorterExtensions
    {
        /// <summary>
        /// deserializes extjs filter into list of ReadFilter
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<ReadSorter> ExtJsJsonSortersToReadSorters(this string json)
        {
            return string.IsNullOrEmpty(json) ?
                new List<ReadSorter>() :
                JsonConvert.DeserializeObject<List<ReadSorter>>(json);
        }
    }
}
