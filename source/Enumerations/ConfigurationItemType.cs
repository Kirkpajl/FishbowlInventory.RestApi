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
    /// The configuration item type.
    /// </summary>
    /// <remarks>
    /// 'Finished Good' | 'Raw Good' | 'Repair Raw Good' | 'Note' | 'Bill of Materials'
    /// </remarks>
    [JsonConverter(typeof(EnumDescriptionConverter))]  //[JsonConverter(typeof(EnumStringConverter<ConfigurationItemType>))]
    public enum ConfigurationItemType
    {
        [Description("Finished Good")] FinishedGood,
        [Description("Raw Good")] RawGood,
        [Description("Repair Raw Good")] RepairRawGood,
        [Description("Note")] Note,
        [Description("Bill of Materials")] BillOfMaterials
    }
}
