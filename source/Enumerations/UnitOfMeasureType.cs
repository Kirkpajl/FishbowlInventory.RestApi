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
    /// The basic type of the UOM.
    /// </summary>
    /// <remarks>'Area' | 'Count' | 'Length' | 'Time' | 'Volume' | 'Weight'</remarks>
    [JsonConverter(typeof(EnumDescriptionConverter))]  //[JsonConverter(typeof(EnumStringConverter<UnitOfMeasureType>))]
    public enum UnitOfMeasureType
    {
        [Description("Area")] Area,
        [Description("Count")] Count,
        [Description("Length")] Length,
        [Description("Time")] Time,
        [Description("Volume")] Volume,
        [Description("Weight")] Weight
    }
}
