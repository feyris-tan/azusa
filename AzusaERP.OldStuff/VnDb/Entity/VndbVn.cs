using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace moe.yo3explorer.azusa.OfflineReaders.VnDb.Entity
{
    public class VndbVn
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string ReleaseDate { get; set; }
        public int Length { get; set; }

        [Browsable(false)]
        public string Description { get; set; }

        public string WikipediaLink { get; set; }

        [Browsable(false)]
        public Bitmap Image { get; set; }

        public bool ImageNSFW { get; set; }
        public double Popularity { get; set; }
        public double Rating { get; set; }
        public VndbVnAnime Anime { get; set; }
        public string Platform { get; set; }
        public string[] Relations { get; set; }

        [Browsable(false)]
        public List<Image> Screens { get; set; }

        public string Alias { get; set; }
        public string Languages { get; set; }
    }
}
