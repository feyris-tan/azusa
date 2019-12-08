using System;

namespace moe.yo3explorer.azusa
{
    static class UnixTimeConverter
    {
        public static int ToUnixTime(this DateTime source)
        {
            return (int)(source.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime FromUnixTime(long utime) => new DateTime(1970, 1, 1).AddSeconds(utime);
    }
}
