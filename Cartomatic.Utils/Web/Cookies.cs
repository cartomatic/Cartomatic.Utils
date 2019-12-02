#if NETFULL
using System;
using System.Web;

namespace Cartomatic.Utils.Web
{
    public static class CookieExtansions
    {
        /// <summary>
        /// Sets a value for a specified param of a specified cookie
        /// </summary>
        /// <param name="pValue">Cookie param name</param>
        /// <param name="pName">Cookie param value</param>
        /// <param name="response"></param>
        /// <param name="cookieName"></param>
        /// <param name="cookieSecure">should the cookie be a secure cookie</param>
        /// <param name="cookieValidSeconds">How long the cookie should be valid</param>
        public static void SetCookieValue(this HttpResponse response, string cookieName, string pName, string pValue, bool cookieSecure, int cookieValidSeconds)
        {
            var request = HttpContext.Current.Request;

            var cookie = request.GetCookie(cookieName) ?? new HttpCookie(cookieName);

            cookie.Expires = DateTime.Now.AddSeconds(cookieValidSeconds);
            cookie.Secure = cookieSecure;

            if (!string.IsNullOrEmpty(pName))
            {
                cookie[pName] = pValue;
            }

            response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Gets a cookie value for a specified param of a specified cookie
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pName"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookieValue(this HttpRequest request, string cookieName, string pName)
        {
            string pValue = null;

            var cookie = request.GetCookie(cookieName);

            if (cookie != null)
            {
                pValue = cookie[pName];
            }

            return pValue;
        }

        /// <summary>
        /// Gets a cookie by name off the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        private static HttpCookie GetCookie(this HttpRequest request, string cookieName)
        {
            return request.Cookies[cookieName]; ;
        }
    }
}
#endif