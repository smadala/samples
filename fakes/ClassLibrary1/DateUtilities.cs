using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class DateUtilities
    {
        public static bool IsCurrentYearALeapYear()
        {
            return DateTime.Now.Year % 4 == 0;
        }
    }
}
