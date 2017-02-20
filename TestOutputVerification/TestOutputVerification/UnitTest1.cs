using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestOutputVerification
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine("Inside TestMethod1");
            Assert.AreEqual(0, 0);
            Console.WriteLine("Exiting TestMethod1");
        }

        [TestMethod]
        public void TestMethod2()
        {
            System.Diagnostics.Debug.WriteLine("Inside TestMethod2");
            Assert.AreEqual(0, 1);
            System.Diagnostics.Debug.WriteLine("Exiting TestMethod2");
        }
    }
}
