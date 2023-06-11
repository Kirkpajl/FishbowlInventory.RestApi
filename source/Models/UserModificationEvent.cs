using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    public class UserModificationEvent
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("dateLastModified")]
        public DateTime DateLastModified { get; set; }
    }
}
