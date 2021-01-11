using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RestSharp;



namespace Cartomatic.Utils.ApiClient
{
    /// <summary>
    /// base for rest api clients
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RestApiClientWithHealthCheck<T> : ApiClientWithHealthCheck<T>
        where T:IApiClientConfiguration
    {
        /// <summary>
        /// Calls a specified REST backend at the specified url and using the specified method and params and parses the response to the appropriate output type
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="queryParams"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected internal virtual async Task<RestApiCallOutput<TOut>> ApiCallAsync<TOut>(string url, Method method = Method.GET, Dictionary<string, string> queryParams = null, object data = null)
        {
            try
            {
                var response = await RestApiClientUtils.ApiCallAsync<TOut>(url, method, queryParams, data);

                if (response.Response.IsSuccessful)
                    LogLastHealthyResponse();

                return response;
            }
            catch(ApiCallException aex)
            {
                if (
                    aex.ResponseStatus == HttpStatusCode.ServiceUnavailable ||
                    aex.ResponseStatus == HttpStatusCode.GatewayTimeout ||
                    aex.ResponseStatus == HttpStatusCode.BadGateway
                )
                    MarkServiceAsDead();

                if (
#if NETCOREAPP3_1
                        aex.ResponseStatus == HttpStatusCode.TooManyRequests ||
#else
                        (int)aex.ResponseStatus == 429 || //Too many requests
#endif

                        aex.ResponseStatus == HttpStatusCode.RequestTimeout
                )
                    MarkServiceAsUnHealthy();

                //other codes so far ok - 400, 404, etc.

                //how about 500 - this may mean external api is problematic with given call, but may not be dead really - handle this via extension hook at the discretion of the actual api client implementation
                HandleCustomErrors(aex);

                throw;
            }
        }

        /// <summary>
        /// Extension hook for handling custom errors based on the exception data
        /// </summary>
        protected internal virtual void HandleCustomErrors(ApiCallException aex)
        {
            
        }
    }
}
