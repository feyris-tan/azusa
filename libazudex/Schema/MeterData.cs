using System;
using System.IO;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class MeterData : GenericTimestampRecord
    {
        public MeterData(BinaryReader br)
        {
            systemTime = br.ReadUInt32();
            displayTime = br.ReadUInt32();
            _meterGlucose = br.ReadUInt16();
            _meterTime = br.ReadUInt32();
            crc = br.ReadUInt16();
        }

        private ushort _meterGlucose;
        private uint _meterTime;
        
        public DateTime MeterTime
        {
            get { return (BASE_TIME + new TimeSpan(TimeSpan.TicksPerSecond * _meterTime)); }
        }

        public ushort MeterGlucose
        {
            get { return _meterGlucose; }
        }
    }
}