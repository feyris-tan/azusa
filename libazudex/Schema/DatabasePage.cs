using System;
using System.IO;
using moe.yo3explorer.azusa.dex.Schema.Enums;
using moe.yo3explorer.azusa.dex.Schema.Unimplemented;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class DatabasePage
    {
        public DatabasePage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader br = new BinaryReader(ms);
            _firstIndex = br.ReadUInt32();
            _numRec = br.ReadUInt32();
            _recordType = (RecordType)br.ReadByte();
            _revision = br.ReadByte();
            _pageNo = br.ReadUInt32();
            _r1 = br.ReadUInt32();
            _r2 = br.ReadUInt32();
            _r3 = br.ReadUInt32();
            _crc = br.ReadUInt16();

            Type recordType = null;
            
            /*
             *  
             */
            switch (_recordType)
            {
                case RecordType.SensorData:
                    recordType = typeof(SensorData);
                    break;
                case RecordType.ManufacturingData:
                    recordType = typeof(ManufacturingParameters);
                    break;
                case RecordType.FirmwareParameterData:
                    recordType = typeof(FirmwareParameterData);
                    break;
                case RecordType.PcSoftwareParameter:
                    recordType = typeof(PcSoftwareParameter);
                    break;
                case RecordType.EgvData:
                    recordType = typeof(EGVRecord);
                    break;
                case RecordType.CalSet:
                    recordType = typeof(CalSet);
                    break;
                case RecordType.InsertionTime:
                    recordType = typeof(InsertionTime);
                    break;
                case RecordType.ReceiverLogData:
                    recordType = typeof(ReceiverLogData);
                    break;
                case RecordType.MeterData:
                    recordType = typeof(MeterData);
                    break;
                case RecordType.UserEventData:
                    recordType = typeof(UserEventData);
                    break;
                case RecordType.UserSettingData:
                    recordType = typeof(UserSettingData);
                    break;
                case RecordType.ReceiverErrorData:
                    ms.Dispose();
                    _record = new BaseDatabaseRecord[0];
                    return;
                case RecordType.Deviation:
                    ms.Dispose();
                    _record = new BaseDatabaseRecord[0];
                    return;
                default:
                    File.WriteAllBytes(String.Format("{0}.dmp", _recordType), buffer);
                    throw new NotImplementedException(_recordType.ToString());
            }

            _record = new BaseDatabaseRecord[_numRec];
            for (uint i = 0; i < _numRec; i++)
            {
                _record[i] = (BaseDatabaseRecord)Activator.CreateInstance(recordType, br);
            }
            ms.Dispose();
        }

        private uint _firstIndex;
        private uint _numRec;
        private RecordType _recordType;
        private byte _revision;
        private uint _pageNo;
        private uint _r1, _r2, _r3;
        private ushort _crc;
        private BaseDatabaseRecord[] _record;

        public RecordType RecordType
        {
            get { return _recordType; }
        }

        public BaseDatabaseRecord[] Record
        {
            get { return _record; }
        }
    }
}