using System;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class MediaType
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public bool GraphData { get; set; }
        public DateTime DateAdded { get; set; }
        public byte[] Icon { get; set; }
        public bool IgnoreForStatistics { get; set; }
        public string VnDbKey { get; set; }

        public override string ToString()
        {
            return ShortName;
        }
    }
}
