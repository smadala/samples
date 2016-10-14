using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var msg = "hi there";
            var interface1 = new ClassLibrary1.Fakes.StubInterface1()
            {
                SayHiString = (lang) =>
                {
                    return msg;
                }
            };
            Assert.AreEqual(msg, ClassLibrary1.Class1.SayHiInEnglish(interface1));
        }

        [TestMethod]
        public void TestMethod2()
        {
            using (ShimsContext.Create())
            {
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2000, 1, 1);
                Assert.IsTrue(ClassLibrary1.Class1.IsCurrentYearLeapYear());
            }
        }
    }
}
