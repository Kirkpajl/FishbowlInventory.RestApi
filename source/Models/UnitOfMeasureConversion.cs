using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Stores conversion information for a Unit of Measure.
    /// </summary>
    public class UnitOfMeasureConversion
    {
        /// <summary>
        /// The conversion's unique identification number.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The conversion description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// The resulting UOM of the conversion.
        /// </summary>
        [JsonPropertyName("targetUom")]
        public UnitOfMeasure TargetUom { get; set; }

        /// <summary>
        /// Divide the quantity by the divisor to convert to the target UOM.
        /// </summary>
        [JsonPropertyName("divisor")]
        public double Divisor { get; set; }

        /// <summary>
        /// Multiply the quantity by the multiplier to convert to the target UOM.
        /// </summary>
        [JsonPropertyName("multiplier")]
        public double Multiplier { get; set; }
    }
}
