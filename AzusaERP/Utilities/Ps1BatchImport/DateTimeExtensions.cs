using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.Utilities.Ps1BatchImport
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTime(this DateTime source)
        {
            return (long)(source.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime FromUnixTime(long utime) => new DateTime(1970, 1, 1).AddSeconds(utime);

        public static long ToUnixMinute(this DateTime source)
        {
            return source.ToUnixTime() / 60;
        }
    }
}
