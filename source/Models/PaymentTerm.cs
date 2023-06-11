using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// The terms of payment on the order.
    /// </summary>
    public class PaymentTerm
    {
        /// <summary>
        /// The unique identification number.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The name of the object.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
