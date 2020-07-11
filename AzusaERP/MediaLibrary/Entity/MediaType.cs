using System;
using moe.yo3explorer.azusa.Control.JsonIO;
using Newtonsoft.Json;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class MediaType
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public bool GraphData { get; set; }
        [JsonConverter(typeof(UnixtimeToDatetimeJsonConverter))]
        public DateTime DateAdded { get; set; }
        public byte[] Icon { get; set; }
        public bool IgnoreForStatistics { get; set; }
        public string VnDbKey { get; set; }
        public bool HasFilesystem { get; set; }

        public override string ToString()
        {
            return ShortName;
        }
    }
}
