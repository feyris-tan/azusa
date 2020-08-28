using System;
using AzusaERP.OldStuff;

namespace moe.yo3explorer.azusa.WarWalking.Entity
{
    public class Tour
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("hash")]
        public long Hash { get; set; }
        [JsonProperty("utimeRecordingStarted")]
        [JsonConverter(typeof(UnixtimeToDatetimeJsonConverter))]
        public DateTime RecordingStarted { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("dateAdded")]
        [JsonConverter(typeof(UnixtimeToDatetimeJsonConverter))]
        public DateTime DateAdded { get; set; }
    }

    
}
