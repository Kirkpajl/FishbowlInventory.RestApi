using System;
using System.Collections.Generic;
using System.Text;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// Controls how the FishbowlInventory.CsvIgnoreAttribute ignores properties on serialization and deserialization.
    /// </summary>
    public enum CsvIgnoreCondition
    {
        /// <summary>
        /// Property will always be serialized and deserialized, regardless of System.Text.Json.JsonSerializerOptions.IgnoreNullValues configuration.
        /// </summary>
        Never,

        /// <summary>
        /// Property will always be ignored.
        /// </summary>
        Always,

        /// <summary>
        /// Property will only be ignored if it is null.
        /// </summary>
        WhenWritingDefault,
        
        /// <summary>
        /// If the value is null, the property is ignored during serialization. This is applied only to reference-type properties and fields.
        /// </summary>
        WhenWritingNull
    }
}
