using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject3
{
    [TestClass]
    public class MoreTests
    {
        [TestInitialize]
        public void TestInit()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AnotherInconclusiveTest()
        {
        }
    }
}
