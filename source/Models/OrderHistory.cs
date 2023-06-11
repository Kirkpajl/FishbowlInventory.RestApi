using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// This is a storage object for Order History.
    /// </summary>
    public class OrderHistory
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("tableID")]
        public int TableId { get; set; }

        [JsonPropertyName("recordID")]
        public int RecordId { get; set; }
    }
}
