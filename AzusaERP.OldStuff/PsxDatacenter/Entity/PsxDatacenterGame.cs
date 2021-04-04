using System;
using System.ComponentModel;

namespace moe.yo3explorer.azusa.OfflineReaders.PsxDatacenter.Entity
{
    public class PsxDatacenterGame
    {
        public string Platform { get; set; }
        public string SKU { get; set; }
        public string Title { get; set; }
        public string Languages { get; set; }
        public string CommonTitle { get; set; }
        public string Region { get; set; }
        public string Genre { get; set; }
        public string DeveloperId { get; set; }
        public string PublisherId { get; set; }
        public DateTime DateRelease { get; set; }

        [Browsable(false)]
        public byte[] Cover { get; set; }

        [Browsable(false)]
        public string Description { get; set; }

        public string Barcode { get; set; }
    }
}
