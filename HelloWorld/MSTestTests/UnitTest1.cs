using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTestTests
{
    [TestClass]
    public class UnitTest1
    {
        public TestContext TestContext
        {
            get;
            set;
        }

        [TestMethod]
        public void CheckCurrentYear()
        {
            Assert.IsTrue(HelloWorld.Common.YearUtils.IsCurrentYear(new DateTime(2017, 1, 1)));
        }

        [TestMethod]
        public void CheckLastYear()
        {
            Assert.IsFalse(HelloWorld.Common.YearUtils.IsCurrentYear(new DateTime(2015, 1, 1)));
        }

        [TestMethod]
        public void TestThatFails()
        {
            this.Foo();
        }

        private void Foo()
        {
            this.Bar();
        }

        private void Bar()
        {
            throw new NotImplementedException();
        }


        [TestMethod]
        public void TestMethodTextContextWriteLine()
        {
            this.TestContext.WriteLine("Hello There");
        }
    }
}
        