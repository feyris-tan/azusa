using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzusaERP.OldStuff
{
    class HashLib
    {
        public static HashFactory HashFactory { get; set; }
    }

    internal class HashFactory
    {
        public Hash64 Hash64 { get; set; }
        public Checksum Checksum { get; set; }
    }

    internal class Checksum
    {
        public IHash CreateCRC64a()
        {
            throw new NotImplementedException();
        }
    }

    internal class Hash64
    {
        public IHash CreateMurmur2()
        {
            throw new NotImplementedException();
        }
    }

    internal interface IHash
    {
        IHash ComputeString(string name);
        long GetULong();
        IHash ComputeBytes(byte[] data);
    }
}
