using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    enum TrackMode : byte
    {
        UNKNOWN = 0x00,
        AUDIO = 0xA9, /* sector size = 2352 */
        MODE1 = 0xAA, /* sector size = 2048 */
        MODE2 = 0xAB, /* sector size = 2336 */
        MODE2_FORM1 = 0xAC, /* sector size = 2048 */
        MODE2_FORM2 = 0xAD  /* sector size = 2324 (+4) */
    }
}
