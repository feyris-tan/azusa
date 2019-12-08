using System.IO;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class UnimplemetedRecord : BaseDatabaseRecord
    {
        public UnimplemetedRecord(BinaryReader br, int size)
        {
            Data = br.ReadBytes(size);
        }
        
        public byte[] Data { private set; get; }
    }
}