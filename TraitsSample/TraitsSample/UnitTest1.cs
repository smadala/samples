using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TraitsSample
{
    [TestClass]
    public class UnitTest1
    {
        [Owner("sbaid")]
        [TestCategory("bvt")]
        [TestProperty("k1", "v1")]
        [TestProperty("k1", "v2")]
        [Priority(1)]
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestProperty("k1", "v1")]
        [Priority(1)]
        [TestMethod]
        public void TestMethod2()
        {
        }

        [TestCategory("bvt")]
        [Priority(2)]
        [TestMethod]
        public void TestMethod3()
        {
        }

        [Owner("navb")]
        [Priority(2)]
        [TestMethod]
        public void TestMethod4()
        {
        }
    }

    [TestCategory("Sample")]
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod21()
        {
        }
    }
}
