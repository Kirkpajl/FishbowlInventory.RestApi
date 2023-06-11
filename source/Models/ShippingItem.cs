using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    public class ShippingItem
    {
        [JsonPropertyName("shipItemId")]
        public int Id { get; set; }

        [JsonPropertyName("productNumber")]
        public string ProductNumber { get; set; }

        [JsonPropertyName("productDescription")]
        public string ProductDescription { get; set; }

        [JsonPropertyName("qtyShipped")]
        public int QuantityShipped { get; set; }

        [JsonPropertyName("uom")]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        [JsonPropertyName("cost")]
        public int Cost { get; set; }

        [JsonPropertyName("sku")]
        public string SKU { get; set; }

        [JsonPropertyName("upc")]
        public string UPC { get; set; }

        [JsonPropertyName("orderItemId")]
        public int OrderItemId { get; set; }

        [JsonPropertyName("orderLineItem")]
        public int OrderLineItem { get; set; }

        [JsonPropertyName("cartonName")]
        public string Name { get; set; }

        [JsonPropertyName("tagNum")]
        public int TagNumber { get; set; }

        [JsonPropertyName("weight")]
        public decimal Weight { get; set; }

        [JsonPropertyName("weightUOM")]
        public UnitOfMeasure WeightUnitOfMeasure { get; set; }

        [JsonPropertyName("displayWeight")]
        public decimal DisplayWeight { get; set; }

        [JsonPropertyName("displayWeightUOM")]
        public UnitOfMeasure DisplayWeightUnitOfMeasure { get; set; }

        [JsonPropertyName("tracking")]
        public string Tracking { get; set; }
    }
}
