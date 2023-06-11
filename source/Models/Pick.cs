using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Stores all the information associated with a pick.
    /// </summary>
    public class Pick
    {
        [JsonPropertyName("pickID")]
        public int Id { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("typeID")]
        public int TypeId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("statusID")]
        public int StatusId { get; set; }

        [JsonPropertyName("priority")]
        public string Priority { get; set; }

        [JsonPropertyName("priorityID")]
        public int PriorityId { get; set; }

        [JsonPropertyName("locationGroupID")]
        public int LocationGroupId { get; set; }

        [JsonPropertyName("dateLastModified")]
        public DateTime? DateLastModified { get; set; }

        [JsonPropertyName("dateScheduled")]
        public DateTime? DateScheduled { get; set; }

        [JsonPropertyName("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [JsonPropertyName("dateStarted")]
        public DateTime? DateStarted { get; set; }

        [JsonPropertyName("dateFinished")]
        public DateTime? DateFinished { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("pickOrders")]
        public PickOrder[] PickOrders { get; set; }

        [JsonPropertyName("pickItems")]
        public PickItem[] PickItems { get; set; }
    }
}
