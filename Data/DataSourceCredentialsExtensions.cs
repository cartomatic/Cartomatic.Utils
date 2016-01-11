using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Reflection;
using Npgsql;

namespace Cartomatic.Utils.Data
{
    public static class DataSourceCredentialsExtensions
    {
        //TODO - make it dynamically resolve types! So the util is not so tightly coupled with different drivers as it sucks!

        /// <summary>
        /// Returns a db engine specific Connection object
        /// </summary>
        /// <param name="dbc"></param>
        /// <returns>Db engine specific Connection</returns>
        public static IDbConnection GetDbConnectionObject(this IDataSourceCredentials dbc)
        {
            IDbConnection output = null;

            switch (dbc.DataSourceType)
            {
                case DataSourceType.PgSql:

                    //TODO - make it dynamically resolve types! So the util is not so tightly coupled with different drivers as it sucks!
                    //Assembly assembly = Assembly.Load("Npgsql");
                    //Type t = assembly.GetType("Npgsql.NpgsqlConnection");
                    //ConstructorInfo ctor = t.GetConstructor(new Type[] {typeof (string)});

                    //output = (IDbConnection)ctor.Invoke(new object[] { dbc.GetConnectionString() });
                    
                    output = new NpgsqlConnection(dbc.GetConnectionString());
                    break;

                case DataSourceType.SqlServer:
                    output = new System.Data.SqlClient.SqlConnection(dbc.GetConnectionString());
                    break;

                //case DataSourceType.Oracle:
                //    //TODO - when implementing oracle make it dynamically resolve assemblies as pgsql code does
                //    output = null; //new System.Data.OracleClient.OracleConnection(dbc.connectionString);
                //    break;
            }

            return output;
        }

        /// <summary>
        /// Returns a db engine specific Command object
        /// </summary>
        /// <param name="dbc"></param>
        /// <returns>Db engine specific Command</returns>
        public static IDbCommand GetDbCommandObject(this IDataSourceCredentials dbc)
        {
            IDbCommand output = null;

            switch (dbc.DataSourceType)
            {
                case DataSourceType.PgSql:
                    output = new NpgsqlCommand();
                    break;

                case DataSourceType.SqlServer:
                    output = new System.Data.SqlClient.SqlCommand();
                    break;

                //case DataSourceType.Oracle:
                //    //TODO - when implementing oracle make it dynamically resolve assemblies as pgsql code does
                //    output = null; //new System.Data.OracleClient.OracleCommand();
                //    break;
            }

            return output;
        }

        /// <summary>
        /// Returns a db engine specific DataAdapter object
        /// </summary>
        /// <param name="dbc"></param>
        /// <returns>Db engine specific DataAdapter</returns>
        public static IDbDataAdapter GetDbDataAdapterObject(this IDataSourceCredentials dbc)
        {
            IDbDataAdapter output = null;

            switch (dbc.DataSourceType)
            {
                case DataSourceType.PgSql:
                    output = new NpgsqlDataAdapter();
                    break;

                case DataSourceType.SqlServer:
                    output = new System.Data.SqlClient.SqlDataAdapter();
                    break;

                //case DataSourceType.Oracle:
                //    //TODO - when implementing oracle make it dynamically resolve assemblies as pgsql code does
                //    output = null; //new System.Data.OracleClient.OracleDataAdapter();
                //    break;
            }

            return output;
        }
    }
}
