using System.IO;

namespace moe.yo3explorer.azusa.dex.Schema.Unimplemented
{
    public class ReceiverLogData : UnimplemetedRecord
    {
        public ReceiverLogData(BinaryReader br)
            : base(br, 20)
        {
            
        }
    }
}