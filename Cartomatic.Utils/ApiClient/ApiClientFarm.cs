using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.ApiClient
{
    /// <summary>
    /// An abstract for using API client farms; provides basic functionality for setting up a farm and retrieving clients
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ApiClientFarm<T> : IApiClientFarm<T>
        where T : IApiClient
    {
        /// <summary>
        /// Client configurations
        /// </summary>
        protected internal IEnumerable<IApiClientConfiguration> ClientConfigs { get; set; }
        IEnumerable<IApiClientConfiguration> IApiClientFarm<T>.ClientConfigs { get => ClientConfigs; set => ClientConfigs = value.ToList(); }


        /// <summary>
        /// Counter used to calculate next client to be used
        /// </summary>
        internal int  UsageCounter { get; set; }
        

        /// <summary>
        /// Gets the idx of the next available mc config. uses round robin internally
        /// </summary>
        /// <returns></returns>
        internal int GetNextConfigIdx()
        {
            //make sure to handle int overflow
            return Math.Abs(++UsageCounter) % (ClientConfigs.Count());
        }

        /// <summary>
        /// Tests if can return a client
        /// </summary>
        internal void TestClientAvailability()
        {
            if(ClientConfigs == null || !ClientConfigs.Any())
                throw new InvalidOperationException("There are no clients configured for this farm.");

            //TODO - when marking inactive / inaccessible clients is implemented should test for the 'actual' client availability too
        }

        /// <summary>
        /// Gets a next available configured client
        /// </summary>
        /// <returns></returns>
        public T GetClient()
        {
            TestClientAvailability();
            return GetClient(ClientConfigs.ToArray()[GetNextConfigIdx()]);
        }
        IApiClient IApiClientFarm<T>.GetClient() => GetClient();

        /// <summary>
        /// Creates a client instance for the provided config
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        private T GetClient(IApiClientConfiguration cfg)
        {
            var client = (T)Activator.CreateInstance(typeof(T));
            client.SetConfig(cfg);
            client.Init();
            return client;
        }

        /// <summary>
        /// Gets client by id
        /// </summary>
        /// <param name="endPointId"></param>
        /// <returns></returns>
        public T GetClient(string endPointId)
        {
            TestClientAvailability();

            var cfg = ClientConfigs.FirstOrDefault(cf => cf.EndPointId == endPointId);

            if (cfg == null)
                return (T)(object)null;

            return GetClient(cfg);
        }
        IApiClient IApiClientFarm<T>.GetClient(string endPointId) => GetClient(endPointId);
    }
}
