using System;
using System.Collections.Generic;
using System.Text;
using FishbowlInventory.Serialization;

namespace FishbowlInventory.Serialization
{
    /// <summary>
    /// Specifies the property name that is present in the JSON when serializing and deserializing.
    /// This overrides any naming policy specified by <see cref="JsonNamingPolicy"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CsvPropertyNameAttribute : CsvAttribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CsvPropertyNameAttribute"/> with the specified property name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public CsvPropertyNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CsvPropertyNameAttribute"/> with the specified property name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public CsvPropertyNameAttribute(string name, int order)
        {
            Name = name;
            Order = order;
        }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The sort order of the property.
        /// </summary>
        public int? Order { get; }
    }
}
