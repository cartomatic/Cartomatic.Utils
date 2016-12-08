using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.Data
{
    public class DataSourceCredentials : IDataSourceCredentials
    {

        /// <summary>
        /// Type of the data source the credentials apply to
        /// </summary>
        public DataSourceType DataSourceType { get; set; }

        public string ServerHost { get; set; }

        public string ServerName { get; set; }

        public int? ServerPort { get; set; }

        public string DbName { get; set; }

        public string ServiceDb { get; set; }

        public string UserName { get; set; }

        public string Pass { get; set; }

        public string ServiceUserName { get; set; }

        public string ServiceUserPass { get; set; }



        /// <summary>
        /// Whether or not default service Db name should be used when db name not provided - applicable to PgSql only
        /// </summary>
        public bool UseDefaultServiceDb { get; set; }




        public DataSourceCredentials()
        {
            UseDefaultServiceDb = true;
        }

        /// <summary>
        /// Returns a default Db name
        /// </summary>
        /// <returns></returns>
        protected internal string GetDefaultDbName()
        {
            string dbName = string.Empty;

            switch (DataSourceType)
            {
                case DataSourceType.PgSql:
                    dbName = "postgres";
                    break;
            }

            return dbName;
        }

        /// <summary>
        /// Returns a connection string for the configured data source type
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString(bool serviceDatabase = false, bool superUser = false)
        {
            string conn = "INVALID CONNECTION STRING";

            switch (DataSourceType)
            {
                case DataSourceType.PgSql:
                    conn =
                        "Server=" + ServerHost + ";" +
                        "Port=" + ServerPort + ";" +
                        "Database=" + (serviceDatabase ? (string.IsNullOrEmpty(ServiceDb) ? "postgres" : ServiceDb) : (string.IsNullOrEmpty(DbName) ? (UseDefaultServiceDb ? "postgres" : "") : DbName)) + ";" +
                        "user id=" + (serviceDatabase || superUser ? (string.IsNullOrEmpty(ServiceUserName) ? "postgres" : ServiceUserName) : UserName) + ";" +
                        "password=" + (serviceDatabase || superUser ? (string.IsNullOrEmpty(ServiceUserPass) ? "postgres" : ServiceUserPass) : Pass) + ";";
                    break;

                case DataSourceType.SqlServer:
                    conn =
                        "server=" + ServerHost + (ServerPort.HasValue ? "," + ServerPort : "") + ";" +
                        "user id=" + (serviceDatabase || superUser ? ServiceUserName : UserName) + ";" +
                        "password=" + (serviceDatabase || superUser ? ServiceUserPass : Pass) + ";" +
                        "database=" + (serviceDatabase ? ServiceDb : DbName) + ";";
                    break;

                case DataSourceType.Oracle:
                    throw new NotImplementedException();
                    break;
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