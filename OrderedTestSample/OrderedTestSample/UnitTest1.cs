using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrderedTestSample
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine("I am test1");
        }

        [TestMethod]
        public void TestMethod2()
        {
            Console.WriteLine("I am test2");
        }

        [TestMethod]
        public void TestMethod3()
        {
            Console.WriteLine("I am test3");
        }
    }
}
