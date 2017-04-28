using System;
using System.Diagnostics;
using System.Threading;
using Xunit;

namespace NetCoreCrashTest
{
    public class UnitTest1
    {
        [Fact]
        public void Fails()
        {
            Action fail = () => Debug.Assert(false, "Oops");
            var thread = new Thread(new ThreadStart(fail));
            thread.Start();
            thread.Join();
        }
    }
}
