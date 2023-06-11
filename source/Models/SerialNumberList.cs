using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// A list of serial numbers.
    /// </summary>
    public class SerialNumberList
    {
        /// <summary>
        /// A list of serial number values.
        /// </summary>
        [JsonPropertyName("numbers")]
        public SerialNumber[] Numbers { get; set; }
    }
}
