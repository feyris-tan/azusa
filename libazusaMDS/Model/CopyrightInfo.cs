using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    public class CopyrightInfo
    {
        internal CopyrightInfo(byte[] buffer, int pointer)
        {
            copyProtected = buffer[pointer++];
            regionInfo = buffer[pointer++];
            unk1 = buffer[pointer++];
            unk2 = buffer[pointer++];
        }

        byte copyProtected, regionInfo, unk1, unk2;
    }
}
