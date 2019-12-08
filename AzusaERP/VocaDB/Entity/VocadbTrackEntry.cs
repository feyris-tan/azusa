using System;

namespace moe.yo3explorer.azusa.VocaDB.Entity
{
    public class VocadbTrackEntry
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public string Name { get; set; }
        public int AlbumId { get; set; }
        public int DiscNumber { get; set; }
        public int SongId { get; set; }
        public int TrackNumber { get; set; }
    }
}
