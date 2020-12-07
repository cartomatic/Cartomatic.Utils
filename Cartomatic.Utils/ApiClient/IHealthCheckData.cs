using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Cartomatic.Utils.ApiClient
{
    /// <summary>
    /// Health check call output data container
    /// </summary>
    public interface IHealthCheckData
    {
        /// <summary>
        /// Raw response object
        /// </summary>
        HttpStatusCode ResponseStatus { get; set; }
    }
    
}
