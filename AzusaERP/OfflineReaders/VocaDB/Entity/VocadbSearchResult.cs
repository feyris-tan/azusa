using System;

namespace moe.yo3explorer.azusa.OfflineReaders.VocaDB.Entity
{
    public class VocadbSearchResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ArtistString { get; set; }
        public string DiscType { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CatalogNumber { get; set; }
    }
}
