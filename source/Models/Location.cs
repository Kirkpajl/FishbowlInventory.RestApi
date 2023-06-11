using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Represents a Location.
    /// </summary>
    public class Location
    {
        [JsonPropertyName("locationID")]
        public int LocationId { get; set; }

        [JsonPropertyName("typeID")]
        public int TypeId { get; set; }

        [JsonPropertyName("parentID")]
        public int ParentId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("countedAsAvailable")]
        public bool CountedAsAvailable { get; set; }

        [JsonPropertyName("default")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("pickable")]
        public bool IsPickable { get; set; }

        [JsonPropertyName("receivable")]
        public bool IsReceivable { get; set; }

        [JsonPropertyName("locationGroupID")]
        public int LocationGroupId { get; set; }

        [JsonPropertyName("locationGroupName")]
        public string LocationGroupName { get; set; }

        [JsonPropertyName("enforceTracking")]
        public bool EnforceTracking { get; set; }

        [JsonPropertyName("sortOrder")]
        public int SortOrder { get; set; }
    }
}
