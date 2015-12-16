using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.Web
{
    public static class WebRequestExtensions
    {
        public static HttpWebRequest CreateHttpWebRequest(this string url)
        {
            HttpWebRequest output = null;

            try
            {
                output = (HttpWebRequest)HttpWebRequest.Create(url);
            }
            catch { }

            return output;
        }
    }
}
