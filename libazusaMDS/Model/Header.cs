using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    public class Header
    {
        public Header(byte[] buffer, int offset = 0)
        {
            signature = new byte[16];
            Array.Copy(buffer, 0, signature, offset, 16);
            offset += 16;

            version = new byte[2];
            version[0] = buffer[offset++];
            version[1] = buffer[offset++];

            medium_type = (MediumType)BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            num_sessions = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            __dummy1__ = new ushort[2];
            __dummy1__[0] = BitConverter.ToUInt16(buffer, offset);
            offset += 2;
            __dummy1__[1] = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            bca_len = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            __dummy2__ = new uint[2];
            __dummy2__[0] = BitConverter.ToUInt32(buffer, offset);
            offset += 4;
            __dummy2__[1] = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            bca_data_offset = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            __dummy3__ = new uint[6];
            for (int i = 0; i < 6; i++)
            {
                __dummy3__[i] = BitConverter.ToUInt32(buffer, offset);
                offset += 4;
            }

            disc_structures_offset = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            __dummy4__ = new uint[3];
            __dummy4__[0] = BitConverter.ToUInt32(buffer, offset);
            offset += 4;
            __dummy4__[1] = BitConverter.ToUInt32(buffer, offset);
            offset += 4;
            __dummy4__[2] = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            sessions_blocks_offset = BitConverter.ToUInt32(buffer, offset);
            offset += 4;

            dpm_blocks_offset = BitConverter.ToUInt32(buffer, offset);
            offset += 4;
        }

        byte[] signature; /* "MEDIA DESCRIPTOR" */
        byte[] version; /* Version ? */
        MediumType medium_type; /* Medium type */
        ushort num_sessions; /* Number of sessions */
        ushort[] __dummy1__; /* Wish I knew... */
        ushort bca_len; /* Length of BCA data (DVD-ROM) */
        uint[] __dummy2__;
        uint bca_data_offset; /* Offset to BCA data (DVD-ROM) */
        uint[] __dummy3__; /* Probably more offsets */
        uint disc_structures_offset; /* Offset to disc structures */
        uint[] __dummy4__; /* Probably more offsets */
        uint sessions_blocks_offset; /* Offset to session blocks */
        uint dpm_blocks_offset; /* offset to DPM data blocks */

        internal bool Verify()
        {
            return Encoding.ASCII.GetString(signature).Equals("MEDIA DESCRIPTOR");
        }

        public ushort NumberOfSessions
        {
            get
            {
                return num_sessions;
            }
        }
        
        public uint SessionBlocksOffset
        {
            get
            {
                return sessions_blocks_offset;
            }
        }

        public uint DiscStructureOffset
        {
            get
            {
                return disc_structures_offset;
            }
        }

        public MediumType MediumType
        {
            get { return medium_type; }
        }
    }
}
