using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Xunit21Sample
{
    public class Xunit21Tests
    {
        [Fact]
        public void TestMethod1()
        {
            // throw new ArgumentException();
        }

        [Fact]
        public async Task TestMethod2()
        {
            await Task.Delay(1);
            // throw new ArgumentException();
        }
    }
}
