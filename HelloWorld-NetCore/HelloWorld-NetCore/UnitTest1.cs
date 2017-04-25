using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloWorld_NetCore
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            new ClassLibrary1.Class1().Foo();
            Console.WriteLine("hi there");
        }

        [TestMethod]
        public void TestMethod2()
        {
            Console.WriteLine("hi there - TestMethod2");
        }
    }
}
