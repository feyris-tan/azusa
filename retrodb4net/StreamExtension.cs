using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace retrodb4net
{
    static class BinaryReaderExtension
    {
        private const byte MPF_FIXMAP = 0x80;
        private const byte MPF_MAP16 = 0xde;
        private const byte MPF_MAP32 = 0xdf;
        private const byte MPF_FIXARRAY = 0x90;
        private const byte MPF_ARRAY16 = 0xdc;
        private const byte MPF_ARRAY32 = 0xdd;
        private const byte MPF_FIXSTR = 0xa0;
        private const byte MPF_STR8 = 0xd9;
        private const byte MPF_STR16 = 0xda;
        private const byte MPF_STR32 = 0xdb;
        private const byte MPF_BIN8 = 0xc4;
        private const byte MPF_BIN16 = 0xc5;
        private const byte MPF_BIN32 = 0xc6;
        private const byte MPF_FALSE = 0xc2;
        private const byte MPF_TRUE = 0xc3;
        private const byte MPF_INT8 = 0xd0;
        private const byte MPF_INT16 = 0xd1;
        private const byte MPF_INT32 = 0xd2;
        private const byte MPF_INT64 = 0xd3;
        private const byte MPF_UINT8 = 0xcc;
        private const byte MPF_UINT16 = 0xcd;
        private const byte MPF_UINT32 = 0xce;
        private const byte MPF_UINT64 = 0xcf;
        private const byte MPF_NIL = 0xc0;

        public static object ReadRMSGPack(this BinaryReader br)
        {
            byte[] type = new byte[1];
            if (br.BaseStream.Read(type, 0, 1) == 0)
                return new EndOfStreamException();

            if (type[0] < MPF_FIXMAP)
            {
                throw new NotImplementedException(nameof(MPF_FIXMAP));
            }
            else if (type[0] < MPF_FIXARRAY)
            {
                return ReadMap(br, type[0] - MPF_FIXMAP);
            }
            else if (type[0] < MPF_FIXSTR)
            {
                throw new NotImplementedException(nameof(MPF_FIXSTR));
            }
            else if (type[0] < MPF_NIL)
            {
                uint tmp_len = (uint)(type[0] - MPF_FIXSTR);
                byte[] strBuffer = new byte[tmp_len];
                if (br.BaseStream.Read(strBuffer, 0, (int)tmp_len) != tmp_len)
                    throw new EndOfStreamException();
                return Encoding.ASCII.GetString(strBuffer);
            }
            else if (type[0] > MPF_MAP32)
            {
                throw new NotImplementedException(nameof(MPF_MAP32));
            }

            switch (type[0])
            {
                case MPF_NIL:
                    return null;
                case MPF_FALSE:
                    throw new NotImplementedException(nameof(MPF_FALSE));
                case MPF_TRUE:
                    throw new NotImplementedException(nameof(MPF_TRUE));
                case MPF_BIN8:
                case MPF_BIN16:
                case MPF_BIN32:
                    int binSizeLength = (1 << (type[0] - MPF_BIN8));
                    byte[] binSizeBuffer = new byte[4];
                    br.BaseStream.Read(binSizeBuffer, 0, binSizeLength);
                    if (binSizeLength == 1)
                        binSizeLength = binSizeBuffer[0];
                    else
                        throw new NotImplementedException(binSizeLength.ToString());
                    byte[] bin = new byte[binSizeLength];
                    br.BaseStream.Read(bin, 0, binSizeLength);
                    return bin;
                case MPF_UINT8:
                case MPF_UINT16:
                case MPF_UINT32:
                case MPF_UINT64:
                    ulong uintHelper = 1UL << (type[0] - MPF_UINT8);
                    byte[] uintBuffer = new byte[8];
                    br.BaseStream.Read(uintBuffer, 0, (int)uintHelper);
                    if (uintHelper == 1)
                        return uintBuffer[0];
                    else if (uintHelper == 2)
                    {
                        uintBuffer[2] = uintBuffer[1];
                        uintBuffer[3] = uintBuffer[0];
                        return BitConverter.ToUInt16(uintBuffer, 2);
                    }
                    else if (uintHelper == 4)
                    {
                        uintBuffer[4] = uintBuffer[3];
                        uintBuffer[5] = uintBuffer[2];
                        uintBuffer[6] = uintBuffer[1];
                        uintBuffer[7] = uintBuffer[0];
                        return BitConverter.ToUInt32(uintBuffer, 4);
                    }
                    else if (uintHelper == 8)
                    {
                        uintBuffer = uintBuffer.Reverse().ToArray();
                        return BitConverter.ToUInt64(uintBuffer, 0);
                    }
                    else
                        throw new NotImplementedException(uintHelper.ToString());
                case MPF_INT8:
                case MPF_INT16:
                case MPF_INT32:
                case MPF_INT64:
                    throw new NotImplementedException(nameof(MPF_INT8));
                case MPF_STR8:
                case MPF_STR16:
                case MPF_STR32:
                    int strSize = (1 << (type[0] - MPF_STR8));
                    byte[] strHelpBuffer = new byte[4];
                    br.BaseStream.Read(strHelpBuffer, 0,strSize);
                    if (strSize == 1)
                        strSize = strHelpBuffer[0];
                    else if (strSize == 2)
                    {
                        strHelpBuffer[2] = strHelpBuffer[1];
                        strHelpBuffer[3] = strHelpBuffer[0];
                        strSize = BitConverter.ToUInt16(strHelpBuffer, 2);
                    }
                    else
                        throw new NotImplementedException(strSize.ToString());
                    byte[] strBuffer = new byte[strSize];
                    br.BaseStream.Read(strBuffer, 0, strSize);
                    return Encoding.ASCII.GetString(strBuffer);
                case MPF_ARRAY16:
                case MPF_ARRAY32:
                    throw new NotImplementedException(nameof(MPF_ARRAY16));
                case MPF_MAP16:
                case MPF_MAP32:
                    int mapLen = (int)AutoReadUint(br, (uint)(2 << (type[0] - MPF_MAP16)));
                    return ReadMap(br, mapLen);
            }
            
            return 0;
        }

        private static Dictionary<object,object> ReadMap(BinaryReader br,int tmp_len)
        {
            Dictionary<object, object> result = new Dictionary<object, object>();

            for (int i = 0; i < tmp_len; i++)
            {
                object k = ReadRMSGPack(br);
                object v = ReadRMSGPack(br);
                if (!result.ContainsKey(k))
                    result.Add(k, v);
            }

            return result;
        }

        private static uint AutoReadUint(BinaryReader br, uint len)
        {
            byte[] buffer;
            switch (len)
            {
                case 4:
                    buffer = br.ReadBytes(4);
                    if (BitConverter.IsLittleEndian)
                        buffer = buffer.Reverse().ToArray();
                    return BitConverter.ToUInt32(buffer,0);
                default:
                    throw new NotImplementedException(len.ToString());
            }
        }
    }
}
