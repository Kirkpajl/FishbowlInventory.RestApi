using FishbowlInventory.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Api.Common
{    
    class FishbowlErrorResponse
    {
        [JsonPropertyName("timeStamp")]
        [JsonConverter(typeof(FishbowlDateTimeConverter))]
        public DateTime TimeStamp { get; set; }

        [JsonPropertyName("status")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }
    }
}
