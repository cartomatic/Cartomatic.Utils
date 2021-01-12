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
    public class ApiClientFarmTests
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

        [Test]
        public void ShouldCheckHealthWhenClientDeadReturnsFalse()
        {
            var farm = GetTestableApiClientFarm();

            var client = farm.GetClient();
            client.HealthStatus = HealthStatus.Dead;

            farm.ShouldCheckHealth(client).Should().BeFalse();
        }

        [Test]
        public void ShouldCheckHealthWhenClientNotTestedBeforeReturnsTrue()
        {
            var farm = GetTestableApiClientFarm();

            var client = farm.GetClient();
            
            farm.ShouldCheckHealth(client).Should().BeTrue();
        }

        [Test]
        public void ShouldCheckHealthWhenLastHealthCheckTimeOutOfRangeReturnsTrue()
        {
            var farm = GetTestableApiClientFarm();
            var client = farm.GetClient();

            client.LastHealthCheckTime = DateTime.Now.AddSeconds(-((farm.Config.HealthCheckIntervalSeconds ?? 0) + 1)).Ticks;

            farm.ShouldCheckHealth(client).Should().BeTrue();
        }

        [Test]
        public void ShouldCheckHealthWhenLastHealthyResponseTimeOutOfRangeReturnsTrue()
        {
            var farm = GetTestableApiClientFarm();
            var client = farm.GetClient();

            client.LastHealthyResponseTime = DateTime.Now.AddSeconds(-((farm.Config.HealthCheckIntervalSeconds ?? 0) + 1)).Ticks;

            farm.ShouldCheckHealth(client).Should().BeTrue();
        }

        [Test]
        public void ShouldCheckHealthWhenLastHealthCheckTimeInRangeReturnsFalse()
        {
            var farm = GetTestableApiClientFarm();
            var client = farm.GetClient();

            client.LastHealthCheckTime = DateTime.Now.Ticks;
            
            farm.ShouldCheckHealth(client).Should().BeFalse();
        }

        [Test]
        public void ShouldCheckHealthWhenLastyHealthyResponseTimeInRangeReturnsFalse()
        {
            var farm = GetTestableApiClientFarm();
            var client = farm.GetClient();

            client.LastHealthyResponseTime = DateTime.Now.Ticks;

            farm.ShouldCheckHealth(client).Should().BeFalse();
        }

        TestableApiClientFarm GetTestableApiClientFarm() => new TestableApiClientFarm
        {
            ClientConfigs = new[] { new TestApiClientConfiguration() }, 
            Config = new TestableApiClientFarmConfiguration
            {
                HealthCheckIntervalSeconds = 1,
                MonitorHealth = false //false, so the default health check is skipped when obtaining a client
            }
        };

    }

    class TestableRestApiClient : RestApiClientWithHealthCheck<ApiClientConfiguration>
    {
        protected internal override void Init() { }

        public override async Task CheckHealthStatusAsync()
        {
            HealthStatus = ApiClient.HealthStatus.Healthy;
            LastHealthCheckTime = DateTime.Now.Ticks;
        }

        protected internal override (HealthStatus? clientStatus, string message) HandleCustomErrors(
            HealthStatus? clientStatus, ApiCallException aex) => (null, null);
    }

    class TestableApiClientFarmConfiguration : ApiClientFarmConfiguration { }

    class TestableApiClientFarm : ApiClientFarm<TestableRestApiClient>
    {
        protected override bool SkipClientBasedOnHealthCheckData(IHealthCheckData data) => false;

        protected override Task ReportDeadClient(IApiClient client)
        {
            throw new NotImplementedException();
        }

        public override Task ReportClientData(string endPointId)
        {
            throw new NotImplementedException();
        }
    }

    class TestApiClientConfiguration : ApiClientConfiguration { }
}
