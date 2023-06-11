using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    public class PartTracking
	{
        /// <summary>
        /// The unique identification number.
        /// </summary>
        [JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("abbr")]
		public string Abbreviation { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("sortOrder")]
		public int SortOrder { get; set; }

		[JsonPropertyName("trackingTypeID")]
		public int TrackingTypeId { get; set; }

		[JsonPropertyName("active")]
		public bool IsActive { get; set; }
	}
}
