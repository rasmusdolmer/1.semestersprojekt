using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.semesterprojekt
{
    class DateTimeConverter
    {
        public static DateTime DateTimeOffsetAndTimeSetToDateTime(DateTimeOffset date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
    }
}
