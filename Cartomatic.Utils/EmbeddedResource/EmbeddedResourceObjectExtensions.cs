using System;
using System.IO;

namespace Cartomatic.Utils.EmbeddedResource
{
    /// <summary>
    /// object extensions for embedded resource utils
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets a stream for an embedded resource
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fullyQualifiedResourceName"></param>
        /// <returns></returns>
        public static Stream GetEmbeddedResourceStream(this object obj, string fullyQualifiedResourceName)
        {
            return obj.GetType().GetEmbeddedResourceStream(fullyQualifiedResourceName);
        }

        /// <summary>
        /// Gets a stream for an embedded resource
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fullyQualifiedResourceName"></param>
        /// <returns></returns>
        public static Stream GetEmbeddedResourceStream(this Type type, string fullyQualifiedResourceName)
        {
            var assembly = type.Assembly;
            return assembly.GetManifestResourceStream(fullyQualifiedResourceName);
        }

        /// <summary>
        /// Gets embedded resource of an object's assembly
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fullyQualifiedResourceName"></param>
        /// <returns></returns>
        public static byte[] GetEmbeddedResource(this object obj, string fullyQualifiedResourceName)
        {
            return obj.GetType().GetEmbeddedResource(fullyQualifiedResourceName);
        }

        /// <summary>
        /// Gets embedded resource of an object's assembly
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fullyQualifiedResourceName"></param>
        /// <returns></returns>
        public static byte[] GetEmbeddedResource(this Type type, string fullyQualifiedResourceName)
        {
            using var stream = type.GetEmbeddedResourceStream(fullyQualifiedResourceName);
            var resourceData = stream.ReadStream();

            return resourceData;
        }

        /// <summary>
        /// Gets embedded resource string content
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fullyQualifiedResourceName"></param>
        /// <returns></returns>
        public static string GetEmbeddedResourceStringContent(this object obj, string fullyQualifiedResourceName)
        {
            return obj.GetType().GetEmbeddedResourceStringContent(fullyQualifiedResourceName);
        }

        /// <summary>
        /// Gets embedded resource string content
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fullyQualifiedResourceName"></param>
        /// <returns></returns>
        public static string GetEmbeddedResourceStringContent(this Type type, string fullyQualifiedResourceName)
        {
            using var str = type.GetEmbeddedResourceStream(fullyQualifiedResourceName);
            using var rdr = new StreamReader(str);
            var content = rdr.ReadToEnd();

            return content;
        }
    }
}
