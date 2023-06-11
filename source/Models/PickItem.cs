using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Stores information about items that are involved in a pick.
    /// </summary>
    public class PickItem
    {
        [JsonPropertyName("pickItemID")]
        public int Id { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("part")]
        public Part Part { get; set; }

        [JsonPropertyName("tag")]
        public Tag Tag { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("uom")]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        [JsonPropertyName("tracking")]
        public TrackingItem Tracking { get; set; }

        [JsonPropertyName("destinationTag")]
        public Tag DestinationTag { get; set; }

        [JsonPropertyName("orderType")]
        public string OrderType { get; set; }

        [JsonPropertyName("orderTypeID")]
        public int OrderTypeId { get; set; }

        [JsonPropertyName("orderNum")]
        public string OrderNumber { get; set; }

        [JsonPropertyName("orderID")]
        public int OrderId { get; set; }

        [JsonPropertyName("soItemId")]
        public int SoItemId { get; set; }

        [JsonPropertyName("poItemId")]
        public int PoItemId { get; set; }

        [JsonPropertyName("xoItemId")]
        public int XoItemId { get; set; }

        [JsonPropertyName("woItemId")]
        public int WoItemId { get; set; }

        [JsonPropertyName("slotNumber")]
        public int SlotNumber { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("pickItemType")]
        public int PickItemType { get; set; }
    }
}
