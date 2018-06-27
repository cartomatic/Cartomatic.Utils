using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cartomatic.Utils.JsonSerializableObjects
{
    /// <summary>
    /// whether or not object is 'JsonSerializable'
    /// </summary>
    public interface IJsonSerializable
    {
        [JsonIgnore]
        string Serialized { get; set; }
    }
}
