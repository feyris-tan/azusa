using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace moe.yo3explorer.azusa.Control.Licensing
{
    public class GuidUtils
    {
        private GuidUtils() {}

        private static Random rng = new Random();

        private static ulong GetV1Time()
        {
            DateTime gregBegin = new DateTime(1582, 10, 15);
            TimeSpan ts = DateTime.Now - gregBegin;
            ulong result = (ulong)ts.TotalMilliseconds;
            result *= 10000;
            return result;
        }

        private static byte[] GetNodeId()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up)
                    continue;

                return ni.GetPhysicalAddress().GetAddressBytes();
            }
            return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
        }

        private static byte GetAddressFamily()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up)
                    continue;

                IPInterfaceProperties ipInterfaceProperties = ni.GetIPProperties();
                if (ipInterfaceProperties.UnicastAddresses.Count > 0)
                {
                    return 2;
                }
            }

            return 0;
        }

        public static Guid MakeVersion1()
        {
            byte VERSION = 1;
            byte VARIANT = 1;
            long random = (long)((long)rng.Next() * (long)rng.Next());

            MemoryStream ms = new MemoryStream(new byte[16]);
            BinaryWriter bb = new BinaryWriter(ms);
            ulong nanoTime = GetV1Time();
            int time_low = (int)((nanoTime & 0xFFFFFFFF00000000L) >> 32);
            short time_mid = (short)((nanoTime & 0x00000000FFFF0000L) >> 16);
            short time_hi_and_version = (short)((ulong)((VERSION & 0x0F) << 12) | ((nanoTime & 0x000000000000FFF0L) >> 4));
            short clock_seq_hi_and_res = (short)(((VARIANT & 0x0F) << 13) | (random & 0x1FFF));
            byte[] node = GetNodeId();

            bb.Write(time_low);
            bb.Write(time_mid);
            bb.Write(time_hi_and_version);
            bb.Write(clock_seq_hi_and_res);
            bb.Write(node);

            return new Guid(ms.GetBuffer());

        }

        public static Guid MakeVersion2(short userId, short groupId)
    {
        byte VERSION = 2;
        byte VARIANT = 1;
        long random = (long)((long)rng.Next() * (long)rng.Next());;
        ulong nanoTimeE = GetV1Time();
        nanoTimeE &= 0xFFFFFFF000000000L;
        nanoTimeE |= ((ulong)userId << 24);
        nanoTimeE |= ((ulong)groupId << 4);

        int time_low = (int)((nanoTimeE & 0xFFFFFFFF00000000L) >> 32);
        short time_mid = (short)((nanoTimeE & 0x00000000FFFF0000L) >> 16);
        short time_hi_and_version = (short)((ulong)((VERSION & 0x0F) << 12) | ((nanoTimeE & 0x000000000000FFF0L) >> 4));
        short clock_seq_hi_and_res = (short)(((VARIANT & 0x0F) << 13) | (random & 0x1FFF));
        byte[] node = GetNodeId();

        MemoryStream ms = new MemoryStream(new byte[16]);
        BinaryWriter bb = new BinaryWriter(ms);
        bb.Write(time_low);
        bb.Write(time_mid);
        bb.Write(time_hi_and_version);
        bb.Write(clock_seq_hi_and_res);
        bb.Write(node);

        return new Guid(ms.GetBuffer());
    }

        public static Guid MakeApollo()
    {
        ulong nanoTime = GetV1Time();
        int timeHigh32 = (int)((nanoTime & 0xFFFFFFFF00000000L) >> 32);
        short timeLow16 = (short)((nanoTime & 0x00000000FFFF0000L) >> 16);
        short reserved = 0;
        byte family = GetAddressFamily();
        byte[] hostId = GetNodeId();

        MemoryStream ms = new MemoryStream(new byte[16]);
        BinaryWriter bb = new BinaryWriter(ms);
        bb.Write(timeHigh32);
        bb.Write(timeLow16);
        bb.Write(reserved);
        bb.Write(family);
        bb.Write(hostId);

        return new Guid(ms.GetBuffer());
    }

        public static Guid MakeVersion3FromHash(byte[] hashed)
    {
        byte VERSION = 3;

        if (hashed.Length != 16)
            throw new ArgumentOutOfRangeException(String.Format("Expected 16, got {0}", hashed.Length));

        byte[] buffer = new byte[16];
        buffer[0] = hashed[0];
        buffer[1] = hashed[1];
        buffer[2] = hashed[2];
        buffer[3] = hashed[3];
        buffer[4] = hashed[4];
        buffer[5] = hashed[5];
        buffer[6] = hashed[6];
        buffer[7] = hashed[7];

        buffer[7] &= 0x0F;
        buffer[7] |= (byte)(VERSION << 4);

        buffer[8] = hashed[8];

        buffer[8] &= 0xBF;
        buffer[8] |= 0x80;

        buffer[9] = hashed[9];

        buffer[10] = hashed[10];
        buffer[11] = hashed[11];
        buffer[12] = hashed[12];
        buffer[13] = hashed[13];
        buffer[14] = hashed[14];
        buffer[15] = hashed[15];

        return new Guid(buffer);
    }

        public static Guid MakeVersion3(Guid nsGuid, string id)
        {
            byte VERSION = 3;
            MD5 md5 = MD5.Create();

            String message = nsGuid.ToString() + id;
            byte[] hashed = md5.ComputeHash(Encoding.UTF8.GetBytes(message));
            byte[] buffer = new byte[16];
            buffer[0] = hashed[0];
            buffer[1] = hashed[1];
            buffer[2] = hashed[2];
            buffer[3] = hashed[3];
            buffer[4] = hashed[4];
            buffer[5] = hashed[5];
            buffer[6] = hashed[6];
            buffer[7] = hashed[7];

            buffer[7] &= 0x0F;
            buffer[7] |= (byte)(VERSION << 4);

            buffer[8] = hashed[8];

            buffer[8] &= 0xBF;
            buffer[8] |= 0x80;

            buffer[9] = hashed[9];

            buffer[10] = hashed[10];
            buffer[11] = hashed[11];
            buffer[12] = hashed[12];
            buffer[13] = hashed[13];
            buffer[14] = hashed[14];
            buffer[15] = hashed[15];

            return new Guid(buffer);
        }

        public static Guid MakeVersion4()
    {
        byte VERSION = 4;

        byte[] buffer = new byte[16];
        rng.NextBytes(buffer);

        buffer[8] &= 0xBF;
        buffer[8] |= 0x80;

        buffer[7] &= 0x0F;
        buffer[7] |= (byte)(VERSION << 4);

        return new Guid(buffer);
    }

        public static Guid MakeVersion5(Guid nsGuid,String id)
    {
        byte VERSION = 5;
        SHA1 md5 = SHA1.Create();

        String message = nsGuid.ToString() + id;
        byte[] hashed = md5.ComputeHash(Encoding.UTF8.GetBytes(message));
        byte[] buffer = new byte[16];
        buffer[0] = hashed[0];
        buffer[1] = hashed[1];
        buffer[2] = hashed[2];
        buffer[3] = hashed[3];
        buffer[4] = hashed[4];
        buffer[5] = hashed[5];
        buffer[6] = hashed[6];
        buffer[7] = hashed[7];

        buffer[7] &= 0x0F;
        buffer[7] |= (byte)(VERSION << 4);

        buffer[8] = hashed[8];

        buffer[8] &= 0xBF;
        buffer[8] |= 0x80;

        buffer[9] = hashed[9];

        buffer[10] = hashed[10];
        buffer[11] = hashed[11];
        buffer[12] = hashed[12];
        buffer[13] = hashed[13];
        buffer[14] = hashed[14];
        buffer[15] = hashed[15];

        return new Guid(buffer);
    }
}
}