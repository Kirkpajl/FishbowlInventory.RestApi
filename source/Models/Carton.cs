using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Stores information about a carton.
    /// </summary>
    public class Carton
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("shipId")]
        public int ShipId { get; set; }

        [JsonPropertyName("cartonNum")]
        public int Number { get; set; }

        [JsonPropertyName("trackingNum")]
        public string TrackingNumber { get; set; }

        [JsonPropertyName("freightWeight")]
        public int FreightWeight { get; set; }

        [JsonPropertyName("freightAmount")]
        public int FreightAmount { get; set; }

        [JsonPropertyName("shippingItems")]
        public ShippingItem[] ShippingItems { get; set; }
    }
}
