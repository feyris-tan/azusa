using System.IO;

namespace moe.yo3explorer.azusa.Utilities.BandcampImporter
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
