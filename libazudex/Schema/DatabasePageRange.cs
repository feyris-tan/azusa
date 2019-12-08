using System;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class DatabasePageRange
    {
        internal DatabasePageRange(byte[] bytes)
        {
            _start = BitConverter.ToInt32(bytes, 0);
            _end = BitConverter.ToInt32(bytes, 4) + 1;
        }

        private int _start, _end;

        public int Start
        {
            get { return _start; }
        }

        public int End
        {
            get { return _end; }
        }

        public override string ToString()
        {
            return String.Format("{0} -> {1}", _start, _end);
        }
    }
}