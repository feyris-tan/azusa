using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myfigurecollectionDumper
{
    internal static class DateTimeExtensions
    {
        public static long ToUnixTime(this DateTime value)
        {
            return (long)(value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }
    }
}
