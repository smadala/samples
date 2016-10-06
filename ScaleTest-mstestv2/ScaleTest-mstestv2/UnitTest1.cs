using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodWithReferences()
        {
            new ClassLibrary1.Class1().foo();
            new ClassLibrary2.Class1().bar();
            new ClassLibrary3.Class1().onemorebar();
        }
    }
}
