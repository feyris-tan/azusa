using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace com.dyndns_server.yo3explorer.yo3explorer.Dreamcast
{
    public class GDRomStream : Stream, IDisposable
    {
        public static readonly byte[] PVD_STRING = { 0x01, 0x43, 0x44, 0x30, 0x30, 0x31, 0x01, 0 }; //"\x01" "CD001" "\x01" "\0";
        public static readonly byte[] SVD_STRING = { 0x02, 0x43, 0x44, 0x30, 0x30, 0x31, 0x01, 0 }; //"\x02" "CD001" "\x01" "\0";
        public static readonly byte[] VDT_STRING = { 0xff, 0x43, 0x44, 0x30, 0x30, 0x31, 0x01, 0 }; //"\xFF" "CD001" "\x01" "\0";
        public static readonly byte[] SYNC_DATA = { 0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0 };
        public static readonly byte[] SUB_HEADER = { 0, 0, 0x08, 0, 0, 0, 0x08, 0 };
        byte[] buffer;
        MemoryStream fdest;
        int sector_size = -1;
        int mode = -1;
        private long internal_length;

        internal GDRomStream(FileInfo _fi)
            : this(_fi.OpenRead())
        {
        }

        internal GDRomStream(Stream _instr)
        {
            internal_length = _instr.Length;
            int start_lba = 45000;
            fsource = _instr;
            //long image_length = fsource.Length;
            buffer = new byte[4096];

            fsource.Read(buffer, 0, 16);
            if (memory_compare(SYNC_DATA, buffer, 12) == 0)
            {
                sector_size = 2352;
                switch (buffer[15])
                {
                    case 2:
                        mode = 2;
                        break;
                    case 1:
                        mode = 1;
                        break;
                        throw new Exception("Unsupported track mode");
                }
                if (seek_pvd(buffer, 2352, mode) == 0)
                {
                    throw new Exception("Cound not find PVD!");
                }
            }

            Console.Write(".");
            fdest = new MemoryStream();
            fsource.Position = 0;
            for (int i = 0; i < 16; i++)
            {
                sector_read(buffer, sector_size, mode);
                fdest.Write(buffer, 0, 2048);
            }
            long last_pos = fsource.Position;
            bool last_vd = false;

            Console.Write(".");
            do
            {
                sector_read(buffer, sector_size, mode);
                if (memory_compare(PVD_STRING, buffer, 8) == 0)
                {
                    //PVD
                }
                else if (memory_compare(SVD_STRING, buffer, 8) == 0)
                {
                    //SVD
                }
                else if (memory_compare(VDT_STRING, buffer, 8) == 0)
                {
                    //VDT
                    last_vd = true;
                }
                else
                {
                    throw new Exception("Found unknown Volume Descriptor");
                }
                fdest.Write(buffer, 0, 2048);
                last_pos = fsource.Position;
            } while (last_vd == false);

            // add padding
            Console.Write(".");
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = 0;

            Console.Write(".");
            long remaining_length = 300 - (last_pos / sector_size);
            for (int i = 0; i < remaining_length; i++)
            {
            }

            Console.Write(".");
            if (last_pos > (start_lba * sector_size))
            {
                throw new Exception("Sorry, LBA value is too small...");
            }
            if (start_lba < 11700)
            {
                Console.WriteLine("LBA value seems fishy");
            }

            Console.Write(".");
            remaining_length = start_lba - (last_pos / sector_size);
            long progress;
            for (int i = 0; i < remaining_length; i++)
            {
                if ((i % 512) == 0)
                {
                    progress = i * 100 / remaining_length;
                }
                fdest.Write(buffer, 0, 2048);
            }
            header_length = fdest.Position - 2048;

            //append iso
            fsource.Position = 0;
            //remaining_length = image_length / sector_size;
            //body_length = remaining_length * 2048;
            for (int i = 0; i < remaining_length; i++)
            {
            }
            internal_position = 0;
            fdest.Position = 0;
            current_sector_id = -1;
            current_sector = new byte[2048];
        }

        #region Initial stuff...

        private int seek_pvd(byte[] buffer, int sector_size, int mode)
        {
            fsource.Position = 0;
            fsource.Position += sector_size * 16;
            if (sector_size == 2352) fsource.Position += 16;
            if (mode == 2) fsource.Position += 8;
            fsource.Read(buffer, 0, 8);
            if (memory_compare(PVD_STRING, buffer, 8) == 0) return 1;
            return 0;
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

        private int sector_read(byte[] buffer, int sector_size, int mode)
        {
            int status;
            if (sector_size == 2352) fsource.Position += 16;
            if (mode == 2) fsource.Position += 8;
            status = fsource.Read(buffer, 0, 2048);
            if (sector_size >= 2336)
            {
                fsource.Position += 280;
                if (mode == 1) fsource.Position += 8;
            }
            return status;
        }

        Stream fsource;
        long header_length;

        #endregion Initial stuff...

        long internal_position;
        long current_sector_id;
        byte[] current_sector;
        
        public override int ReadByte()
        {
            if (internal_position < header_length)
            {
                fdest.Position = internal_position;
                internal_position++;
                return fdest.ReadByte();
            }
            else
            {
                //get requested sector
                long requested_sector = (internal_position - header_length) / 2048;
                requested_sector--;
                if (requested_sector != current_sector_id)
                {
                    /*fsource.Position = sector_table[(int)requested_sector];
                    fsource.Read(current_sector, 0, 2048);
                    fsource.Position = sector_table[(int)requested_sector];
                    current_sector_id = requested_sector;*/
                    long where_to_look = requested_sector * 2352;
                    fsource.Position = where_to_look;
                    sector_read(buffer, sector_size, mode);
                    current_sector_id = requested_sector;
                }
                int peek_pos = (int)(internal_position % 2048);
                internal_position++;
                return buffer[peek_pos];
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[i + offset] = (byte)ReadByte();
            }
            return count;
        }

        #region Trolls

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    internal_position = offset;
                    break;
                case SeekOrigin.Current:
                    internal_position += offset;
                    break;
                case SeekOrigin.End:
                    internal_position = Length - offset;
                    break;
            }
            return internal_position;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { return internal_length; }
        }

        public override long Position
        {
            get
            {
                return internal_position;
            }
            set
            {
                internal_position = value;
            }
        }

        #endregion Trolls

        void IDisposable.Dispose()
        {
            fdest.Dispose();
            current_sector = null;
        }
    }
}