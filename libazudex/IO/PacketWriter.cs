using System;

namespace moe.yo3explorer.azusa.dex.IO
{
    internal class PacketWriter
    {
        private const int MAX_PAYLOAD = 1584;
        private const int MIN_LEN = 6;
        public const int MAX_LEN = 1590;
        private const byte SOF = 0x01;
        private const int OFFSET_SOF = 0;
        private const int OFFSET_LENGTH = 1;
        public const int OFFSET_CMD = 3;
        private const int OFFSET_PAYLOAD = 4;

        private byte[] packet;
        private short ptr;

        public PacketWriter()
        {
            ptr = 0;
            packet = new byte[MAX_LEN];
        }

        public byte[] PacketString()
        {
            byte[] output = new byte[ptr];
            Array.Copy(packet, 0, output, 0, ptr);
            return output;
        }

        private void AppendCrc()
        {
            SetLength();
            byte[] packetString = PacketString();
            ushort crc = CRC16.CalculateCRC16(packetString, 0, packetString.Length);
            byte[] packedCrc = BitConverter.GetBytes(crc);
            packet[ptr++] = packedCrc[0];
            packet[ptr++] = packedCrc[1];
        }

        private void SetLength()
        {
            byte[] length = BitConverter.GetBytes(ptr + 2);
            packet[OFFSET_LENGTH + 0] = length[0];
            packet[OFFSET_LENGTH + 1] = length[1];
            if (ptr == 1)
                ptr += 2;
        }

        public void ComposePacket(DexcomCommands command, byte[] payload = null)
        {
            if (ptr != 0) throw new Exception();
            packet[ptr++] = 0x01;
            packet[ptr++] = 0;
            packet[ptr++] = 0;
            packet[ptr++] = (byte)command;
            if (payload != null){
                Array.Copy(payload, 0, packet, ptr, payload.Length);
                ptr += (short)payload.Length;
            }
            AppendCrc();
        }
    }
}