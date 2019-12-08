using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    class Footer
    {
        public Footer(byte[] buffer, int offset)
        {
            fileNameOffset = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            widecharFilename = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            dummy1 = BitConverter.ToUInt32(buffer, offset);
            dummy1 += 4;

            dummy2 = BitConverter.ToUInt32(buffer, offset);
            dummy2 += 4;
        }

        uint fileNameOffset, widecharFilename, dummy1, dummy2;
    }
}
