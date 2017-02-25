using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        public int foo(int input)
        {
            if (input < 10)
                return input * 2;
            else
                return input * 3;
        }

        public int bar(int input)
        {
            if (input < 10)
                return input * 3;
            else
                return input * 4;

        }
    }
}
