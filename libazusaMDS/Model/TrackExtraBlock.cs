using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    class TrackExtraBlock
    {
        public TrackExtraBlock(byte[] buffer, int offset)
        {
            pregap = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            length = BitConverter.ToUInt32(buffer, offset);
            length += 4;
        }

        uint pregap, length;
    }
}
