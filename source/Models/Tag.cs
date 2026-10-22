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
        public decimal Quantity { get; set; } //~~~~~ type int -> decimal

        [JsonPropertyName("quantityCommitted")]
        public decimal QuantityCommitted { get; set; } //~~~~~ type int -> decimal

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
