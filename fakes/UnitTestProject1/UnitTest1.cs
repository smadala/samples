using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary1;

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
        
    }
}
