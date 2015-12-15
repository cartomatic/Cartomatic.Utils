using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cartomatic.Utils.Serialisation
{
    public static class JsonSerializationExtensions
    {
        /// <summary>
        /// Deserializes json string to the type specified
        /// </summary>
        /// <typeparam name="T">Type to deserialize to</typeparam>
        /// <param name="json"></param>
        /// <param name="silent"></param>
        /// <returns></returns>
        public static T DeserializeFromJson<T>(this string json, bool silent = true)
        {
            return (T) json.DeserializeFromJson(typeof (T), silent);
        }

        public static object DeserializeFromJson(this string json, Type destinationType, bool silent = true)
        {
            object output = null;

            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    output = JsonConvert.DeserializeObject(json, destinationType);
                }
                catch (Exception ex)
                {
                    if (!silent)
                    {
                        throw;
                    }
                }
            }

            return output;
        }


        /// <summary>
        /// Serializes object to json string
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SerializeToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
