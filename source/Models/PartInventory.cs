using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    public class PartInventory
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("partNumber")]
        public string PartNumber { get; set; }

        /// <summary>
        /// The quantity of the part to adjust in inventory.
        /// </summary>
        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        [JsonPropertyName("partDescription")]
        public string PartDescription { get; set; }

        /// <summary>
        /// The part unit of measure.
        /// </summary>
        [JsonPropertyName("uom")]
        public UnitOfMeasure UnitOfMeasure { get; set; }
        /*
        /// <summary>
        /// The location for the inventory.
        /// </summary>
        [JsonPropertyName("location")]
        public Location Location { get; set; }

        /// <summary>
        /// The unit cost for the inventory to be added. If blank, the current part cost will be used.
        /// </summary>
        [JsonPropertyName("unitCost")]
        public double UnitCost { get; set; }

        /// <summary>
        /// A note about the inventory adjustment.
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; }

        /// <summary>
        /// The customer associated with the inventory adjustment.
        /// </summary>
        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        /// <summary>
        /// The class category for the inventory adjustment.
        /// </summary>
        [JsonPropertyName("class")]
        public Category Class { get; set; }

        /// <summary>
        /// The date for the inventory adjustment. If blank, the current date will be used.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// A list of the tracking items for the inventory. Only used if the part tracks inventory.
        /// </summary>
        [JsonPropertyName("trackingItems")]
        public TrackingItem[] TrackingItems { get; set; }
        */
    }
}
