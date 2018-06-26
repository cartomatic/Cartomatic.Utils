using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.ApiClient
{
    public interface IApiClientFarm<T>
        where T : IApiClient
    {
        IEnumerable<IApiClientConfiguration> ClientConfigs { get; set; }
        
        IApiClient GetClient();

        IApiClient GetClient(string endPointId);
    }
}
