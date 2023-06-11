using FishbowlInventory.Converters;
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
    /// Stores information about a part.
    /// </summary>
    public class Part
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The part number.
        /// </summary>
        [JsonPropertyName("number")]
        public string Number { get; set; }

        /// <summary>
        /// The part description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// The UPC code for the part.
        /// </summary>
        [JsonPropertyName("upc")]
        public string UPC { get; set; }

        /// <summary>
        /// The basic type of the part.
        /// </summary>
        [JsonConverter(typeof(EnumDescriptionConverter))]
        [JsonPropertyName("type")]
        public PartType Type { get; set; }

        /// <summary>
        /// The ABC code for the part.
        /// </summary>
        [JsonPropertyName("abc")]
        public string ABCCode { get; set; }

        /// <summary>
        /// The part details.
        /// </summary>
        [JsonPropertyName("details")]
        public string Details { get; set; }

        /// <summary>
        /// Indicates if the part has an associated bill of materials. True returns parts with an associated bill of materials, false does not filter the results.
        /// </summary>
        [JsonPropertyName("hasBom")]
        public bool HasBillOfMaterial { get; set; }

        /// <summary>
        /// The active status of the UOM.
        /// </summary>
        [JsonPropertyName("active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// The associated product number.
        /// </summary>
        [JsonPropertyName("productNumber")]
        public string ProductNumber { get; set; }

        /// <summary>
        /// The associated product description.
        /// </summary>
        [JsonPropertyName("productDescription")]
        public string ProductDescription { get; set; }

        /// <summary>
        /// The Vendor Part Number
        /// </summary>
        [JsonPropertyName("vendorPartNumber")]
        public string VendorPartNumber { get; set; }

        /// <summary>
        /// The name of the associated vendor.
        /// </summary>
        [JsonPropertyName("vendorName")]
        public string VendorName { get; set; }



        [JsonPropertyName("uom")]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        [JsonPropertyName("weightUOM")]
        public UnitOfMeasure WeightUnitOfMeasure { get; set; }

        [JsonPropertyName("width")]
        public double Width { get; set; }

        [JsonPropertyName("height")]
        public double Height { get; set; }

        [JsonPropertyName("length")]
        public double Length { get; set; }

        [JsonPropertyName("sizeUOM")]
        public UnitOfMeasure SizeUnitOfMeasure { get; set; }

        [JsonPropertyName("alertNote")]
        public string Note { get; set; }

        [JsonPropertyName("standardCost")]
        public decimal StandardCost { get; set; }

        [JsonPropertyName("averageCost")]
        public decimal? AverageCost { get; set; }

        [JsonPropertyName("hasTracking")]
        public bool HasTracking { get; set; }

        [JsonPropertyName("isSerialized")]
        public bool IsSerialized { get; set; }





        //[JsonPropertyName("manufacturer")]
        //public string Manufacturer { get; set; }

        //[JsonPropertyName("tagLabel")]
        //public string TagLabel { get; set; }

        //[JsonPropertyName("tracksSerialNumbers")]
        //public bool TracksSerialNumbers { get; set; }

        //[JsonPropertyName("configurable")]
        //public bool IsConfigurable { get; set; }

        //[JsonPropertyName("usedFlag")]
        //public bool IsUsed { get; set; }

        //[JsonPropertyName("partTrackingList")]
        //public PartTrackingItem[] PartTrackingItems { get; set; }

        //[JsonPropertyName("image")]
        //public string Image { get; set; }
    }
}
