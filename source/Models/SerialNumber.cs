using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Serial number value.
    /// </summary>
    public class SerialNumber
    {
        /// <summary>
        /// The type of tracking being set.
        /// </summary>
        [JsonPropertyName("partTracking")]
        public PartTracking PartTracking { get; set; }

        /// <summary>
        /// The serial number value.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
