using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Contains the number that a vendor uses to refer to a part that you may reference differently. A value is required in order to create a valid object.
    /// </summary>
    public class VendorPartNumber
    {
        [JsonPropertyName("number"), Required]
        public string Number { get; set; }
    }
}
