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
    /// The type of a BOM item contained in the manufacture order
    /// </summary>
    /// <remarks>'All' | 'Finished Good' | 'Raw Good' | 'Repair' | 'Note' | 'Bill of Materials'</remarks>
    [JsonConverter(typeof(EnumDescriptionConverter))]  //[JsonConverter(typeof(EnumStringConverter<BillOfMaterialItemType>))]
    public enum BillOfMaterialItemType
    {
        [Description("All")] All,
        [Description("Finished Good")] FinishedGood,
        [Description("Raw Good")] RawGood,
        [Description("Repair")] Repair,
        [Description("Note")] Note,
        [Description("Bill of Materials")] BillOfMaterials
    }
}
