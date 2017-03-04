using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestIndicatorSample
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            new ClassLibrary1.Class1().Foo();
        }
    }
}
