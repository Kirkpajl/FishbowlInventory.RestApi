﻿using FishbowlInventory.Serialization;
using System;
using System.Text.Json.Serialization;

namespace FishbowlInventory.Attributes
{
    /// <summary>
    /// When placed on a property, field, or type, specifies the converter type to use.
    /// </summary>
    /// <remarks>
    /// The specified converter type must derive from <see cref="JsonConverter"/>.
    /// When placed on a property or field, the specified converter will always be used.
    /// When placed on a type, the specified converter will be used unless a compatible converter is added to
    /// <see cref="JsonSerializerOptions.Converters"/> or there is another <see cref="CsvConverterAttribute"/> on a property or field
    /// of the same type.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CsvConverterAttribute : CsvAttribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="JsonConverterAttribute"/> with the specified converter type.
        /// </summary>
        /// <param name="converterType">The type of the converter.</param>
        public CsvConverterAttribute(Type converterType)
        {
            ConverterType = converterType;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JsonConverterAttribute"/>.
        /// </summary>
        protected CsvConverterAttribute() { }

        /// <summary>
        /// The type of the converter to create, or null if <see cref="CreateConverter(Type)"/> should be used to obtain the converter.
        /// </summary>
        public Type ConverterType { get; private set; }

        /*
        /// <summary>
        /// If overridden and <see cref="ConverterType"/> is null, allows a custom attribute to create the converter in order to pass additional state.
        /// </summary>
        /// <returns>
        /// The custom converter.
        /// </returns>
        public virtual JsonConverter? CreateConverter(Type typeToConvert)
        {
            return null;
        }
        */
    }
}
