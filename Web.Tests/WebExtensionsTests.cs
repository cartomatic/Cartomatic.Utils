using FluentAssertions;
using NUnit.Framework;

namespace Cartomatic.Utils.Web.Tests
{
    [TestFixture]
    public class WebExtnsionTests
    {
        [Test]
        public void CreateHttpWebRequest_WhenPassingUrlString_CreatesWebRequestWithProperUrl()
        {
            var url = "http://subdomain.somesite.net:1234/p1/p2/?x=y";
            var wr = url.CreateHttpWebRequest();

            wr.RequestUri.AbsoluteUri.Should().Be(url);
        }
    }
}
