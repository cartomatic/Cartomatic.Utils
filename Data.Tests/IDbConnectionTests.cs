using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Cartomatic.Utils.Data;

namespace Data.Tests
{
    [TestFixture]
    public class IDbConnectionTests
    {
        [Test]
        public void ConnectionClose_WhenCalled_ProperlyClosesConnection()
        {
            var conn = new Connection();
            conn.Open();
            var stateAfterOpen = conn.State;

            conn.CloseConnection();
            var stateAfterClose = conn.State;

            stateAfterOpen.Should().Be(ConnectionState.Open);
            stateAfterClose.Should().Be(ConnectionState.Closed);
        }

        class Connection : IDbConnection
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public IDbTransaction BeginTransaction()
            {
                throw new NotImplementedException();
            }

            public IDbTransaction BeginTransaction(IsolationLevel il)
            {
                throw new NotImplementedException();
            }

            public void Close()
            {
                State = ConnectionState.Closed;
            }

            public void ChangeDatabase(string databaseName)
            {
                throw new NotImplementedException();
            }

            public IDbCommand CreateCommand()
            {
                throw new NotImplementedException();
            }

            public void Open()
            {
                State = ConnectionState.Open;
            }

            public string ConnectionString { get; set; }
            public int ConnectionTimeout { get; private set; }
            public string Database { get; private set; }
            public ConnectionState State { get; private set; }
        }
    }
}
