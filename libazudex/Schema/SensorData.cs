using System.IO;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class SensorData : GenericTimestampRecord
    {
        public SensorData(BinaryReader br)
        {
            systemTime = br.ReadUInt32();
            displayTime = br.ReadUInt32();
            unfiltered = br.ReadUInt32();
            filtered = br.ReadUInt32();
            rssi = br.ReadUInt16();
            crc = br.ReadUInt16();

        }

        private uint unfiltered;
        private uint filtered;
        private ushort rssi;

        public uint Unfiltered
        {
            get { return unfiltered; }
        }

        public uint Filtered
        {
            get { return filtered; }
        }

        public ushort Rssi
        {
            get { return rssi; }
        }
    }
}