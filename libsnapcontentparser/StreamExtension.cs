using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.snapcontentparser
{
    internal static class StreamExtension
    {
        private static void FlipEndian(byte[] data)
        {
            if (data.Length % 2 == 1) throw new InvalidDataException();
            byte temp;

            for (int i = 0; i < (data.Length / 2); i++)
            {
                temp = data[i];
                data[i] = data[data.Length - i - 1];
                data[data.Length - i - 1] = temp;
            }
        }

        public static int ReadInt32LE(this Stream stream)
        {
            byte[] buffer = new byte[4];
            if (stream.Read(buffer, 0, 4) != 4)
                throw new IOException("failed to read int32.");
            if (BitConverter.IsLittleEndian)
                return BitConverter.ToInt32(buffer, 0);
            else
            {
                FlipEndian(buffer);
                return BitConverter.ToInt32(buffer, 0);
            }
        }

        public static uint sgetb32(this Stream stream)
        {
            byte b, s;
            int c;
            uint v = 0;
            s = 0;

            int phase = 0;
            switch (phase)
            {
                case 0:
                    c = stream.ReadByte();
                    if (c == -1)
                        throw new EndOfStreamException();
                    b = (byte) c;
                    if ((b & 0x80) == 0)
                    {
                        v |= (uint) b << s;
                        s += 7;
                        if (s >= 32)
                            throw new EndOfStreamException();
                        goto case 0;
                    }

                    v |= (uint) (b & 0x7f) << s;
                    return v;
                default:
                    throw new AccessViolationException();
            }
        }

        public static char ReadAsciiChar(this Stream stream)
        {
            int readByte = stream.ReadByte();
            if (readByte == -1)
                throw new EndOfStreamException();
            return (char) readByte;
        }

        public static string ReadBinaryString(this Stream stream,int size = 255)
        {
            uint len = sgetb32(stream);

            if (len + 1 > size)
                throw new SnapraidContentException(SnapraidContentFailureReason.STRING_TOO_LONG);

            byte[] buffer = new byte[len];
            if (stream.Read(buffer,0,(int)len) != len)
                throw new EndOfStreamException();
            return Encoding.ASCII.GetString(buffer);
        }

        public static ulong sgetb64(this Stream stream)
        {
            byte b, s;
            int c;
            ulong v = 0;
            s = 0;

            int phase = 0;
            switch (phase)
            {
                case 0:
                    c = stream.ReadByte();
                    if (c == -1)
                        throw new EndOfStreamException();
                    b = (byte)c;
                    if ((b & 0x80) == 0)
                    {
                        v |= (ulong)b << s;
                        s += 7;
                        if (s >= 64)
                            throw new EndOfStreamException();
                        goto case 0;
                    }

                    v |= (ulong)(b & 0x7f) << s;
                    return v;
                default:
                    throw new AccessViolationException();
            }
        }

        public static byte[] ReadFixedLength(this Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            if (stream.Read(buffer, 0, length) != length)
            {
                throw new EndOfStreamException();
            }

            return buffer;
        }
    }
}
