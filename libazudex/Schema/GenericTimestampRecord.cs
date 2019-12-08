using System;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class GenericTimestampRecord : BaseDatabaseRecord
    {
        protected uint systemTime, displayTime;

        protected static DateTime BASE_TIME = new DateTime(2009, 1, 1);
        
        public DateTime SystemTime
        {
            get { return (BASE_TIME + new TimeSpan(TimeSpan.TicksPerSecond * systemTime)); }
        }

        public DateTime DisplayTime
        {
            get { return (BASE_TIME + new TimeSpan(TimeSpan.TicksPerSecond * displayTime)); }
        }
    }
}