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
    /// Stores a detail about a contact.There are no fields that are essential for the use of a Contact Information object. Use only those fields that you need.
    /// </summary>
    public class ContactInformation
    {
        [JsonPropertyName("contactName")]
        public string Name { get; set; }

        [JsonPropertyName("contactId")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public ContactType Type { get; set; }  //public int Type { get; set; }

        [JsonPropertyName("default")]
        public bool IsDefault { get; set; }

        [CsvPropertyName("datum")]
        public string Datum { get; set; }
    }
}
