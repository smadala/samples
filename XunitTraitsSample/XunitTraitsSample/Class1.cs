using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XunitTraitsSample
{
    public class TestClass1
    {
        [Fact]
        public void foo()
        {

        }

        [Fact]
        public void bar()
        {

        }
    }

    public class TestClass2
    {
        [Trait("Category", "bvt")]
        [Trait("Priority", "1")]
        [Fact]
        public void foo()
        {

        }
       

        [Fact]
        public void bar()
        {

        }
    }

    public class TestClass3
    {
        [Trait("Category", "bvt")]
        [Trait("Priority", "1")]
        [Fact]
        public void foo()
        {

        }

        [Trait("Priority", "2")]
        [Fact]
        public void bar()
        {
        }
    }
}
