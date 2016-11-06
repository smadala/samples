using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        public static bool IsLeapYear(int year)
        {
            if (year % 4 == 0)
                return true;
            else
                return false;
        }
    }
}
