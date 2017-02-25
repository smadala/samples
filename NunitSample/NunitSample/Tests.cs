using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitSample
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestMethod1()
        {
            Assert.AreEqual(new ClassLibrary1.Class1().foo(4), 8);
            Assert.AreEqual(new ClassLibrary1.Class1().foo(11), 33);

        }

        [Test]
        public void TestMethod2()
        {
            Assert.AreEqual(new ClassLibrary1.Class1().bar(3), 9);
            Assert.AreEqual(new ClassLibrary1.Class1().bar(11), 44);
        }

        [TestCase(12, 3, 4)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 4, 3)]

        public void TestMethod2(int a, int b, int c)
        {

        }
    }
}
