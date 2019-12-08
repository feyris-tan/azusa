using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    enum SubChannel
    {
        NONE = 0x00, /* no subchannel */
        PW_INTERLEAVED = 0x08  /* 96-byte PW subchannel, interleaved */
    }
}
