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
    /// Indicates whether the order is a standard or drop ship purchase order
    /// </summary>
    /// <remarks> 'Standard' | 'Drop Ship'</remarks>
    [JsonConverter(typeof(EnumDescriptionConverter))]  // [JsonConverter(typeof(EnumStringConverter<PurchaseOrderType>))]
    public enum PurchaseOrderType
    {
        [Description("Standard")] Standard,
        [Description("Drop Ship")] DropShip
    }
}
