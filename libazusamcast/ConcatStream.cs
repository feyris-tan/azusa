using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tsubasa.IO
{
    public class ConcatStream : Stream
    {
        Stream[] _parts;
        byte _current_part_no;
        Stream _current_part;
        
        public ConcatStream(Stream[] _s)
        {
            _parts = _s;
            _current_part = _parts[0];
        }

        public ConcatStream(List<Stream> _ls)
            : this(_ls.ToArray())
        {
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

        public override long Length
        {
            get
            {
                long result = 0;
                foreach (Stream part in _parts)
                {
                    result += part.Length;
                }
                return result;
            }
        }

        public override long Position
        {
            get
            {
                if (_current_part_no == 0)
                {
                    return _current_part.Position;
                }
                else
                {
                    long result = 0;
                    for (int i = 0; i < _current_part_no; i++)
                    {
                        result += _parts[i].Length;
                    }
                    result += _current_part.Position;
                    return result;
                }
            }
            set
            {
                long target = value;
                //long tmptarget = value;
                sbyte tmppart = -1;
                for (int i = 0; i < _parts.Length; i++)
                {
                    _parts[i].Position = 0;
                }
                while (target > 0)
                {
                    tmppart++;
                    if (_parts[tmppart].Length > target)
                    {
                        _current_part_no = (byte)tmppart;
                        _current_part = _parts[tmppart];
                        _current_part.Position = target;
                        return;
                    }
                    target -= _parts[tmppart].Length;
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int rv = 0;
            for (int i = 0; i < count; i++)
            {
                buffer[offset + i] = (byte)ReadByte();
                rv++;
            }
            return rv;
        }

        public override int ReadByte()
        {
            if (_current_part.Position == _current_part.Length)
            {
                _current_part_no++;
                _current_part = _parts[_current_part_no];
                _current_part.Position = 0;
            }
            return _current_part.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long target = 0;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    target = offset;
                    break;
                case SeekOrigin.Current:
                    target = Position + offset;
                    break;
                case SeekOrigin.End:
                    target = Length - offset;
                    break;
            }
            Position = target;
            return Position;
        }

        #region Unnessecary

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }
        
        protected override void Dispose(bool disposing)
        {
            for (int i = 0; i < _parts.Length; i++)
            {
                _parts[i].Dispose();
            }
        }

        #endregion Unnessecary
    }
}