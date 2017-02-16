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
