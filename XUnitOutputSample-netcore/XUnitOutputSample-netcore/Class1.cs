using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XunitOutputSample
{
    public class Class1
    {
        [Fact]
        public void TestMethod1()
        {
            System.Threading.Thread.Sleep(10 * 1000);
        }

        [Fact]
        public void TestMethod2()
        {
            System.Threading.Thread.Sleep(5 * 1000);
        }

        [Fact]
        public void TestMethod3()
        {
            System.Threading.Thread.Sleep(5 * 1000);
        }

        [Fact]
        public void FailingTest()
        {
            this.foo();
        }

        private void foo()
        {
            this.bar();
        }

        private void bar()
        {
            throw new NotImplementedException();
        }
    }
}
