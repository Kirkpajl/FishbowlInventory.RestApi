using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Contains Contact Information.
    /// </summary>
    public class Contact
    {
        [JsonPropertyName("contacts")]
        public ContactInformation[] Contacts { get; set; }
    }
}
