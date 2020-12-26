using System;
using System.IO;

namespace moe.yo3explorer.azusa.Utilities.FolderMapper.Control
{
    public class RawCdRomStream : Stream
    {
        public bool not_bin;
        public string error_message;

        public RawCdRomStream(FileInfo _str)
            : this(_str.OpenRead())
        {
        }

        public RawCdRomStream(Stream _io)
        {
            fsource = _io;
            buffer = new byte[4096];
            _io.Position = 0;
            _io.Read(buffer, 0, 16);
            if (memory_compare(SYNC_DATA, buffer, 12) == 0)
            {
                switch (buffer[15])
                {
                    case 2:
                        mode = 2;
                        sector_size = 2352;
                        if ((_io.Length % 2352) != 0)   //copy-protected Playstation CD's use subchannel data, therefore their sectors are larger than normal MODE 2 sectors
                        {
                            sector_size = 2448;
                        }
                        break;
                    case 1:
                        mode = 1;
                        sector_size = 2448;
                        if ((_io.Length % 2448) != 0)   //this is needed by Konami Twinkle CD-ROMs
                        {
                            sector_size = 2352;
                        }
                        break;
                }
                if (sector_size == 0)
                {
                    error_message = "can't find sector size!";
                    not_bin = true;
                }
                if (seek_pvd(buffer, sector_size, mode) == 0)
                {
                    error_message = "Could not find PVD!";
                    not_bin = true;
                }
            }
            else
            {
                error_message = "Didn't find sync data!";
                not_bin = true;
                return;
            }
            fsource.Position = 0;
        }

        byte[] buffer;
        long current_sector_id;
        Stream fsource;
        long internal_position;
        int sector_size;
        int mode;
        byte[] PVD_STRING = { 0x01, 0x43, 0x44, 0x30, 0x30, 0x31, 0x01, 0 }; //"\x01" "CD001" "\x01" "\0";
        byte[] SYNC_DATA = { 0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0 };

        private int sector_read(byte[] buffer)
        {
            int status;
            if (sector_size == 2352) fsource.Position += 16;
            if (sector_size == 2448) fsource.Position += 16;
            if (mode == 2) fsource.Position += 8;
            status = fsource.Read(buffer, 0, 2048);
            if (sector_size >= 2336)
            {
                fsource.Position += 280;
                if (mode == 1) fsource.Position += 8;
            }
            return status;
        }

        public override int ReadByte()
        {
            //get requested sector
            long requested_sector = (internal_position) / 2048;
            if (requested_sector != current_sector_id)
            {
                long where_to_look = requested_sector * sector_size;
                fsource.Position = where_to_look;
                sector_read(buffer);
                current_sector_id = requested_sector;
            }
            int peek_pos = (int)(internal_position % 2048);
            internal_position++;
            return buffer[peek_pos];
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[i + offset] = (byte)ReadByte();
            }
            return count;
        }

        private int memory_compare(byte[] left, byte[] right, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (left[i] != right[i])
                {
                    return left[i] - right[i];
                }
            }
            return 0;
        }

        private int seek_pvd(byte[] buffer, int sector_size, int mode)
        {
            fsource.Position = 0;
            fsource.Position += sector_size * 16;
            if (sector_size == 2352) fsource.Position += 16;
            if (sector_size == 2448) fsource.Position += 16;
            if (mode == 2) fsource.Position += 8;
            fsource.Read(buffer, 0, 8);
            if (memory_compare(PVD_STRING, buffer, 8) == 0) return 1;
            return 0;
        }

        public override bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get
            {
                long editorial = fsource.Length;
                editorial /= 2352;
                editorial *= 2048;
                return editorial;
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                internal_position = value;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public long TotalSectors
        {
            get
            {
                return fsource.Length / sector_size;
            }
        }

        public override void Close()
        {
            fsource.Close();
        }
    }
}
