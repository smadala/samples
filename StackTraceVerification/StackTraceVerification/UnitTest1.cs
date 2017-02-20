using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StackTraceVerification
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ClassLibrary1.Class1.foo();
        }
    }
}
