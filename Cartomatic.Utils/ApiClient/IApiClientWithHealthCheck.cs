using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.ApiClient
{
    public interface IApiClientWithHealthCheck<T> : IApiClient<T>, IApiClientWithHealthCheck
        where T : IApiClientConfiguration
    {
    }

    public interface IApiClientWithHealthCheck
    {
        /// <summary>
        /// Current health status if provided
        /// </summary>
        HealthStatus? HealthStatus { get; }

        /// <summary>
        /// Last time a health check has been performed (in ticks)
        /// </summary>
        long? LastHealthCheckTime { get; }

        /// <summary>
        /// Last time a healthy response has been retrieved (in ticks)
        /// </summary>
        long? LastHealthyResponseTime { get; }

        /// <summary>
        /// Last time an unhealthy response has been retrieved (in ticks)
        /// </summary>
        long? LastUnHealthyResponseTime { get; }

        /// <summary>
        /// initiates a health check procedure
        /// </summary>
        /// <returns></returns>
        Task CheckHealthStatusAsync();

        /// <summary>
        /// returns current health check data if any
        /// </summary>
        /// <returns></returns>
        IHealthCheckData LastHealthCheckData { get; }

        /// <summary>
        /// Marks client as healthy
        /// </summary>
        void MarkAsHealthy();
    }
}
