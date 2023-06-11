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
    /// The order status.
    /// </summary>
    /// <remarks>
    /// 'All' | 'All Open' | 'Entered' | 'Issued' | 'Partial' | 'Fulfilled' | 'Closed Short' | 'Void'
    /// </remarks>
    [JsonConverter(typeof(EnumDescriptionConverter))]
    //[JsonConverter(typeof(EnumStringConverter<ManufactureOrderStatus>))]
    public enum ManufactureOrderStatus
    {
        [Description("All")] All,
        [Description("All Open")] AllOpen,
        [Description("Entered")] Entered,
        [Description("Issued")] Issued,
        [Description("Partial")] Partial,
        [Description("Fulfilled")] Fulfilled,
        [Description("Closed Short")] ClosedShort,
        [Description("Void")] Void
    }
}
