using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.BandcampImporter
{
    class BandcampAlbumEntry
    {
        public uint TrackNo { get; set; }
        public FileInfo FileInfo { get; set; }

        public override string ToString()
        {
            if (FileInfo == null)
                return base.ToString();

            return FileInfo.ToString();
        }
    }
}
