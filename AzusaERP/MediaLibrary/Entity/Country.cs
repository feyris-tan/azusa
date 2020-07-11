using System;
using moe.yo3explorer.azusa.Control.JsonIO;
using Newtonsoft.Json;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class Country
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public string BroadcastTelevisionSystem { get; set; }
        public string CurrencyName { get; set; }
        public double CurrencyConversion { get; set; }
        public int LanguageId { get; set; }
        public short PowerVoltage { get; set; }
        public byte PowerFrequency { get; set; }
        public byte DvdRegion { get; set; }
        public char BlurayRegion { get; set; }
        [JsonConverter(typeof(UnixtimeToDatetimeJsonConverter))]
        public DateTime DateAdded { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
