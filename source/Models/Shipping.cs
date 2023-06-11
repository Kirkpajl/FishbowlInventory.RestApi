using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Contains the details about a shipment.
    /// </summary>
    public class Shipping
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("orderNumber")]
        public int OrderNumber { get; set; }

        [JsonPropertyName("orderType")]
        public string OrderType { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonPropertyName("dateLastModified")]
        public DateTime? DateLastModified { get; set; }

        [JsonPropertyName("carrier")]
        public string Carrier { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        /// <summary>
        /// Origin
        /// </summary>
        [JsonPropertyName("fob")]
        public string FOB { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("cartonCount")]
        public int CartonCount { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("cartons")]
        public Carton[] Cartons { get; set; }
    }
}
