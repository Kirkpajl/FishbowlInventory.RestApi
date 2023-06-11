using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Converters
{
    public class FishbowlDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));

            string dateTimeString = reader.GetString();  // "2022-01-25T19:22:53.621-0500"

            // Insert the missing colon in the TimeZoneOffset portion of the serialized DateTime string
            if (dateTimeString.Length == 28)
                dateTimeString = dateTimeString.Insert(26, ":");

            return DateTime.Parse(dateTimeString);
            //return DateTime.Parse(reader.GetString());



            //return DateTime.ParseExact(reader.GetString(), "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff('+'/'-')HHmm", null).ToUniversalTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffZ"));
        }
    }

}
