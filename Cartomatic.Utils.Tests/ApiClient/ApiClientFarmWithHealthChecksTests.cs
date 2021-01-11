using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Cartomatic.Utils.ApiClient.Tests
{
    [TestFixture]
    public class ApiClientFarmWithHealthChecksTests
    {
        [Test]
        public void GetClient_WhenCalled_ShouldKickUpUsageCounter()
        {
            var farm = ClientFarm.GetTestFarm();

            var calls = 10;
            for (var i = 0; i < calls; i++)
                farm.GetClient();

            farm.UsageCounter.Should().Be(calls);
        }

        [Test]
        public void GetClientById_WhenPassedValidId_ShouldReturnClientWithProperConfiguration()
        {
            var farm = ClientFarm.GetTestFarm();
            var client = farm.GetClient();
            var clientById = farm.GetClient(client.EndPointId);

            client.EndPointId.Should().Be(clientById.EndPointId);
        }

        [Test]
        public void GetClientById_WhenPassedInvalidId_ShouldReturnNull()
        {
            var farm = ClientFarm.GetTestFarm();
            var clientById = farm.GetClient("whatever");

            clientById.Should().BeNull();
        }
    }
}
