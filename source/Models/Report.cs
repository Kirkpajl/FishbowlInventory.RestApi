using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
	/// <summary>
	/// Contains the details about a report.
	/// </summary>
	public class Report
    {
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("path")]
		public string Path { get; set; }

		[JsonPropertyName("reportTreeID")]
		public int ReportTreeId { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("activeFlag")]
		public bool IsActive { get; set; }

		[JsonPropertyName("readOnly")]
		public bool IsReadOnly { get; set; }

		[JsonPropertyName("createdDate")]
		public DateTime? CreatedDate { get; set; }

		[JsonPropertyName("dateLastModified")]
		public DateTime? DateLastModified { get; set; }

		[JsonPropertyName("userId")]
		public int UserId { get; set; }
	}
}
