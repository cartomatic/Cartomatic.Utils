using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.ApiClient
{
    /// <summary>
    /// IApiClient configuration
    /// </summary>
    public interface IApiClientFarmConfiguration
    {
        /// <summary>
        /// Whether or not client farm should monitor health of its clients; client must implement IApiClientWithHealthCheck in order to be health-checked
        /// </summary>
        bool? MonitorHealth { get; set; }

        /// <summary>
        /// How often should the health check be performed
        /// </summary>
        int? HealthCheckIntervalSeconds { get; set; }

        /// <summary>
        /// How many times should a client be health-checked before it is marked as dead
        /// </summary>
        int? AllowedHealthCheckFailures { get; set; }
    }
}
