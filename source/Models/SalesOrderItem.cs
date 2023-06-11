using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    public class SalesOrderItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("productNumber")]
        public string ProductNumber { get; set; }

        [JsonPropertyName("soID")]
        public int SalesOrderId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("customerPartNum")]
        public string CustomerPartNumber { get; set; }

        //[JsonPropertyName("taxable")]
        //public bool IsTaxable { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("productPrice")]
        public int ProductPrice { get; set; }

        [JsonPropertyName("totalPrice")]
        public int TotalPrice { get; set; }

        [JsonPropertyName("uomCode")]
        public string UOMCode { get; set; }

        [JsonPropertyName("itemType")]
        public int ItemType { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("quickBooksClassName")]
        public string QuickBooksClassName { get; set; }

        [JsonPropertyName("lineNumber")]
        public int LineNumber { get; set; }

        [JsonPropertyName("kitItemFlag")]
        public bool IsKitItem { get; set; }

        [JsonPropertyName("showItemFlag")]
        public bool ShowItem { get; set; }

        [JsonPropertyName("adjustmentAmount")]
        public double AdjustmentAmount { get; set; }

        [JsonPropertyName("adjustPercentage")]
        public int AdjustPercentage { get; set; }

        [JsonPropertyName("dateLastFulfillment")]
        public DateTime? DateLastFulfillment { get; set; }

        [JsonPropertyName("dateLastModified")]
        public DateTime? DateLastModified { get; set; }

        [JsonPropertyName("dateScheduledFulfillment")]
        public DateTime? DateScheduledFulfillment { get; set; }

        [JsonPropertyName("exchangeSOLineItem")]
        public int ExchangeSalesOrderLineItem { get; set; }

        [JsonPropertyName("itemAdjustID")]
        public int ItemAdjustId { get; set; }

        [JsonPropertyName("qtyFulfilled")]
        public int QuantityFulfilled { get; set; }

        [JsonPropertyName("qtyPicked")]
        public int QuantityPicked { get; set; }

        [JsonPropertyName("revisionLevel")]
        public int RevisionLevel { get; set; }

        [JsonPropertyName("totalCost")]
        public double TotalCost { get; set; }

        [JsonPropertyName("taxableFlag")]
        public bool IsTaxable { get; set; }
    }
}
