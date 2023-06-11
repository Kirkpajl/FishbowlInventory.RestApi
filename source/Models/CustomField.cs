using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Stores the details about a Fishbowl custom field.
    /// </summary>
    public class CustomField
    {
        /// <summary>
        /// The custom field's unique identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The name of the custom field.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The custom field's value.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }

        /// <summary>
        /// The category of the custom field's value.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        //[JsonPropertyName("description")]
        //public string Description { get; set; }

        //[JsonPropertyName("sortOrder")]
        //public int SortOrder { get; set; }

        //[JsonPropertyName("info")]
        //public string Info { get; set; }

        //[JsonPropertyName("requiredFlag")]
        //public bool IsRequired { get; set; }

        //[JsonPropertyName("activeFlag")]
        //public bool IsActive { get; set; }
        
        ///// <summary>
        ///// Custom list objects
        ///// </summary>
        //[JsonPropertyName("customList")]
        //public CustomList CustomList { get; set; }
    }
}
