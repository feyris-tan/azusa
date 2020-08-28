namespace moe.yo3explorer.azusa.OfflineReaders.VnDb.Entity
{
    public class VndbVnAnime
    {
        public string TitleRomanji { get; set; }
        public string TitleKanji { get; set; }
        public int Year { get; set; }

        public override string ToString()
        {
            return TitleRomanji ?? TitleKanji ?? "???";
        }
    }
}
