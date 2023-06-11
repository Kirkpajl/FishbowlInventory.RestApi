using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Stores credit card information to be used in a credit card payment.
    /// </summary>
    public class CreditCard
    {
        [JsonPropertyName("cardNumber")]
        public string Number { get; set; }
        
        [JsonPropertyName("cardExpMonth")]
        public int ExpMonth { get; set; }

        [JsonPropertyName("cardExpYear")]
        public int ExpYear { get; set; }

        [JsonPropertyName("securityCode")]
        public string SecurityCode { get; set; }

        [JsonPropertyName("nameOnCard")]
        public string NameOnCard { get; set; }

        [JsonPropertyName("cardAddress")]
        public string Address { get; set; }

        [JsonPropertyName("cardZipCode")]
        public string ZipCode { get; set; }

        [JsonPropertyName("cardCountryCode")]
        public string CountryCode { get; set; }
    }
}
