using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Contains information about a Product.
    /// </summary>
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("partID")]
        public int PartId { get; set; }

        [JsonPropertyName("part")]
        public Part Part { get; set; }

        [JsonPropertyName("num")]
        public string Number { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }

        [JsonPropertyName("upc")]
        public string UPC { get; set; }

        [JsonPropertyName("sku")]
        public string SKU { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("uom")]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        /// <summary>
        /// See DB table SOITEMTYPE for options.
        /// </summary>
        [JsonPropertyName("defaultSOItemType")]
        public string DefaultSOItemType { get; set; }

        [JsonPropertyName("displayType")]
        public string DisplayType { get; set; }

        [JsonPropertyName("url")]
        public string URL { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        ///// <summary>
        ///// See UOM table
        ///// </summary>
        //[JsonPropertyName("weightUOMID")]
        //public int WeightUnitOfMeasureId { get; set; }

        [JsonPropertyName("weightUOM")]
        public UnitOfMeasure WeightUnitOfMeasure { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("len")]
        public int Length { get; set; }

        ///// <summary>
        ///// See UOM table
        ///// </summary>
        //[JsonPropertyName("sizeUOMID")]
        //public int SizeUnitOfMeasureId { get; set; }

        [JsonPropertyName("sizeUOM")]
        public UnitOfMeasure SizeUnitOfMeasure { get; set; }

        [JsonPropertyName("accountingID")]
        public int AccountingId { get; set; }

        [JsonPropertyName("accountingHash")]
        public string AccountingHash { get; set; }

        [JsonPropertyName("sellableInOtherUOMFlag")]
        public bool IsSellableInOtherUnitOfMeasure { get; set; }

        [JsonPropertyName("activeFlag")]
        public bool IsActive { get; set; }

        [JsonPropertyName("taxableFlag")]
        public bool IsTaxable { get; set; }

        [JsonPropertyName("usePriceFlag")]
        public bool UsePrice { get; set; }

        [JsonPropertyName("kitFlag")]
        public bool IsKit { get; set; }

        [JsonPropertyName("showSOComboFlag")]
        public bool ShowSOCombo { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }
    }
}
