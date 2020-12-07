using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cartomatic.Utils.ApiClient
{
    /// <summary>
    /// Api client base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ApiClientWithHealthCheck<T> : ApiClient<T>, IApiClientWithHealthCheck<T>
        where T : IApiClientConfiguration
    {
        /// <summary>
        /// Last api health check data returned by the backend
        /// </summary>
        public IHealthCheckData LastHealthCheckData { get; protected set; }

        /// <summary>
        /// current health check status
        /// </summary>
        public HealthStatus? HealthStatus { get; protected set; }

        /// <summary>
        /// Last time a health check has been performed (in ticks)
        /// </summary>
        public long? LastHealthCheckTime { get; protected set; }

        /// <summary>
        /// Checks current health status of a backend service
        /// </summary>
        /// <returns></returns>
        /// <remarks>Default implementation always sets health check status to healthy</remarks>
        public virtual async Task CheckHealthStatusAsync()
        {
            HealthStatus = ApiClient.HealthStatus.Healthy;
            LastHealthCheckTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Last time a healthy response has been retrieved (in ticks)
        /// </summary>
        public long? LastHealthyResponseTime { get; protected set; }

        /// <summary>
        /// Logs time of a last healthy response
        /// </summary>
        protected internal virtual void LogLastHealthyResponse()
        {
            HealthStatus = ApiClient.HealthStatus.Healthy;
            LastHealthyResponseTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Last time an unhealthy response has been retrieved (in ticks)
        /// </summary>
        public long? LastUnHealthyResponseTime { get; protected set; }

        /// <summary>
        /// Marks client as dead
        /// </summary>
        protected internal virtual void MarkServiceAsDead()
        {
            LastUnHealthyResponseTime = DateTime.Now.Ticks;
            HealthStatus = ApiClient.HealthStatus.Dead;
        }

        /// <summary>
        /// Marks client as unhealthy
        /// </summary>
        protected internal virtual void MarkServiceAsUnHealthy()
        {
            LastUnHealthyResponseTime = DateTime.Now.Ticks;
            HealthStatus = ApiClient.HealthStatus.Unhealthy;
        }
    }
}
