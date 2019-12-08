using System.IO;
using moe.yo3explorer.azusa.dex.Schema.Enums;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class EGVRecord : GenericTimestampRecord
    {
        public EGVRecord(BinaryReader br)
        {
            systemTime = br.ReadUInt32();
            displayTime = br.ReadUInt32();
            _glucose = br.ReadUInt16();
            _trendArrow = br.ReadByte();
            crc = br.ReadUInt16();
        }

        private ushort _glucose;
        private byte _trendArrow;

        public ushort Glucose
        {
            get { return (ushort)(_glucose & 1023);  }
        }

        public TrendArrow TrendArrow
        {
            get { return (TrendArrow) (_trendArrow & 15); }
        }

        public SpecialGlucoseValue? SpecialGlucoseValue
        {
            get
            {
                if (Glucose > 12)
                    return null;

                return (SpecialGlucoseValue) Glucose;
            }
        }
    }
}