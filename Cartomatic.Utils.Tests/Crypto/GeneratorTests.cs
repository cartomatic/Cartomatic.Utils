using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Cartomatic.Utils.Crypto;
using FluentAssertions;

namespace Crypto.Tests
{
    [TestFixture]
    public class GeneratorTests
    {
        [Test]
        public void StringGenerator_ShouldGenerate_StringOfProperLength()
        {
            var len = 15;
            var rndStr = Generator.GenerateRandomString(len);
            rndStr.Length.Should().Be(len);
        }
    }
}
