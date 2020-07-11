using System;
using moe.yo3explorer.azusa.Control.JsonIO;
using Newtonsoft.Json;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPeriodicEvent { get; set; }
        public string URL { get; set; }
        [JsonConverter(typeof(UnixtimeToDatetimeJsonConverter))]
        public DateTime DateAdded { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
