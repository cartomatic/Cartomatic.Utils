using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Cartomatic.Utils.Serialization.Tests
{
    [DataContract]
    public class TestObjectWithSimpleTypes
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
