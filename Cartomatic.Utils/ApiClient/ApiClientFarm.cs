using Cartomatic.Utils.Email;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        internal long UsageCounter { get; set; }


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
            if (ClientConfigs == null || !ClientConfigs.Any())
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

            //allows for temporarily omitting a client based on health check data - for example if client is too busy to handle new requests swiftly
            if (clientOk && SkipClientBasedOnHealthCheckData(clientWithHealthCheck))
                return false;

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
                    MarkClientAsDead(client, 0, $"Health check failed {UnhealthyClients[client]} times");
                }
            }
        }

        /// <summary>
        /// Whether or not a client should be skipped based on the health check data;
        /// extension hook for allowing extra client tests based on the actual data a health check returned
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected abstract bool SkipClientBasedOnHealthCheckData(IApiClientWithHealthCheck client);

        /// <summary>
        /// Reports client status to a set of configured emails
        /// </summary>
        /// <returns></returns>
        protected void ReportClientStatus(IApiClient client, HealthStatus status)
        {
            var emailTpl = GetClienStatustNotificationEmailTitleAndMsg(client, status);

            if (Config.EmailSender == null || Config.ClientStatusNotificationEmails == null ||
                !Config.ClientStatusNotificationEmails.Any())
                return;

            var emailSender = new EmailSender();

            foreach (var email in Config.ClientStatusNotificationEmails)
            {
                emailSender.Send(Config.EmailSender, emailTpl, email);
            }
        }

        /// <summary>
        /// Returns a title for dead client notification email
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected virtual EmailTemplate GetClienStatustNotificationEmailTitleAndMsg(IApiClient client, HealthStatus status)
        {
            var title = string.Empty;
            var msg = string.Empty;

            switch (status)
            {
                case HealthStatus.Dead:
                    title = "Api client DEAD!";
                    msg = "One of the configured api clients went dead.";
                    break;
                case HealthStatus.Healthy:
                    title = "Api client ALIVE!";
                    msg = "Api client has been reactivated.";
                    break;
            }

            title = $"[{GetApiName()}] {title}";

            msg = $@"{msg}

Client details:
{JsonConvert.SerializeObject(GetClientData(client), Formatting.Indented)}
";

            return new EmailTemplate
            {
                Title = title,
                Body = msg,
                IsBodyHtml = false
            };
        }

        /// <summary>
        /// Gets a user friendly api name for the email communication
        /// </summary>
        /// <returns></returns>
        protected abstract string GetApiName();

        /// <summary>
        /// Returns client specific data
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected virtual Dictionary<string, object> GetClientData(IApiClient client)
        {
            if (client == null)
                return null;

            var clientData = new Dictionary<string, object>
            {
                { nameof(client.EndPointId), client.EndPointId },
                { nameof(client.EndPointUrl), client.EndPointUrl }
            };


            if (client is IApiClientWithHealthCheck hcClient)
            {
                clientData.Add(nameof(hcClient.HealthStatus), hcClient.HealthStatus);
                clientData.Add(nameof(hcClient.LastHealthyResponseTime), hcClient.LastHealthyResponseTime);
                clientData.Add(nameof(hcClient.LastHealthCheckTime), hcClient.LastHealthCheckTime);
                clientData.Add(nameof(hcClient.LastHealthCheckData), hcClient.LastHealthCheckData);
                clientData.Add(nameof(hcClient.LastUnHealthyResponseTime), hcClient.LastUnHealthyResponseTime);
                clientData.Add(nameof(hcClient.DeadReason), hcClient.DeadReason);
                clientData.Add(nameof(hcClient.DeadReasonMessage), hcClient.DeadReasonMessage);
            }

            return clientData;
        }

        /// <summary>
        /// Returns client data by endpoint id;
        /// extension hook
        /// </summary>
        /// <param name="endPointId"></param>
        public virtual object GetClientData(string endPointId)
        {
            return GetClientData(GetClient(endPointId)); ;
        }

        /// <summary>
        /// Returns data for all clients
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<object> GetClientData()
        {
            return ClientConfigs.Select(cfg => GetClientData(GetClient(cfg)));
        }

        /// <summary>
        /// Marks client as healthy - this is to bring a client back to live once it went dead and then was fixed
        /// </summary>
        /// <param name="endPointId"></param>
        public void MarkClientAsHealthy(string endPointId)
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

            ReportClientStatus(client, HealthStatus.Healthy);
        }

        /// <summary>
        /// Marks client as dead
        /// </summary>
        /// <param name="endPointId"></param>
        /// <param name="statusCode"></param>
        /// <param name="msg"></param>
        public void MarkClientAsDead(string endPointId, HttpStatusCode statusCode, string msg)
        {
            var client = GetClient(endPointId);
            if (client is IApiClientWithHealthCheck clientWithHealthCheck)
            {
                MarkClientAsDead(clientWithHealthCheck, statusCode, msg);
            }
        }

        /// <summary>
        /// Marks client as dead
        /// </summary>
        /// <param name="client"></param>
        /// <param name="statusCode"></param>
        /// <param name="msg"></param>
        public void MarkClientAsDead(IApiClientWithHealthCheck client, HttpStatusCode statusCode, string msg)
        {
            client.MarkAsDead(statusCode, msg);
            ReportClientStatus(client as IApiClient, HealthStatus.Dead);
        }
    }
}
