using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    public class TrackBlock
    {
        public TrackBlock(byte[] buffer, int offset)
        {
            mode = buffer[offset++];

            subchannel = buffer[offset++];

            adr_ctl = buffer[offset++];

            tno = buffer[offset++];

            point = buffer[offset++];

            min = buffer[offset++];

            sec = buffer[offset++];

            frame = buffer[offset++];

            zero = buffer[offset++];

            pmin = buffer[offset++];

            psec = buffer[offset++];

            pframe = buffer[offset++];

            extra_offset = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            sector_size = BitConverter.ToUInt16(buffer, offset);
            sector_size += 2;

            __dummy4__ = new byte[18];
            Array.Copy(buffer, offset, __dummy4__, 0, 18);
            sector_size += 18;

            start_sector = BitConverter.ToUInt32(buffer, offset);
            sector_size += 4;

            start_offset = BitConverter.ToUInt64(buffer, offset);
            offset += 8;

            number_of_files = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            footer_offset = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            __dummy6__ = new byte[24];
            Array.Copy(buffer, offset, __dummy6__, 0, 24);
            sector_size += 24;
        }

        byte mode; /* Track mode */
        byte subchannel; /* Subchannel mode */

        /* These are the fields from Sub-channel Q information, which are
           also returned in full TOC by READ TOC/PMA/ATIP command */
        byte adr_ctl; /* Adr/Ctl */
        byte tno; /* Track number field */
        byte point; /* Point field (= track number for track entries) */
        byte min; /* Min */
        byte sec; /* Sec */
        byte frame; /* Frame */
        byte zero; /* Zero */
        byte pmin; /* PMin */
        byte psec; /* PSec */
        byte pframe; /* PFrame */

        uint extra_offset; /* Start offset of this track's extra block. */
        ushort sector_size; /* Sector size. */

        byte[] __dummy4__;
        uint start_sector; /* Track start sector (PLBA). */
        ulong start_offset; /* Track start offset. */
        uint number_of_files; /* Number of files */
        uint footer_offset; /* Start offset of footer. */
        byte[] __dummy6__;
    }
}
