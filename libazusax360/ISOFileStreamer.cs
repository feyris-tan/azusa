using System;
using System.IO;

namespace libazusax360
{
    public class ISOFileStreamer : Stream
    {
        //TODO REWRITE THIS COMPLETELY

        // Fields
        private long _fsOffset;
        private BinaryReader instream;
        private long length;
        private long position;
        private long startOffset;
        
        public ISOFileStreamer(long FileSystemOffset, BinaryReader instream)
        {
            this._fsOffset = FileSystemOffset;
            this.instream = instream;
            this.length = instream.BaseStream.Length;
        }

        public ISOFileStreamer(long StartOffset, long length, BinaryReader instream)
        {
            this.startOffset = StartOffset;
            this.length = length;
            this.instream = instream;
        }

        public ISOFileStreamer(XFSInputFile xfs, long FileSystemOffset, BinaryReader instream)
        {
            this.startOffset = xfs.StartSector * 0x800L;
            this._fsOffset = FileSystemOffset;
            this.startOffset += this._fsOffset;
            this.length = xfs.GetFileLength();
            this.instream = instream;
        }

        public override void Flush()
        {
            this.instream.BaseStream.Flush();
        }

        public void GetNewFile(XFSInputFile xfs)
        {
            this.startOffset = xfs.StartSector * 0x800L;
            this.startOffset += this._fsOffset;
            this.length = xfs.GetFileLength();
            this.position = 0L;
        }
        
        public override int Read(byte[] buffer, int offset, int count)
        {
            int num = count;
            if ((count % 0x800) != 0)
            {
                num += 0x800 - (count % 0x800);
            }
            int sourceIndex = (int)(this.position % 0x800L);
            this.instream.BaseStream.Seek((this.position + this.startOffset) - sourceIndex, SeekOrigin.Begin);
            long length = this.length;
            if ((length % 0x800L) != 0L)
            {
                length += 0x800L - (length % 0x800L);
            }
            if ((this.position + num) > length)
            {
                num = (int)(length - this.position);
            }
            byte[] buffer2 = new byte[num];
            int num4 = this.instream.Read(buffer2, offset, num);
            if ((count + this.position) > this.length)
            {
                count = (int)(this.length - this.position);
            }
            if (num4 > count)
            {
                num4 = count;
            }
            num4 = Math.Min(num4, buffer2.Length - sourceIndex);
            Array.Copy(buffer2, sourceIndex, buffer, 0, num4);
            this.position += num4;
            return num4;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                this.instream.BaseStream.Seek(this.startOffset + offset, SeekOrigin.Begin);
                this.position = offset;
            }
            else if (origin == SeekOrigin.End)
            {
                this.instream.BaseStream.Seek((this.startOffset + this.length) - offset, SeekOrigin.Begin);
                this.position = this.length - offset;
            }
            else
            {
                this.instream.BaseStream.Seek((this.startOffset + this.position) + offset, SeekOrigin.Begin);
                this.position += offset;
            }
            return this.position;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.instream.BaseStream.Seek(this.position + this.startOffset, SeekOrigin.Begin);
            this.instream.BaseStream.Write(buffer, offset, count);
            this.position += count;
        }

        // Properties
        public override bool CanRead => this.instream.BaseStream.CanRead;

        public override bool CanSeek => this.instream.BaseStream.CanSeek;

        public override bool CanWrite => this.instream.BaseStream.CanWrite;
        
        public override long Length => this.length;

        public override long Position
        {
            get => this.position;
            set
            {
                this.instream.BaseStream.Seek(this.startOffset + value, SeekOrigin.Begin);
                this.position = value;
            }
        }
    }
}