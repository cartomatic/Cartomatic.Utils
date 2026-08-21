using System.Collections.Generic;

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
        /// client configs
        /// </summary>
        IEnumerable<IApiClientConfiguration> ClientConfigs { get; set; }
        
        /// <summary>
        /// Gets a client
        /// </summary>
        /// <returns></returns>
        IApiClient GetClient();

        /// <summary>
        /// Gets a client with a specified id
        /// </summary>
        /// <param name="endPointId"></param>
        /// <returns></returns>
        IApiClient GetClient(string endPointId);
    }
}
