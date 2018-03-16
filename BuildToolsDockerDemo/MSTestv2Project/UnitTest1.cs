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
            System.Threading.Thread.Sleep(60 * 1000);
        }

        [Priority(2)]
        [TestMethod]
        public void TestMethod2()
        {
            System.Threading.Thread.Sleep(60 * 1000);
        }

        [Priority(3)]
        [TestMethod]
        public void TestMethod3()
        {
            System.Threading.Thread.Sleep(60 * 1000);
        }
    }
}
