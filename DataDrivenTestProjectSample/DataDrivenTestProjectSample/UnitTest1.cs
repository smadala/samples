using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDrivenTestProjectSample
{
    [TestClass]
    public class MyTestClass
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        [DeploymentItem("DataDrivenTestData.xml")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                   "DataDrivenTestData.xml",
                   "Row",
                    DataAccessMethod.Sequential)]
        public void TestMethod1()
        {
            int op1 = Int32.Parse((string)TestContext.DataRow["op1"]);
            int op2 = Int32.Parse((string)TestContext.DataRow["op2"]);
            int res = Int32.Parse((string)TestContext.DataRow["res"]);
            Console.WriteLine("Sum tests ");
            Assert.AreEqual(op1 + op2, res);
        }
    }
}
