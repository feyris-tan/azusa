using System;

namespace moe.yo3explorer.azusa.dex.IO
{
    internal class PacketReader
    {
        public PacketReader(byte[] packet)
        {
            length = BitConverter.ToUInt16(packet, 1);
            command = (DexcomCommands) packet[3];
            crc = BitConverter.ToUInt16(packet, length - 2);
            payloadLength = (ushort)(length - 6);
            if (payloadLength > 0)
            {
                payload = new byte[payloadLength];
                Array.Copy(packet, 4, payload, 0, payloadLength);
            }
        }

        private ushort length;
        private DexcomCommands command;
        private ushort crc;
        private ushort payloadLength;
        private byte[] payload;

        public ushort PacketLength
        {
            get { return length; }
        }

        public DexcomCommands Command
        {
            get { return command; }
        }
        
        public ushort CRC
        {
            get { return crc; }
        }

        public byte[] Payload
        {
            get { return payload; }
        }
    }
}