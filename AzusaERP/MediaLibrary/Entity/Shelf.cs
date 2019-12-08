namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class Shelf
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool ShowSku { get; set; }
        public bool ShowRegion { get; set; }
        public bool ShowPlatform { get; set; }
        public bool IgnoreForStatistics { get; set; }
        public bool ScreenshotRequired { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
