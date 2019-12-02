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
#pragma warning disable 1591
        [JsonIgnore]
        string Serialized { get; set; }
#pragma warning restore 1591
    }
}
