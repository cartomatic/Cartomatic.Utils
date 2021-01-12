using System;
using System.Collections.Generic;
using System.Text;

namespace Cartomatic.Utils.ApiClient
{
    public abstract class ApiClientFarmConfiguration : IApiClientFarmConfiguration
    {
        public bool? MonitorHealth { get; set; }
        public int? HealthCheckIntervalSeconds { get; set; }
    }
}
