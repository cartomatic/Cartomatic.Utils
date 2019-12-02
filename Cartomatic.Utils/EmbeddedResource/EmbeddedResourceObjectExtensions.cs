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
        /// <remarks>
        /// ported from maphive
        /// </remarks>
        /// <param name="obj"></param>
        /// <param name="fullyQualifiedResourceName"></param>
        /// <returns></returns>
        public static Stream GetEmbeddedResourceStream(this object obj, string fullyQualifiedResourceName)
        {
            var assembly = obj.GetType().Assembly;
            return assembly.GetManifestResourceStream(fullyQualifiedResourceName);
        }

        /// <summary>
        /// Gets embedded resource of an object's assembly
        /// </summary>
        /// <remarks>
        /// ported from maphive
        /// </remarks>
        /// <param name="obj"></param>
        /// <param name="fullyQualifiedResourceName"></param>
        /// <returns></returns>
        public static byte[] GetEmbeddedResource(this object obj, string fullyQualifiedResourceName)
        {
            byte[] resourceData;

            using (var stream = obj.GetEmbeddedResourceStream(fullyQualifiedResourceName))
            {
                resourceData = stream.ReadStream();
            }

            return resourceData;
        }

        /// <summary>
        /// Gets embedded resource string content
        /// </summary>
        /// <remarks>
        /// ported from maphive
        /// </remarks>
        /// <param name="obj"></param>
        /// <param name="fullyQualifiedResourceName"></param>
        /// <returns></returns>
        public static string GetEmbeddedResourceStringContent(this object obj, string fullyQualifiedResourceName)
        {
            var content = string.Empty;

            using(var str = obj.GetEmbeddedResourceStream(fullyQualifiedResourceName))
            using (var rdr = new StreamReader(str))
            {
                content = rdr.ReadToEnd();
            }

            return content;
        }
    }
}
