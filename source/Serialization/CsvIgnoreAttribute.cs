using FishbowlInventory.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using FishbowlInventory.Serialization;

namespace FishbowlInventory.Serialization
{
    /// <summary>
    /// Prevents a property or field from being serialized or deserialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CsvIgnoreAttribute : CsvAttribute
    {
        /// <summary>
        /// Specifies the condition that must be met before a property or field will be ignored.
        /// </summary>
        /// <remarks>The default value is <see cref="CsvIgnoreCondition.Always"/>.</remarks>
        public CsvIgnoreCondition Condition { get; set; } = CsvIgnoreCondition.Always;

        /// <summary>
        /// Initializes a new instance of <see cref="CsvIgnoreAttribute"/>.
        /// </summary>
        public CsvIgnoreAttribute() { }
    }
}
