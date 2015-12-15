using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Cartomatic.Utils.Serialisation;
using FluentAssertions;
using NUnit.Framework;

namespace Cartomatic.Serialization.Tests
{
    [TestFixture]
    public class JsonSerializationExtensionsTests
    {
        [Test]
        public void FromJson_WhenPassedArbitraryObject_DeserializesItProperly()
        {
            string json = MakeTestJson();
            var refObj = MakeTestObject();

            var obj = json.DeserializeFromJson<TestObjectWithSimpleTypes>();

            obj.ShouldBeEquivalentTo(refObj);
        }

        [Test]
        public void DeserializeFromJson_WhenPassedInvalidJson_ShouldNotThrowButReturnNullByDefault()
        {
            var invalidJson = MakeInvalidTestJson();
            var obj = invalidJson.DeserializeFromJson<TestObjectWithSimpleTypes>();

            obj.Should().BeNull();
        }

        [Test]
        public void DeserializeFromJson_WhenPassedInvalidJsonAndSilentSetToTrue_ShouldThrow()
        {
            var json = MakeInvalidTestJson();
            Action act = () => json.DeserializeFromJson<TestObjectWithSimpleTypes>(false);

            act.ShouldThrow<Exception>();
        }

        [Test]
        public void ToJson_WhenCalledOnArbitraryObject_SerializesItProperly()
        {
            var obj = MakeTestObject();
            var refJson = MakeTestJson();

            string json = obj.SerializeToJson();

            json.Should().BeSameAs(refJson);
        }

        private TestObjectWithSimpleTypes MakeTestObject()
        {
            return new TestObjectWithSimpleTypes()
            {
                PropBool = true,
                PropEnum = TestEnum.Value2,
                PropInt = 666,
                PropDbl = 1.234,
                PropString = "someText"
            };
        }

        private string MakeTestJson()
        {
            return "{PropBool: true, PropEnum: 1, PropInt: 666, PropDbl: 1.234, PropString: 'someText'}";
        }

        private string MakeInvalidTestJson()
        {
            return "{PropBool: 'x', PropEnum: 1, PropInt: 666, PropDbl: 1.234, PropString: 'someText'}";
        }


        [DataContract]
        private class TestObjectWithSimpleTypes
        {
            [DataMember]
            public bool PropBool { get; set; }

            [DataMember]
            public TestEnum PropEnum { get; set; }

            [DataMember]
            public int PropInt { get; set; }

            [DataMember]
            public double PropDbl { get; set; }

            [DataMember]
            public string PropString { get; set; }

        }


        public enum TestEnum
        {
            Value1,
            Value2
        }
    }
}
