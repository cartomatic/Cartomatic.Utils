using Cartomatic.Utils.ApiClient.Enums;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace Cartomatic.Utils.ApiClient
{
    /// <summary>
    /// Api client base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ApiClient<T> : IApiClient<T>
        where T : IApiClientConfiguration
    {
        /// <summary>
        /// Api client confoguration
        /// </summary>
        protected internal IApiClientConfiguration ClientConfiguration { get; private set; }

        /// <summary>
        /// Api call output data container
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        public class RestApiCallOutput<TOut>
        {
            /// <summary>
            /// Output deserialized to a specified type
            /// </summary>
            public TOut Output { get; set; }

            /// <summary>
            /// Raw response object
            /// </summary>
            public HttpStatusCode ResponseStatus { get; set; }
        }

        public Dictionary<HealthCheckStatus, string> HealthChecksPool { get; set; }

        public HealthCheckStatus HealthCheckStatus { get; private set; }

        private readonly Func<T, bool> checkClientStatus = CheckClientStatusIsOk;

        /// <summary>
        /// Returns a client endpoint identifier; Identifier is unique per endpoint as it is computed based on the protocol/host/port combination;
        /// <para/>
        /// this means two independent client instances with the same config are in fact using the same endpoint and therefore share the id
        /// </summary>
        public string EndPointId => ClientConfiguration?.EndPointId;



        /// <summary>
        /// Get Client Response Status
        /// </summary>
        /// <returns></returns>
        protected internal virtual RestApiCallOutput<TOut> GetClientResponseStatus<TOut>(
            IApiClientConfiguration cfg)
        {
            var client = new RestClient(cfg.GetUrl());
            var restRequest = new RestRequest("Status", Method.GET);
            var restResponse = client.Execute(restRequest);

            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                HealthCheckStatus = HealthCheckStatus.Healthy;
            }
            else if (restResponse.StatusCode != HttpStatusCode.OK)
            {
                HealthCheckStatus = HealthCheckStatus.Unhealthy;
            }

            HealthChecksPool.Add(HealthCheckStatus, client.ToString());

            return new RestApiCallOutput<TOut>
            {
                Output = (TOut)Newtonsoft.Json.JsonConvert.DeserializeObject(restResponse.Content, typeof(TOut)),
                ResponseStatus = restResponse.StatusCode
            };
        }

        /// <summary>
        /// Try Update Status For Client
        /// </summary>
        /// <param name="client"></param>
        public void TryUpdateStatusForClient(T client)
        {
            foreach (var healthCheck in HealthChecksPool)
            {
                if (healthCheck.Key == HealthCheckStatus.Unhealthy)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        if (checkClientStatus((T)ClientConfiguration))
                        {
                            HealthChecksPool.Remove(healthCheck.Key);
                            HealthChecksPool.Add(HealthCheckStatus.Healthy, client.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check Client status is ok
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static bool CheckClientStatusIsOk(T arg)
        {
            var client = new RestClient(arg.GetUrl());
            var restRequest = new RestRequest("Status", Method.GET);
            var restResponse = client.Execute(restRequest);

            return restResponse.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Sets a client configuration
        /// </summary>
        /// <param name="cfg"></param>
        public virtual void SetConfig(IApiClientConfiguration cfg)
        {
            ClientConfiguration = cfg;
        }

        /// <summary>
        /// Internal client initialisation procedure
        /// </summary>
        protected internal abstract void Init();
        void IApiClient.Init() => Init();
    }
}
