using System;
using System.IO;
using System.Runtime.Serialization;
using moe.yo3explorer.azusa.dex.Schema.Enums;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class UserEventData : GenericTimestampRecord
    {
        public UserEventData(BinaryReader br)
        {
            systemTime = br.ReadUInt32();
            displayTime = br.ReadUInt32();
            _eventType = (EventType) br.ReadByte();
            _eventSubType = br.ReadByte();
            _displayTime = br.ReadUInt32();
            _eventValue = br.ReadUInt32();
            crc = br.ReadUInt16();
        }

        private EventType _eventType;
        private byte _eventSubType;
        private uint _displayTime;
        private uint _eventValue;

        public EventType EventType
        {
            get { return _eventType; }
        }

        public Nullable<HealthSubType> HealthEvent
        {
            get
            {
                if (_eventType != EventType.Health)
                    return null;

                return (HealthSubType) _eventSubType;
            }
        }

        public Nullable<ExerciseSubType> ExerciseEvent
        {
            get
            {
                if (_eventType != EventType.Exercise)
                    return null;

                return (ExerciseSubType) _eventSubType;
            }
        }
        
        public DateTime DisplayTime2
        {
            get { return (BASE_TIME + new TimeSpan(TimeSpan.TicksPerSecond * _displayTime)); }
        }

        public double EventValue
        {
            get
            {
                if (_eventType == EventType.Insulin)
                    return _eventValue / 100.0;
                return _eventValue;
            }
        }
    }
}