

namespace Cartomatic.Utils.Data
{
    /// <summary>
    /// Enforces availability of properties that allow for connecting to a data source
    /// </summary>
    public interface IDataSourceCredentials
    {
        /// <summary>
        /// data source provider
        /// </summary>
        DataSourceProvider DataSourceProvider { get; set; }

        /// <summary>
        /// server host
        /// </summary>
        string ServerHost { get; set; }

        /// <summary>
        /// server name
        /// </summary>
        string ServerName { get; set; }

        /// <summary>
        /// server port
        /// </summary>
        int? ServerPort { get; set; }

        /// <summary>
        /// name of a database
        /// </summary>
        string DbName { get; set; }

        /// <summary>
        /// name of a service database
        /// </summary>
        string ServiceDb { get; set; }

        /// <summary>
        /// user name
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// user pass
        /// </summary>
        string Pass { get; set; }

        /// <summary>
        /// service db user name
        /// </summary>
        string ServiceUserName { get; set; }

        /// <summary>
        /// service db user pass
        /// </summary>
        string ServiceUserPass { get; set; }

        /// <summary>
        /// gets a connection string for given credentials
        /// </summary>
        /// <param name="serviceDatabase">Whether or not should connect to a service database</param>
        /// <param name="superUser">Whether or not should connect as super user</param>
        /// <param name="pooling">whether or not connection should be pooled</param>
        /// <returns></returns>
        string GetConnectionString(bool serviceDatabase = false, bool superUser = false, bool pooling = true);
    }
}