using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

        /// <summary>
        /// Creates a http request with given context
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contextRequest"></param>
        /// <returns></returns>
        public static HttpWebRequest CreateHttpWebRequest(this string url, HttpRequest contextRequest)
        {
            var request = url.CreateHttpWebRequest();

            if (request == null)
                return null;


            //copy cookies
            request.CookieContainer = new System.Net.CookieContainer();
            foreach (string key in contextRequest.Cookies.AllKeys)
            {
                System.Web.HttpCookie c = contextRequest.Cookies.Get(key);

                System.Net.Cookie outC = new System.Net.Cookie(c.Name, c.Value);
                outC.Domain = request.RequestUri.Host;

                request.CookieContainer.Add(outC);
            }

            //copy headers
            request.Headers = new System.Net.WebHeaderCollection();
            foreach (string key in contextRequest.Headers.AllKeys)
            {
                //skip some headers that should be modified using request properties
                if (key == "Connection" || key == "Content-Length" || key == "Content-Type" || key == "Accept" || key == "Host" || key == "Referer" || key == "User-Agent") continue;

                string h = contextRequest.Headers.Get(key);

                request.Headers.Add(key, h);
            }

            request.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization, Accept");

            //set request params based on incoming request
            request.Method = contextRequest.HttpMethod;
            request.ContentType = contextRequest.ContentType;
            request.UserAgent = contextRequest.UserAgent;
            request.ContentLength = contextRequest.ContentLength;
            request.Accept = contextRequest.Headers.Get("Accept");


            //If this is a post / put request copy input stream
            if (contextRequest.HttpMethod == "POST" || contextRequest.HttpMethod == "PUT")
            {
                var outputStream = request.GetRequestStream();
                contextRequest.InputStream.CopyTo(outputStream);
                outputStream.Close();
            }

            return request;
        }


        /// <summary>
        /// Executes a web request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpWebResponse ExecuteRequest(this HttpWebRequest request)
        {
            System.Net.HttpWebResponse response = null;
            if (request == null)
                return null;
            try
            {
                response = (System.Net.HttpWebResponse)request.GetResponse();
            }
            catch (System.Net.WebException we)
            {
                response = (System.Net.HttpWebResponse)we.Response;
            }
            return response;
        }

        /// <summary>
        /// Copies a HttpWebResponse stream to HttpResponse output stream; also by default copies response internals such as cookies, status, content type
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static void CopyResponse(this System.Net.HttpWebResponse input, System.Web.HttpResponse output, bool copyInternals = true)
        {
            if (input != null)
            {
                if (copyInternals)
                {
                    input.CopyResponseInternals(output);
                }

                //copy web request response stream to System.Web.HttpResponse (context.Response) OutputStrem
                var responseStream = input.GetResponseStream();
                responseStream?.CopyTo(output.OutputStream);

                //Close web request
                input.Close();
            }
        }

        /// <summary>
        /// Copies response internals suh as status code, cookies and stuff
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static void CopyResponseInternals(this System.Net.HttpWebResponse input, System.Web.HttpResponse output)
        {
            if (input != null)
            {
                output.StatusCode = (int) input.StatusCode;
                output.StatusDescription = input.StatusDescription;
                output.ContentType = input.ContentType;

                // copy input cookies so teh can be passed to the client
                foreach (System.Net.Cookie cookie in input.Cookies)
                {
                    System.Web.HttpCookie c = new System.Web.HttpCookie(cookie.Name);
                    c.Value = cookie.Value;

                    output.Cookies.Add(c);
                }
            }
        }

    }
}
