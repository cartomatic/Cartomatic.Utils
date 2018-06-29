using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.ApiClient
{
    public abstract class ApiClient<T> : IApiClient<T>
        where T:IApiClientConfiguration
    {
        protected internal IApiClientConfiguration ClientConfiguration { get; private set; }

        /// <summary>
        /// Returns a client endpoint identifier; Identifier is unique per endpoint as it is computed based on the protocol/host/port combination;
        /// <para/>
        /// this means two independent client instances with the same config are in fact using the same endpoint and therefore share the id
        /// </summary>
        public string EndPointId => ClientConfiguration?.EndPointId;


        /// <summary>
        /// Sets a client configuration
        /// </summary>
        /// <param name="cfg"></param>
        public virtual void SetConfig(IApiClientConfiguration cfg)
        {
            ClientConfiguration = cfg;
        }

        /// <summary>
        /// Internal client initialisation procedure
        /// </summary>
        protected internal abstract void Init();
        void IApiClient.Init() => Init();
    }
}
