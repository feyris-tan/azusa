using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace moe.yo3explorer.azusa.Control.JsonIO
{
    class WeirdQuarkusDatetimeConverter : JsonConverter<DateTime>
    {
        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            string resultWithTimezone = reader.Value.ToString();
            string result = resultWithTimezone;
            bool isUtc = result.EndsWith("[UTC]");
            bool endsWithZ = result.EndsWith("Z") && Char.IsDigit(result.ToCharArray()[result.Length - 2]);
            while (!result.EndsWith("Z"))
                result = result.Substring(0, result.Length - 1);
            DateTime output = ParseISO8601String(result);
            if (isUtc)
                output -= new TimeSpan(0, 2, 0, 0);
            else if (endsWithZ)
                output -= new TimeSpan(0, 2, 0, 0);
            else
                throw new TimeZoneNotFoundException(resultWithTimezone);
            return output;
        }

        //Found on https://stackoverflow.com/questions/3556144/how-to-create-a-net-datetime-from-iso-8601-format
        static readonly string[] formats = { 
            // Basic formats
            "yyyyMMddTHHmmsszzz",
            "yyyyMMddTHHmmsszz",
            "yyyyMMddTHHmmssZ",
            // Extended formats
            "yyyy-MM-ddTHH:mm:sszzz",
            "yyyy-MM-ddTHH:mm:sszz",
            "yyyy-MM-ddTHH:mm:ssZ",
            // All of the above with reduced accuracy
            "yyyyMMddTHHmmzzz",
            "yyyyMMddTHHmmzz",
            "yyyyMMddTHHmmZ",
            "yyyy-MM-ddTHH:mmzzz",
            "yyyy-MM-ddTHH:mmzz",
            "yyyy-MM-ddTHH:mmZ",
            // Accuracy reduced to hours
            "yyyyMMddTHHzzz",
            "yyyyMMddTHHzz",
            "yyyyMMddTHHZ",
            "yyyy-MM-ddTHHzzz",
            "yyyy-MM-ddTHHzz",
            "yyyy-MM-ddTHHZ",
            // idk lol, reasteasy sucks
            "yyyy-MM-ddTHH:mm:ss.fffZ",
            "yyyy-MM-ddTHH:mm:ss.fffZ[K]",
            "yyyy-MM-ddZ",
            "yyyy-MM-ddTHH:mm:ss.ffZ"
        };

        public static DateTime ParseISO8601String(string str)
        {
            return DateTime.ParseExact(str, formats,
                CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
    }
}
