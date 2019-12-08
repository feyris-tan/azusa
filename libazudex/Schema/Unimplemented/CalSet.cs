using System.IO;

namespace moe.yo3explorer.azusa.dex.Schema.Unimplemented
{
    public class CalSet : UnimplemetedRecord
    {
        public CalSet(BinaryReader br)
            : base(br,148)
        {
            //a cal set is 148 bytes long
        }
    }
}