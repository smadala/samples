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

        }

        [TestCase(12, 3, 4)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 4, 3)]

        public void TestMethod2(int a, int b, int c)
        {

        }
    }
}
