using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.Common
{
    public class YearUtils
    {
        public static bool IsCurrentYear(DateTime date)
        {
            if (date.Year == DateTime.Now.Year)
                return true;
            else
                return false;
        }
    }
}
