using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace moe.yo3explorer.azusa.Control.JsonIO
{
    class UnixtimeToDatetimeJsonConverter : JsonConverter<DateTime>
    {
        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            long utime =  (long)reader.Value;
            return UnixTimeConverter.FromUnixTime(utime);
        }
    }
}
