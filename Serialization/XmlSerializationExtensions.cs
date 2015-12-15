using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Cartomatic.Utils.Serialization
{

    public static class XmlSerializationExtensions
    {

        /// <summary>
        /// Deserializes object to xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="silent"></param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(this string xml, bool silent = true)
        {
            return (T)xml.DeserializeFromXml(typeof(T), silent);
        }

        /// <summary>
        /// Deserializes object from xml
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="destinationType"></param>
        /// <param name="silent"></param>
        /// <returns></returns>
        public static object DeserializeFromXml(this string xml, Type destinationType, bool silent = true)
        {
            object output = null;

            XmlSerializer xmlSerializer = new XmlSerializer(destinationType);

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {

                try
                {
                    output = xmlSerializer.Deserialize(ms);
                }
                catch
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
        /// Serializes an object to xml
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SerializeToXml(this object o)
        {
            return o.SerializeToXml(null);
        }

        /// <summary>
        /// Serializes object to xml and adds specified namespaces
        /// </summary>
        /// <param name="o"></param>
        /// <param name="xmlns"></param>
        /// <returns></returns>
        public static string SerializeToXml(this object o, XmlSerializerNamespaces xmlns = null)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(o.GetType());

            using (MemoryStream ms = new MemoryStream())
            {
                if (xmlns != null)
                {
                    xmlSerializer.Serialize(ms, o, xmlns);
                }
                else
                {
                    xmlSerializer.Serialize(ms, o);
                }

                ms.Position = 0; //rewind the position in memeory stream as it should be at the end after writing to it

                using (StreamReader sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }


    }
}
