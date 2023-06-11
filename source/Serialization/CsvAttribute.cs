using System;
using System.Collections.Generic;
using System.Text;

namespace FishbowlInventory.Serialization
{
    /// <summary>
    /// Provides the base class for serialization attributes.
    /// </summary>
    public abstract class CsvAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of the FishbowlInventory.Serialization.JsonAttribute.
        /// </summary>
        protected CsvAttribute()
        {
        }
    }
}
