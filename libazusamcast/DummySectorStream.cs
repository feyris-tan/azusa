using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace com.dyndns_server.yo3explorer.yo3explorer.Dreamcast
{
    internal class DummySectorStream : Stream
    {
        long length;
        long position;

        public DummySectorStream(uint count)
        {
            length = count * 2352;
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
            get { return length; }
        }

        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                if (value > length)
                {
                    throw new Exception();
                }
                position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = 0;
            for (int i = 0; i < count; i++)
            {
                buffer[offset + i] = 0;
                result++;
                position++;
                if (position > length)
                    throw new Exception();
            }
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = offset;
                    return offset;
                case SeekOrigin.Current:
                    position += offset;
                    return position;
                case SeekOrigin.End:
                    position = (length - offset);
                    return position;
            }
            throw new Exception();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}