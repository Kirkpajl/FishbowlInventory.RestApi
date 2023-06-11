using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    internal class FishbowlErrorResponse
    {
        [JsonPropertyName("timeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }



        //{"timeStamp":"2022-10-21T15:40:18.531-0400","status":401,"message":"Invalid authorization token.","path":"/api/integrations/plugin-info"}
    }
}
