using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cartomatic.Utils.Dto
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Copies public properties from a source object to a destination object
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        public static void CopyPublicPropertiesFrom(this object dest, object src)
        {
            if (src == null)
                return;

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

            var srcProps = src.GetType().GetProperties(flags);
            var dstProps = dest.GetType().GetProperties(flags);

            foreach (var srcFld in srcProps)
            {
                var dstFld = dstProps.FirstOrDefault(field => field.Name == srcFld.Name);

                if (dstFld != null && srcFld.PropertyType == dstFld.PropertyType)
                {
                    //make sure to only write to props that are writable
                    if (dstFld.CanWrite)
                    {
                        dstFld.SetValue(dest, srcFld.GetValue(src));
                    }
                }
            }
        }

        /// <summary>
        /// Copies public properties from a src object to a destination object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static T CopyPublicPropertiesToNew<T>(this object src) where T : class
        {
            var dest = (T) Activator.CreateInstance(typeof(T));
            dest.CopyPublicPropertiesFrom(src);
            return dest;
        }
    }
}
