namespace moe.yo3explorer.azusa.WarWalking.Entity
{
    class ByteBuffer
    {
        public ByteBuffer(int size)
        {
            Data = new byte[size];
        }

        public byte[] Data { get; private set; }

        public long Hash { get { return (long)HashLib.HashFactory.Checksum.CreateCRC64a().ComputeBytes(Data).GetULong();  } }
    }
}
