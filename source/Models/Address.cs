using FishbowlInventory.Enumerations;
using FishbowlInventory.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// An object containing information about an address. There are no fields that are essential for the use of an address object. Use only those fields that you need.
    /// </summary>
    public class Address
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("temp-Account")]
        public TempAccount TempAccount { get; set; }

        /// <summary>
        /// The name on the address.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("attn")]
        public string Attention { get; set; }

        /// <summary>
        /// The street line.
        /// </summary>
        [JsonPropertyName("street")]
        public string StreetAddress { get; set; }

        /// <summary>
        /// The city.
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// The state's abbreviation.
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }

        /// <summary>
        /// The postal code.
        /// </summary>
        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The country's abbreviation.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("locationGroupId")]
        public int LocationGroupId { get; set; }

        [JsonPropertyName("default")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("residential")]
        public bool IsResidential { get; set; }

        [JsonPropertyName("type")]
        public AddressType Type { get; set; } = AddressType.MainOffice;  //public string Type { get; set; }

        [JsonPropertyName("addressInformationList")]
        public AddressInformation[] AddressInformation { get; set; }





        [CsvPropertyName("contacts")]
        public ICollection<ContactInformation> Contacts { get; } = new HashSet<ContactInformation>();
    }
}
