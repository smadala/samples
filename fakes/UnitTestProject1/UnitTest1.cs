using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary1;
using Microsoft.QualityTools.Testing.Fakes;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IStockFeed feed = new ClassLibrary1.Fakes.StubIStockFeed()
            {
                GetStockPriceString = (code) => { return 100; }
            };
            var price = new StockAnalyzer().GetMsftStockPrice(feed);
            Assert.AreEqual(100, price, "price is expected 100");
        }
        
        [TestMethod]
        public void TestMethod2()
        {
            var fixedYear = 2000;
            
            using (ShimsContext.Create())
            {
                System.Fakes.ShimDateTime.NowGet = () =>
                {
                    return new DateTime(fixedYear, 1, 1);
                };
                Assert.IsTrue(ClassLibrary1.DateUtilities.IsCurrentYearALeapYear());
            }
        }
    }
}
