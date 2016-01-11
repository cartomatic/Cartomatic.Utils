using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartomatic.Utils.Data;
using NUnit.Framework.Internal;
using NUnit.Framework;
using FluentAssertions;

namespace Data.Tests
{
    [TestFixture]
    public class DataSourceCredentialsExtensionsTests
    {
        [Test]
        public void GetDbConnection_WhenDataSourceTypeIsPgSql_ReturnsNpgsqlConnection()
        {
            var dbc = new DataSourceCredentials()
            {
                DataSourceType = DataSourceType.PgSql
            };

            var conn = dbc.GetDbConnectionObject();

            conn.GetType().FullName.Should().Be("Npgsql.NpgsqlConnection");
        }

        [Test]
        public void GetDbCommandObject_WhenDataSourceTypeIsPgSql_ReturnsNpgsqlCommand()
        {
            var dbc = new DataSourceCredentials()
            {
                DataSourceType = DataSourceType.PgSql
            };

            var conn = dbc.GetDbCommandObject();

            conn.GetType().FullName.Should().Be("Npgsql.NpgsqlCommand");
        }

        [Test]
        public void GetDbDataAdapterObject_WhenDataSourceTypeIsPgSql_ReturnsNpgsqlDataAdapter()
        {
            var dbc = new DataSourceCredentials()
            {
                DataSourceType = DataSourceType.PgSql
            };

            var conn = dbc.GetDbDataAdapterObject();

            conn.GetType().FullName.Should().Be("Npgsql.NpgsqlDataAdapter");
        }

        [Test]
        public void GetDbConnection_WhenDataSourceTypeIsSqlServer_ReturnsSqlClientSqlConnectionn()
        {
            var dbc = new DataSourceCredentials()
            {
                DataSourceType = DataSourceType.SqlServer
            };

            var conn = dbc.GetDbConnectionObject();

            conn.GetType().FullName.Should().Be("System.Data.SqlClient.SqlConnection");
        }

        [Test]
        public void GetDbCommandObject_WhenDataSourceTypeIsSqlServer_ReturnsSqlClientSqlCommandn()
        {
            var dbc = new DataSourceCredentials()
            {
                DataSourceType = DataSourceType.SqlServer
            };

            var conn = dbc.GetDbCommandObject();

            conn.GetType().FullName.Should().Be("System.Data.SqlClient.SqlCommand");
        }

        [Test]
        public void GetDbDataAdapterObject_WhenDataSourceTypeIsSqlServer_ReturnsSqlClientSqlDataAdapter()
        {
            var dbc = new DataSourceCredentials()
            {
                DataSourceType = DataSourceType.SqlServer
            };

            var conn = dbc.GetDbDataAdapterObject();

            conn.GetType().FullName.Should().Be("System.Data.SqlClient.SqlDataAdapter");
        }

    }
}
