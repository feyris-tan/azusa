using System;

namespace moe.yo3explorer.azusa.WarWalking.Entity
{
    public class Tour
    {
        public int ID { get; set; }
        public long Hash { get; set; }
        public DateTime RecordingStarted { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
