using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Contains the details about a report tree.
    /// </summary>
    public class ReportTree
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("parentID")]
        public int ParentId { get; set; }

        [JsonPropertyName("readOnly")]
        public bool IsReadOnly { get; set; }

        [JsonPropertyName("userID")]
        public int UserId { get; set; }
    }
}
