using FishbowlInventory.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// The basic type of the part
    /// </summary>
    /// <remarks> 'Inventory' | 'Service' | 'Labor' | 'Overhead' | 'Non-Inventory' | 'Internal Use' | 'Capital Equipment' | 'Shipping'</remarks>
    [JsonConverter(typeof(EnumDescriptionConverter))]  //[JsonConverter(typeof(EnumStringConverter<PartType>))]
    public enum PartType
    {
        [Description("Inventory")] Inventory,
        [Description("Service")] Service,
        [Description("Labor")] Labor,
        [Description("Overhead")] Overhead,
        [Description("Non-Inventory")] NonInventory,
        [Description("Internal Use")] InternalUse,
        [Description("Capital Equipment")] CapitalEquipment,
        [Description("Shipping")] Shipping
    }
}
