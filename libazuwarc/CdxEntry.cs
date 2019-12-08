using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace libazuwarc
{
    public class CdxEntry
    {
        public string Url
        {
            get => url;
            internal set => url = value;
        }

        public DateTime GrabDate
        {
            get => grabDate;
            internal set => grabDate = value;
        }

        public string Fingerprint
        {
            get => fingerprint;
            internal set => fingerprint = value;
        }

        public long Size
        {
            get => size;
            internal set => size = value;
        }

        public long Offset
        {
            get => offset;
            internal set => offset = value;
        }

        public Guid Uuid
        {
            get => uuid;
            internal set => uuid = value;
        }

        public FileInfo Warc
        {
            get => warc;
            internal set => warc = value;
        }

        private string url;
        private DateTime grabDate;
        private string unk1, unk2;
        private string fingerprint;
        private long size, offset;
        private Guid uuid;
        private FileInfo warc;
    }
}
