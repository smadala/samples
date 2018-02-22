using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Remoting.Messaging;

namespace CallContextData
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Foo.A(Foo.B());
        }
    }

     // [Serializable]
    public class Foo
    {
        private static readonly string ValueKey = typeof(Foo).FullName;

        public static Foo B()
        {
            return CallContext.LogicalGetData(ValueKey) as Foo ?? new Foo();
        }

        public static void A(Foo value)
        {
            CallContext.LogicalSetData(ValueKey, value);
        }
    }
}
