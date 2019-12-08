using System.ComponentModel;

namespace moe.yo3explorer.azusa.VnDb.Entity
{
    public class VndbRelease
    {
        public string OriginalTitle { get; set; }
        public string Title { get; set; }
        public string Released { get; set; }
        public string Type { get; set; }
        public bool IsPatch { get; set; }
        public bool IsFreeware { get; set; }
        public bool IsDoujin { get; set; }

        [Browsable(false)]
        public string Notes { get; set; }

        public int AgeRestriction { get; set; }
        public string GTIN { get; set; }
        public string SKU { get; set; }
        public string Resolution { get; set; }
        public string Language { get; set; }
        public string Media { get; set; }
        public string Platforms { get; set; }
        public string Producers { get; set; }
        public string Website { get; set; }
    }
}
