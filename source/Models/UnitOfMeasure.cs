using FishbowlInventory.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Fishbowl unit of measure (UOM).
    /// </summary>
    public class UnitOfMeasure
    {
        /// <summary>
        /// The UOM's unique identification number.
        /// </summary>
        [JsonPropertyName("id")]  //UOMID
        public int Id { get; set; }

        /// <summary>
        /// The UOM name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The UOM abbreviation.
        /// </summary>
        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; }

        /// <summary>
        /// The UOM description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// The basic type of the UOM.
        /// </summary>
        [JsonPropertyName("type")]
        public UnitOfMeasureType Type { get; set; }

        /// <summary>
        /// The active status of the UOM.
        /// </summary>
        [JsonPropertyName("active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Indicates if the quantity must be a whole number.
        /// </summary>
        [JsonPropertyName("integral")]
        public bool IsIntegral { get; set; }

        /// <summary>
        /// Indicates if the UOM is read-only.
        /// </summary>
        [JsonPropertyName("readOnly")]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// A list of the UOM conversions.
        /// </summary>
        [JsonPropertyName("conversions")]
        public UnitOfMeasureConversion[] Conversions { get; set; }
    }
}
