using FishbowlInventory.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    public class ManufactureOrderConfigurationItem
    {
        /// <summary>
        /// The configuration's unique identification number.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The order status.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// The description of the object.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// The quantity to be fulfilled.
        /// </summary>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// The quantity already fulfilled.
        /// </summary>
        [JsonPropertyName("quantityFulfilled")]
        public int QuantityFulfilled { get; set; }

        /// <summary>
        /// Indicates if the item is a stage.
        /// </summary>
        [JsonPropertyName("stage")]
        public bool Stage { get; set; }

        /// <summary>
        /// The amount to adjust the configuration price on the sales order.
        /// </summary>
        [JsonPropertyName("priceAdjustment")]
        public decimal PriceAdjustment { get; set; }

        /// <summary>
        /// The unit cost of the configuration.
        /// </summary>
        [JsonPropertyName("unitCost")]
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Timestamp of when the configuration was scheduled.
        /// </summary>
        [JsonPropertyName("dateScheduled")]  //'yyyy-MM-dd'
        public DateTime? DateScheduled { get; set; }

        /// <summary>
        /// Timestamp of when the configuration was scheduled.
        /// </summary>
        [JsonPropertyName("dateScheduledToStart")]  //'yyyy-MM-dd'
        public DateTime? DateScheduledToStart { get; set; }

        /// <summary>
        /// The item's note field.
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; }

        /// <summary>
        /// The position in the sort order.
        /// </summary>
        [JsonPropertyName("sortId")]
        public int SortId { get; set; }

        /// <summary>
        /// The part associated with the item.
        /// </summary>
        [JsonPropertyName("part")]
        public Part Part { get; set; }

        /// <summary>
        /// The configuration item type.
        /// </summary>
        /// <remarks>
        /// 'Finished Good' | 'Raw Good' | 'Repair Raw Good' | 'Note' | 'Bill of Materials'
        /// </remarks>
        [JsonPropertyName("type")]
        public ConfigurationItemType Type { get; set; }

        /// <summary>
        /// The configuration item's unit of measure.
        /// </summary>
        [JsonPropertyName("uom")]
        public UnitOfMeasure UOM { get; set; }

        /// <summary>
        /// Indicates if the item is a one time item.
        /// </summary>
        [JsonPropertyName("oneTimeItem")]
        public bool OneTimeItem { get; set; }

        /// <summary>
        /// An optional nested configuration object
        /// </summary>
        [JsonPropertyName("configuration")]
        public ManufactureOrderConfiguration Configuration { get; set; }
    }
}
