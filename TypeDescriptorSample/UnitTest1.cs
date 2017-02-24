using System;
using System.ComponentModel;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var td = TypeDescriptor.GetConverter(typeof(string[]));
            var result = td.CanConvertFrom(typeof(string));
            Assert.False(result);
        }
    }
}