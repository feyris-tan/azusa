using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    public class SessionBlock
    {
        internal SessionBlock(byte[] buffer, int offset)
        {
            session_start = BitConverter.ToInt32(buffer, offset);
            offset += 4;

            session_end = BitConverter.ToInt32(buffer, offset);
            offset += 4;

            session_number = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            num_all_blocks = buffer[offset++];

            num_nontrack_blocks = buffer[offset++];

            first_track = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            last_track = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            __dummy1__ = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            tracks_blocks_offset = BitConverter.ToUInt32(buffer, offset);
            offset += 4;
        }

        int session_start; /* Session's start address */
        int session_end; /* Session's end address */
        ushort session_number; /* Session number */
        byte num_all_blocks; /* Number of all data blocks. */
        byte num_nontrack_blocks; /* Number of lead-in data blocks */
        ushort first_track; /* First track in session */
        ushort last_track; /* Last track in session */
        uint __dummy1__; /* (unknown) */
        uint tracks_blocks_offset; /* Offset of lead-in+regular track data blocks. */

        public ushort FirstTrack
        {
            get
            {
                return first_track;
            }
        }

        public ushort LastTrack
        {
            get
            {
                return last_track;
            }
        }

        public uint TrackBlockOffset
        {
            get
            {
                return tracks_blocks_offset;
            }
        }

        public int SessionEnd
        {
            get
            {
                return session_end;
            }
        }
    }
}
