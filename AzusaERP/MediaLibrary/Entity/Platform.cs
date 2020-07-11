using System;
using moe.yo3explorer.azusa.Control.JsonIO;
using Newtonsoft.Json;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class Platform
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public bool IsSoftware { get; set; }
        [JsonConverter(typeof(UnixtimeToDatetimeJsonConverter))]
        public DateTime DateAdded { get; set; }

        public override string ToString()
        {
            return ShortName;
        }
    }
}
