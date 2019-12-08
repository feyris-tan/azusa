using System;

namespace moe.yo3explorer.azusa.VgmDb.Entity
{
    public class AlbumListEntry
    {
        public int id { get; set; }
        public string catalog { get; set; }
        public DateTime? date { get; set; }
        public string typeName { get; set; }
        public string classificationName { get; set; }
        public string mediaformatName { get; set; }
        public string name { get; set; }
        public string publishformatName { get; set; }
        public string notes { get; set; }
        public string publisher { get; set; }
    }
}
