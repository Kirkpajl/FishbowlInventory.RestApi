using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// A list of Custom Items
    /// </summary>
    public class CustomList
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Custom List Item objects
        /// </summary>
        [JsonPropertyName("customListItems")]
        public CustomListItem[] CustomListItems { get; set; }
    }
}
