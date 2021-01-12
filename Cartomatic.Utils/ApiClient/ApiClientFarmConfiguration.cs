using System;
using System.Collections.Generic;
using System.Text;

namespace Cartomatic.Utils.ApiClient
{
    public abstract class ApiClientFarmConfiguration : IApiClientFarmConfiguration
    {
        /// <inheritdoc />
        public bool? MonitorHealth { get; set; }
        
        /// <inheritdoc />
        public int? HealthCheckIntervalSeconds { get; set; }

        /// <inheritdoc />
        public int? AllowedHealthCheckFailures { get; set; }
    }
}
