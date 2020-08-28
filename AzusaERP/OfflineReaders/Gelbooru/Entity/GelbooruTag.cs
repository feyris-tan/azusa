namespace moe.yo3explorer.azusa.OfflineReaders.Gelbooru.Entity
{
    public class GelbooruTag
    {
        public string Tag { get; set; }
        public int NumberOfImages { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return Tag;
        }
    }
}
