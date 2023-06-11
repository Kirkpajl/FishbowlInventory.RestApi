using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    public class ReceivedReceipt
    {
        [JsonPropertyName("itemType")]
        public int ItemType { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }

        [JsonPropertyName("locationID")]
        public int LocationId { get; set; }

        [JsonPropertyName("tracking")]
        public string Tracking { get; set; }
    }
}
