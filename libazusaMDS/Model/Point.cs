using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    enum Point
    {
        TRACK_FIRST = 0xA0, /* info about first track */
        TRACK_LAST = 0xA1, /* info about last track  */
        TRACK_LEADOUT = 0xA2  /* info about lead-out    */
    }
}
