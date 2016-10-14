using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        public static string SayHiInEnglish(Interface1 call)
        {
            return call.SayHi("en");
        }

        public static bool IsCurrentYearLeapYear()
        {
            return DateTime.Now.Year % 4 == 0;
        }
    }
}
