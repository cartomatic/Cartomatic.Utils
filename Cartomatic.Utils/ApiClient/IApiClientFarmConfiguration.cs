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

    }
}
