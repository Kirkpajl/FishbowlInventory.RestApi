using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// This is an object representing a Fishbowl location group.
    /// </summary>
    public class LocationGroup
    {
        /// <summary>
        /// The location group's unique identification number.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The location group's name.
        /// </summary>
        [JsonPropertyName("name"), Required]
        public string Name { get; set; }

        /// <summary>
        /// The class category.
        /// </summary>
        [JsonPropertyName("class")]
        public Category Class { get; set; }

        /// <summary>
        /// The location group's active status.
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}
