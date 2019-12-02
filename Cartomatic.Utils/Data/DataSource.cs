using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Cartomatic.Utils.Data
{

    /// <summary>
    /// Holds information allowing for connecting to a datasource
    /// </summary>
    public class DataSourceCredentials : IDataSourceCredentials
    {

        /// <summary>
        /// Type of the data source the credentials apply to
        /// </summary>
        public DataSourceProvider DataSourceProvider { get; set; }

        /// <inheritdoc />
        public string ServerHost { get; set; }

        /// <inheritdoc />
        public string ServerName { get; set; }

        /// <inheritdoc />
        public int? ServerPort { get; set; }

        /// <inheritdoc />
        public string DbName { get; set; }

        /// <inheritdoc />
        public string ServiceDb { get; set; }

        /// <inheritdoc />
        public string UserName { get; set; }

        /// <inheritdoc />
        public string Pass { get; set; }

        /// <inheritdoc />
        public string ServiceUserName { get; set; }

        /// <inheritdoc />
        public string ServiceUserPass { get; set; }



        /// <summary>
        /// Whether or not default service Db name should be used when db name not provided - applicable to Npgsql only
        /// </summary>
        public bool UseDefaultServiceDb { get; set; }



#pragma warning disable 1591
        public DataSourceCredentials()
        {
            UseDefaultServiceDb = true;
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns a default Db name
        /// </summary>
        /// <returns></returns>
        protected internal string GetDefaultDbName()
        {
            string dbName = string.Empty;

            switch (DataSourceProvider)
            {
                case DataSourceProvider.Npgsql:
                    dbName = "postgres";
                    break;
            }

            return dbName;
        }

        /// <summary>
        /// Returns a connection string for the configured data source type
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString(bool serviceDatabase = false, bool superUser = false, bool pooling = true)
        {
            string conn = "INVALID CONNECTION STRING";

            switch (DataSourceProvider)
            {
                case DataSourceProvider.Npgsql:
                    conn =
                        "Server=" + ServerHost + ";" +
                        "Port=" + ServerPort + ";" +
                        "Database=" + (serviceDatabase
                                ? (string.IsNullOrEmpty(ServiceDb) ? "postgres" : ServiceDb)
                                : (string.IsNullOrEmpty(DbName) ? (UseDefaultServiceDb ? "postgres" : "") : DbName)) + ";" +
                        "user id=" + (serviceDatabase || superUser
                            ? (string.IsNullOrEmpty(ServiceUserName) ? "postgres" : ServiceUserName)
                            : UserName) + ";" +
                        "password=" + (serviceDatabase || superUser
                            ? (string.IsNullOrEmpty(ServiceUserPass) ? "postgres" : ServiceUserPass)
                            : Pass) + ";" +
                        (pooling ? string.Empty : "Pooling=false;");
                    break;

                case DataSourceProvider.SqlServer:
                    conn =
                        "server=" + ServerHost + (ServerPort.HasValue ? "," + ServerPort : "") + ";" +
                        "user id=" + (serviceDatabase || superUser ? ServiceUserName : UserName) + ";" +
                        "password=" + (serviceDatabase || superUser ? ServiceUserPass : Pass) + ";" +
                        "database=" + (serviceDatabase ? ServiceDb : DbName) + ";";
                    break;

                case DataSourceProvider.Oracle:
                    throw new NotImplementedException();
                    //break;
            }

            return conn;
        }

        /// <summary>
        /// Creates a copy of the object
        /// </summary>
        /// <returns></returns>
        public DataSourceCredentials Clone()
        {
            return (DataSourceCredentials)MemberwiseClone();
        }
    }

}