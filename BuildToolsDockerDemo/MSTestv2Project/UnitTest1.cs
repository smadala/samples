using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTestv2Project
{
    [TestClass]
    public class UnitTest1
    {
        [Priority(1)]
        [TestMethod]
        public void TestMethod1()
        {
        }

        [Priority(2)]
        [TestMethod]
        public void TestMethod2()
        {
        }

        [Priority(3)]
        [TestMethod]
        public void TestMethod3()
        {
        }
    }
}
