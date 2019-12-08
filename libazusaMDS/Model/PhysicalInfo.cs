using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    public class PhysicalInfo
    {
        internal PhysicalInfo(byte[] buffer, int offset)
        {
            byte tempByte = buffer[offset++];
            partVer = (byte)((tempByte & 0xF0) >> 4);
            bookType = (byte)(tempByte & 0x0F);

            tempByte = buffer[offset++];
            maxRate = (byte)((tempByte & 0xF0) >> 4);
            discSize = (byte)(tempByte & 0x0F);

            tempByte = buffer[offset++];
            layerType = (byte)((tempByte & 0xF0) >> 4);
            trackPath = (byte)((tempByte & 0x08) >> 3);
            numLayers = (byte)((tempByte & 0x06) >> 1);
            dummy1 = (byte)((tempByte & 0x01));

            tempByte = buffer[offset++];
            trackDensity = (byte)((tempByte & 0xF0) >> 4);
            linearDensity = (byte)(tempByte & 0x0F);

            dataStart += (uint)((buffer[offset++] << 16));
            dataStart += (uint)((buffer[offset++] << 8));
            dataStart += (uint)((buffer[offset++]));
            dummy2 = buffer[offset++];

            dataEnd += (uint)((buffer[offset++] << 16));
            dataEnd += (uint)((buffer[offset++] << 8));
            dataEnd += (uint)((buffer[offset++]));
            dummy3 = buffer[offset++];

            layer0End += (uint)((buffer[offset++] << 16));
            layer0End += (uint)((buffer[offset++] << 8));
            layer0End += (uint)((buffer[offset++]));
            dummy4 = buffer[offset++];

            tempByte = buffer[offset++];
            dummy5 = (byte)((tempByte & 0xFE) >> 1);
            bca = (byte)(tempByte & 0x01);

            mediaSpecific = new byte[2031];
            Array.Copy(buffer, offset, mediaSpecific, 0, 2031);
            offset += 2031;
        }

        byte partVer, bookType;
        byte maxRate, discSize;
        byte layerType, trackPath, numLayers, dummy1;
        byte trackDensity, linearDensity;
        uint dataStart, dummy2;
        uint dataEnd, dummy3;
        uint layer0End, dummy4;
        byte dummy5, bca;
        byte[] mediaSpecific;

        public int NumberOfLayers { get { return numLayers; } }

        public byte BookType { get { return bookType; } }
    }
}
