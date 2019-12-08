using System;

namespace moe.yo3explorer.azusa.VgmDb.Entity
{
    class Album
    {
        public string[] Products { get; set; }
        public string[] Artists { get; set; }
        public string Classification { get; set; }
        public Track[] Tracks { get; set; }
        public string[] Labels { get; set; }
        public string MediaFormat { get; set; }
        public string[] RelatedAlbums { get; set; }
        public string ReleaseEvent { get; set; }
        public string[] Reprints { get; set; }
        public string TypeName { get; set; }
        public Uri[] Websites { get; set; }
        public string Date { get; set; }
        public string Publisher { get; set; }
    }
}
