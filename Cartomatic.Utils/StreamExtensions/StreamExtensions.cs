using System;
using System.IO;

namespace Cartomatic.Utils
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads stream into a byte arr
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static byte[] ReadStream(this Stream input, int? bufferSize = null)
        {
            byte[] buffer = new byte[bufferSize ?? 16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// copies stream to a new memory stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static Stream CopyStream(this Stream stream, int? bufferSize = null)
        {
            var buffer = new byte[bufferSize ?? 16 * 1024];

            var ms = new MemoryStream();
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }

            return ms;
        }

    }
}
