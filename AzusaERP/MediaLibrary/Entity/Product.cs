using System;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public int InShelf { get; set; }
        public string Name { get; set; }
        public bool Complete { get; set; }
        public byte[] Picture { get; set; }
        public double Price { get; set; }
        public DateTime BoughtOn { get; set; }
        public string Sku { get; set; }
        public int PlatformId { get; set; }
        public int SupplierId { get; set; }
        public int CountryOfOriginId { get; set; }
        public byte[] Screenshot { get; set; }
        public DateTime DateAdded { get; set; }
        public bool Consistent { get; set; }
        public bool NSFW { get; set; }
    }
}
