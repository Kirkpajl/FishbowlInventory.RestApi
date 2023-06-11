using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Stores tax rate information.
    /// </summary>
    public class TaxRate
    {
        /// <summary>
        /// The unique identification number
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The name of the object.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// The tax rate.
        /// </summary>
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }

        [JsonPropertyName("typeID")]
        public int TypeId { get; set; }

        [JsonPropertyName("vendorID")]
        public int VendorId { get; set; }

        [JsonPropertyName("defaultFlag")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("activeFlag")]
        public bool IsActive { get; set; }
    }
}
