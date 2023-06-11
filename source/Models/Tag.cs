using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// A number that helps describe a location.
    /// </summary>
    public class Tag
    {
        [JsonPropertyName("tagID")]
        public int Id { get; set; }

        [JsonPropertyName("num")]
        public string Number { get; set; }

        [JsonPropertyName("partNum")]
        public string PartNumber { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("quantityCommitted")]
        public int QuantityCommitted { get; set; }

        [JsonPropertyName("woNum")]
        public string WorkOrderNumber { get; set; }

        [JsonPropertyName("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [JsonPropertyName("tracking")]
        public TrackingItem Tracking { get; set; }

        [JsonPropertyName("typeID")]
        public int TypeId { get; set; }

        [JsonPropertyName("accountID")]
        public int AccountId { get; set; }
    }
}
