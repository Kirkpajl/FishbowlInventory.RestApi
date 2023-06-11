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
    public class PurchaseOrderItem
    {
        /// <summary>
        /// The unique identifier for the order item.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The part on the order item.
        /// </summary>
        [JsonPropertyName("part")]
        public Part Part { get; } = new Part();

        /// <summary>
        /// The outsourced part on the order item.
        /// </summary>
        [JsonPropertyName("outsourcedPart")]
        public Part OutsourcedPart { get; } = new Part();

        /// <summary>
        /// The order item's type.
        /// </summary>
        [JsonPropertyName("type")]
        public OrderItemType Type { get; } = new OrderItemType();

        /// <summary>
        /// The status of the order item.
        /// </summary>
        [JsonPropertyName("status")]
        public PurchaseOrderItemStatus Status { get; set; } = PurchaseOrderItemStatus.Entered;

        /// <summary>
        /// The part unit of measure.
        /// </summary>
        [JsonPropertyName("uom")]
        public UnitOfMeasure UnitOfMeasure { get; } = new UnitOfMeasure();

        /// <summary>
        /// The line number for the order item.
        /// </summary>
        [JsonPropertyName("lineNumber")]
        public int LineNumber { get; set; }

        /// <summary>
        /// The vendor's number for the part.
        /// </summary>
        [JsonPropertyName("vendorPartNumber")]
        public string VendorPartNumber { get; set; }

        /// <summary>
        /// The quantity to be fulfilled.
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// The quantity already fulfilled.
        /// </summary>
        [JsonPropertyName("quantityFulfilled")]
        public decimal QuantityFulfilled { get; set; }

        /// <summary>
        /// The quantity already picked.
        /// </summary>
        [JsonPropertyName("quantityPicked")]
        public decimal QuantityPicked { get; set; }

        /// <summary>
        /// The unit cost of the part.
        /// </summary>
        [JsonPropertyName("unitCost")]
        public decimal UnitCost { get; set; }

        /// <summary>
        /// The total tax of the part.
        /// </summary>
        [JsonPropertyName("totalTax")]
        public decimal TotalTax { get; set; }

        /// <summary>
        /// The total cost of the part.
        /// </summary>
        [JsonPropertyName("totalCost")]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// The multi-currency total tax of the part.
        /// </summary>
        [JsonPropertyName("mcTotalTax")]
        public decimal McTotalTax { get; set; }

        /// <summary>
        /// The multi-currency total cost of the part.
        /// </summary>
        [JsonPropertyName("mcTotalCost")]
        public decimal McTotalCost { get; set; }

        /// <summary>
        /// The tax rate for the order item. This object is ignored for companies in the United States.
        /// </summary>
        [JsonPropertyName("taxRate")]
        public TaxRate TaxRate { get; } = new TaxRate();

        /// <summary>
        /// Timestamp of when the order is scheduled to be fulfilled.
        /// </summary>
        [JsonPropertyName("dateScheduled")]
        public DateTime? DateScheduled { get; set; }

        /// <summary>
        /// Timestamp of the last fulfilled Date for this PO Item in the default short date format for your region.
        /// </summary>
        [JsonPropertyName("dateLastFulfilled")]
        public DateTime? DateLastFulfilled { get; set; }

        /// <summary>
        /// The revision number for the order item.
        /// </summary>
        [JsonPropertyName("revision")]
        public string Revision { get; set; }

        /// <summary>
        /// The class category for the order item.
        /// </summary>
        public Category Class { get; } = new Category();

        /// <summary>
        /// The note on the order item
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; }

        /// <summary>
        /// The customer on the order item.
        /// </summary>
        [JsonPropertyName("customer")]
        public Customer Customer { get; } = new Customer();

        /// <summary>
        /// A list of custom fields associated with the line item.
        /// </summary>
        [JsonPropertyName("customFields")]
        public List<CustomField> CustomFields { get; } = new List<CustomField>();
    }
}
