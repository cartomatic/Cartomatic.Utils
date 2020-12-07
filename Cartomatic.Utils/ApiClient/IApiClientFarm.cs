using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.ApiClient
{
    /// <summary>
    /// Enforces ApiClient farm functionality
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApiClientFarm<T>
        where T : IApiClient
    {
        /// <summary>
        /// Client farm configuration
        /// </summary>
        IApiClientFarmConfiguration Config { get; set; }

        /// <summary>
        /// client configs
        /// </summary>
        IEnumerable<IApiClientConfiguration> ClientConfigs { get; set; }
        
        /// <summary>
        /// Gets a client
        /// </summary>
        /// <returns></returns>
        T GetClient();

        /// <summary>
        /// Gets a client
        /// </summary>
        /// <returns></returns>
        Task<T> GetClientAsync();

        /// <summary>
        /// Gets a client with a specified id
        /// </summary>
        /// <param name="endPointId"></param>
        /// <returns></returns>
        T GetClient(string endPointId);
    }
}
