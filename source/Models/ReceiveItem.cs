using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Contains receipt information.
    /// </summary>
    public class ReceiveItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("itemNum")]
        public string Number { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("lineNum")]
        public int LineNumber { get; set; }

        [JsonPropertyName("itemStatus")]
        public int Status { get; set; }

        [JsonPropertyName("itemType")]
        public int Type { get; set; }

        [JsonPropertyName("dateLastModified")]
        public DateTime? DateLastModified { get; set; }

        [JsonPropertyName("orderNum")]
        public string OrderNumber { get; set; }

        [JsonPropertyName("orderType")]
        public int OrderType { get; set; }

        [JsonPropertyName("soItemId")]
        public int SalesOrderItemId { get; set; }

        [JsonPropertyName("poItemId")]
        public int PurchaseOrderItemId { get; set; }

        [JsonPropertyName("xoItemId")]
        public int TransferOrderItemId { get; set; }

        [JsonPropertyName("orderItemType")]
        public int OrderItemType { get; set; }

        [JsonPropertyName("receiptID")]
        public int ReceiptId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("uomName")]
        public string UnitOfMeasureName { get; set; }

        [JsonPropertyName("uomID")]
        public int UnitOfMeasureId { get; set; }

        [JsonPropertyName("suggestedLocationID")]
        public int SuggestedLocationId { get; set; }

        [JsonPropertyName("originalUnitCost")]
        public int OriginalUnitCost { get; set; }

        [JsonPropertyName("billedUnitCost")]
        public int BilledUnitCost { get; set; }

        [JsonPropertyName("landedUnitCost")]
        public int LandedUnitCost { get; set; }

        [JsonPropertyName("deliverTo ")]
        public string DeliverTo { get; set; }

        [JsonPropertyName("carrierID")]
        public int CarrierId { get; set; }

        [JsonPropertyName("partTypeID")]
        public int PartTypeId { get; set; }

        [JsonPropertyName("trackingNum")]
        public string TrackingNumber { get; set; }

        [JsonPropertyName("packageCount")]
        public int PackageCount { get; set; }

        [JsonPropertyName("dateScheduled")]
        public DateTime? DateScheduled { get; set; }

        [JsonPropertyName("receivedReceipts")]
        public ReceivedReceipt[] ReceivedReceipts { get; set; }

        [JsonPropertyName("linkedOrders")]
        public string LinkedOrders { get; set; }

        [JsonPropertyName("part")]
        public Part Part { get; set; }
    }
}
