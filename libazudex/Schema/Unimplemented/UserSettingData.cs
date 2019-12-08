using System.IO;

namespace moe.yo3explorer.azusa.dex.Schema.Unimplemented
{
    public class UserSettingData : UnimplemetedRecord
    {
        public UserSettingData(BinaryReader br)
            : base(br, 48)
        {
            
        }
    }
}