using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    public class VendorPartCost
    {
        [JsonPropertyName("totalCost")]
        public double TotalCost { get; set; }

        [JsonPropertyName("unitCost")]
        public double UnitCost { get; set; }

        /// <summary>
        /// The part quantity associated with the cost.
        /// </summary>
        [JsonPropertyName("quantity")]
        public double Quantity { get; set; }

        /// <summary>
        /// The part quantity associated with the cost.
        /// </summary>
        [JsonPropertyName("minimumQuantity")]
        public double MinimumQuantity { get; set; }

        [JsonPropertyName("uom")]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        [JsonPropertyName("vendorPartNum")]
        public string VendorPartNumber { get; set; }
    }
}
