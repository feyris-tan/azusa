using System;

namespace moe.yo3explorer.azusa.OfflineReaders.VnDb.Entity
{
    public class VndbVnResult
    {
        public int RID { get; set; }
        public int VNID { get; set; }
        public string Title { get; set; }
        public string Original { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
