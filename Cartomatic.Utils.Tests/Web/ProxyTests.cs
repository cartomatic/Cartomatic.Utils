using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Cartomatic.Utils.Web.Tests
{
    [TestFixture]
    public class WmsProxyTests
    {
        [Test] public void UrlExtractor_WhenCalled_ExtractsUrlAsRequired()
        {
            //runs only in full framework, but the runner is in netcore currently
            //var proxiedUrl = "http://some.other.url.com/?p1=param1&p2=param2";
            //var url = $"http://some.url.com/?url={proxiedUrl}";

            //url.ExtractProxiedUrl().Should().Be(proxiedUrl);
        }
    }
}
