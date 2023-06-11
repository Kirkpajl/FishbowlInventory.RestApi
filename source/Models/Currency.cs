using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// The currency used on the order.
    /// </summary>
    public class Currency
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

        /// <summary>
        /// The currency code.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// The currency exchange rate.
        /// </summary>
        [JsonPropertyName("rate")]
        public double Rate { get; set; }

        /// <summary>
        /// Indicates if the currency is the home currency.
        /// </summary>
        [JsonPropertyName("homeCurrency")]
        public bool HomeCurrency { get; set; }
    }
}
