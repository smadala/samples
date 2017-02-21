using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        public static void foo()
        {
            bar();
        }

        public static void bar()
        {
            throw new Exception("Something's wrong");
        }
    }
}
