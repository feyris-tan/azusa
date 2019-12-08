using System;
using System.IO;
using moe.yo3explorer.azusa.dex.Schema.Enums;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class InsertionTime : GenericTimestampRecord
    {
        public InsertionTime(BinaryReader br)
        {
            systemTime = br.ReadUInt32();
            displayTime = br.ReadUInt32();
            _insertionTime = br.ReadUInt32();
            _sessionState = (SessionState) br.ReadByte();
            crc = br.ReadUInt16();
        }

        private uint _insertionTime;
        private SessionState _sessionState;
        
        public new DateTime DisplayTime
        {
            get { return (BASE_TIME + new TimeSpan(TimeSpan.TicksPerSecond * _insertionTime)); }
        }

        public SessionState SessionState
        {
            get { return _sessionState; }
        }
    }
}