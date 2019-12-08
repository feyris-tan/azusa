namespace moe.yo3explorer.azusa.VnDb.Entity
{
    public class VndbSearchResult
    {
        public int RID { get; set; }
        public string Title { get; set; }
        public string GTIN { get; set; }
        public string SKU { get; set; }
        public VndbVnResult[] Vns { get; set; }
    }
}
