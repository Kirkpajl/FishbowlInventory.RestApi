using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    public class PickOrder
    {
        [JsonPropertyName("orderType")]
        public string Type { get; set; }

        [JsonPropertyName("orderTypeID")]
        public int TypeId { get; set; }

        [JsonPropertyName("orderNum")]
        public string Number { get; set; }

        [JsonPropertyName("orderID")]
        public int Id { get; set; }

        [JsonPropertyName("orderTo")]
        public string To { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }
    }
}
