using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Anssi;
using Org.BouncyCastle.Utilities;

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
        /// api farm configuration
        /// </summary>
        public IApiClientFarmConfiguration Config { get; set; }


        protected IApiClientConfiguration[] _clientConfigsArr;

        /// <summary>
        /// Client configurations
        /// </summary>
        protected internal IEnumerable<IApiClientConfiguration> ClientConfigs { get; set; }
        IEnumerable<IApiClientConfiguration> IApiClientFarm<T>.ClientConfigs { get => ClientConfigs; set => ClientConfigs = value; }

        /// <summary>
        /// Returns client config by id
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        protected internal IApiClientConfiguration GetClientConfig(int idx)
        {
            _clientConfigsArr ??= ClientConfigs.ToArray();
            return _clientConfigsArr[idx];
        }

        /// <summary>
        /// Counter used to calculate next client to be used
        /// </summary>
        internal long  UsageCounter { get; set; }
        

        /// <summary>
        /// Gets the idx of the next available mc config. uses round robin internally
        /// </summary>
        /// <returns></returns>
        internal int GetNextConfigIdx()
        {
            return (int)(++UsageCounter % ClientConfigs.Count());
        }

        /// <summary>
        /// Gets the idx of the next mc config in row. uses round robin internally
        /// </summary>
        /// <returns></returns>
        internal int GetNextConfigIdx(int idx)
        {
            return ++idx % ClientConfigs.Count();
        }

        internal const string NO_CLIENTS_CONFIGURED_ERR_MSG = "There are no clients configured for this farm.";

        /// <summary>
        /// Tests if can return a client
        /// </summary>
        internal void TestClientAvailability()
        {
            if(ClientConfigs == null || !ClientConfigs.Any())
                throw new InvalidOperationException(NO_CLIENTS_CONFIGURED_ERR_MSG);
        }

        /// <summary>
        /// Gets a next available configured client
        /// </summary>
        /// <returns></returns>
        /// <remarks>when a client implements IApiClientWithHealthCheck then a health check tracking is also performed</remarks>
        public T GetClient()
        {
            var client = default(T);

            Task.WaitAll(Task.Run(async () =>
            {
                client = await GetClientAsync();
            }));

            return client;
        }

        internal const string NO_HEALTHY_CLIENTS_ERR_MSG = "No healthy clients left in the farm.";

        /// <summary>
        /// Gets a next available configured client;
        /// </summary>
        /// <returns></returns>
        /// <remarks>when a client implements IApiClientWithHealthCheck then a health check tracking is also performed</remarks>
        public async Task<T> GetClientAsync()
        {
            TestClientAvailability();

            var clientCfgIdx = GetNextConfigIdx();
            var cfg = GetClientConfig(clientCfgIdx);
            
            var client = GetClient(cfg);

            var clientHealthy = await CheckIfClientHealthy(client);
            while (!clientHealthy)
            {
                clientCfgIdx = GetNextConfigIdx(clientCfgIdx);
                cfg = GetClientConfig(clientCfgIdx);
                var newClient = GetClient(cfg);

                //spin until client repeats. if all were dead and we come back to the initial client, then BOOM
                if (Equals(newClient, client))
                {
                    throw new Exception(NO_HEALTHY_CLIENTS_ERR_MSG);
                }

                clientHealthy = await CheckIfClientHealthy(newClient);
                if (clientHealthy)
                    client = newClient;
            }

            return client;
        }
        
        /// <summary>
        /// cached client instances, so can retrieve updated data such as health status
        /// </summary>
        private readonly Dictionary<IApiClientConfiguration, IApiClient> ClientInstances = new Dictionary<IApiClientConfiguration, IApiClient>();

        /// <summary>
        /// Creates a client instance for the provided config
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        private T GetClient(IApiClientConfiguration cfg)
        {
            if (ClientInstances.ContainsKey(cfg))
                return (T)ClientInstances[cfg];

            var client = (T)Activator.CreateInstance(typeof(T));
            client.SetConfig(cfg);
            client.Init();

            ClientInstances[cfg] = client;

            return client;
        }

        /// <summary>
        /// Gets client by endpoint id
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


        /// <summary>
        /// Whether or not client is healthy; always true when farm configuration MonitorHealth is falsy or client is not IApiClientWithHealthCheck
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected internal async Task<bool> CheckIfClientHealthy(IApiClient client)
        {
            //monitoring not configured or client does not support health checking, so always ok
            if (Config?.MonitorHealth != true || !(client is IApiClientWithHealthCheck clientWithHealthCheck))
                return true;

            var shouldCheckHealth = ShouldCheckHealth(clientWithHealthCheck);

            if (shouldCheckHealth)
                await clientWithHealthCheck.CheckHealthStatusAsync();

            var clientOk = clientWithHealthCheck.HealthStatus == HealthStatus.Healthy;


            if (!clientOk)
            {
                //uhuh, client not healthy, so need to mark it as unhealthy internally and keep track of this
                await HandleUnHealthyClient(clientWithHealthCheck, shouldCheckHealth);
            }

            return clientOk;
        }

        /// <summary>
        /// Whether or not a health check should be performed for given client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected internal bool ShouldCheckHealth(IApiClientWithHealthCheck client)
        {

            var shouldCheckHealth =
                //no need to test dead clients - dead means dead
                client.HealthStatus != HealthStatus.Dead &&

                (
                    //not yet tested
                    !(client.LastHealthCheckTime.HasValue ||
                      client.LastHealthyResponseTime.HasValue) ||

                    //both health checks expired - when one is ok, we're still good
                    !(
                        client.LastHealthCheckTime.HasValue &&
                        new TimeSpan(DateTime.Now.Ticks - client.LastHealthCheckTime.Value).TotalSeconds < Config.HealthCheckIntervalSeconds ||

                        client.LastHealthyResponseTime.HasValue &&
                        new TimeSpan(DateTime.Now.Ticks - client.LastHealthyResponseTime.Value).TotalSeconds < Config.HealthCheckIntervalSeconds
                    )
                    
                );

            return shouldCheckHealth;
        }


        /// <summary>
        /// keeps track of unhealthy clients
        /// </summary>
        protected Dictionary<IApiClientWithHealthCheck, int> UnhealthyClients = new Dictionary<IApiClientWithHealthCheck, int>();

        /// <summary>
        /// Tracks unhealthy client status
        /// </summary>
        /// <param name="client"></param>
        /// <param name="healthChecked"></param>
        protected async Task HandleUnHealthyClient(IApiClientWithHealthCheck client, bool healthChecked)
        {
            //ignore dead clients. once dead, always dead
            if (client.HealthStatus == HealthStatus.Dead)
                return;

            //only handle clients that were just health-checked; health check for other clients is pending
            if (healthChecked)
            {
                if (!UnhealthyClients.ContainsKey(client))
                {
                    UnhealthyClients[client] = 0;
                }

                UnhealthyClients[client]++;

                if (Config.AllowedHealthCheckFailures.HasValue &&
                    UnhealthyClients[client] > Config.AllowedHealthCheckFailures)
                {
                    client.MarkAsDead(0, $"Health check failed {UnhealthyClients[client]} times");
                    await ReportDeadClient(client as IApiClient);
                }
            }
        }

        /// <summary>
        /// Whether or not a client should be skipped based on the health check data;
        /// extension hook for allowing extra client tests based on the actual data a health check returned
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>Basic extension for obtaining the IHealthCheckData is via client's CheckHealthStatusAsync</remarks>
        /// <returns></returns>
        protected abstract bool SkipClientBasedOnHealthCheckData(IHealthCheckData data);

        /// <summary>
        /// Reports a dead client;
        /// extension hook
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected abstract Task ReportDeadClient(IApiClient client);

        /// <summary>
        /// Reports client data;
        /// extension hook
        /// </summary>
        /// <param name="endPointId"></param>
        public abstract Task ReportClientData(string endPointId);

        /// <summary>
        /// Marks client as healthy - this is to bring a client back to live once it went dead and then was fixed
        /// </summary>
        /// <param name="endPointId"></param>
        public virtual void MarkClientAsHealthy(string endPointId)
        {
            var client = GetClient(endPointId);
            if (client is IApiClientWithHealthCheck clientWithHealthCheck)
            {
                clientWithHealthCheck.MarkAsHealthy();

                //reset client unhealthy status counters
                if (UnhealthyClients.ContainsKey(clientWithHealthCheck))
                {
                    UnhealthyClients[clientWithHealthCheck] = 0;
                }
            }
        }
    }
}
