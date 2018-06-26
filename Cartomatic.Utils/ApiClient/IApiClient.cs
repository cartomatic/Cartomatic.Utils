using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.ApiClient
{
    public interface IApiClient<T> : IApiClient
        where T : IApiClientConfiguration
    {
    }

    public interface IApiClient
    {
        /// <summary>
        /// Sets the configuration object
        /// </summary>
        /// <param name="cfg"></param>
        void SetConfig(IApiClientConfiguration cfg);

        /// <summary>
        /// Performs client internal initialization
        /// </summary>
        void Init();

        /// <summary>
        /// Returns an endpoint confogured via client confoguration object
        /// </summary>
        string EndPointId { get; }
    }
}
