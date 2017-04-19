using System;
using System.Threading.Tasks;
using Xunit;

namespace XUnitNetCoreSample
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Task.Delay(10 * 1000).Wait();
        }
    }
}
