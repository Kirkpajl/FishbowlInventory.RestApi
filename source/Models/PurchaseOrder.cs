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
    /// Object used to store data concerning a Purchase Order.
    /// </summary>
    public class PurchaseOrder
    {
        /// <summary>
        /// The purchase order's unique identification number.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The purchase order number.
        /// </summary>
        [JsonPropertyName("number")]
        public string Number { get; set; }

        /// <summary>
        /// The order status.
        /// </summary>
        [JsonPropertyName("status")]
        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.BidRequest;

        /// <summary>
        /// The class category.
        /// </summary>
        [JsonPropertyName("class")]
        public Category Class { get; } = new Category();

        /// <summary>
        /// The shipping carrier.
        /// </summary>
        [JsonPropertyName("carrier")]
        public Carrier Carrier { get; } = new Carrier();

        /// <summary>
        /// The unique identifier of when ownership/liability of the order transfers to the purchaser.
        /// </summary>
        [JsonPropertyName("fobPointId")]
        public int FobPointId { get; set; }

        /// <summary>
        /// Indicates when ownership/liability of the order transfers to the purchaser.
        /// </summary>
        [JsonPropertyName("fobPointName")]
        public string FobPointName { get; set; }

        /// <summary>
        /// The terms of payment on the order.
        /// </summary>
        [JsonPropertyName("paymentTerms")]
        public PaymentTerm PaymentTerms { get; } = new PaymentTerm();

        /// <summary>
        /// The shipping terms on the order.
        /// </summary>
        [JsonPropertyName("shipTermsName")]
        public string ShippingTermsName { get; set; }

        /// <summary>
        /// Indicates whether the order is a standard or drop ship purchase order.
        /// </summary>
        [JsonPropertyName("type")]
        public PurchaseOrderType Type { get; set; } = PurchaseOrderType.Standard;

        /// <summary>
        /// The vendor on the purchase order.
        /// </summary>
        [JsonPropertyName("vendor")] 
        public Vendor Vendor { get; } = new Vendor(); 

        /// <summary>
        /// The name of the vendor's contact.
        /// </summary>
        [JsonPropertyName("vendorContact")]
        public string VendorContact { get; set; }

        /// <summary>
        /// The vendor sales order number.
        /// </summary>
        [JsonPropertyName("vendorSoNumber")]
        public string VendorSalesOrderNumber { get; set; }

        /// <summary>
        /// The customer sales order number.
        /// </summary>
        [JsonPropertyName("customerSoNumber")]
        public string CustomerSalesOrderNumber { get; set; }

        /// <summary>
        /// The Fishbowl user that created the order.
        /// </summary>
        [JsonPropertyName("buyer")] 
        public User Buyer { get; } = new User();

        /// <summary>
        /// The intended recipient of the order.
        /// </summary>
        [JsonPropertyName("deliverTo")]
        public string DeliverTo { get; set; }

        /// <summary>
        /// The revision number.
        /// </summary>
        [JsonPropertyName("revisionNumber")]
        public int RevisionNumber { get; set; }

        /// <summary>
        /// Timestamp of when the order was last modified and the user that made the modifications.
        /// </summary>
        [JsonPropertyName("lastModified")] 
        public UserModificationEvent LastModified { get; } = new UserModificationEvent();

        /// <summary>
        /// Timestamp of when the order was issued and the user that made the modifications.
        /// </summary>
        [JsonPropertyName("issuedByUser")] 
        public UserModificationEvent Issued { get; } = new UserModificationEvent();

        /// <summary>
        /// Timestamp of when the order was created and the user that made the modifications.
        /// </summary>
        [JsonPropertyName("createdByUser")] 
        public UserModificationEvent Created { get; } = new UserModificationEvent();

        /// <summary>
        /// Timestamp of when the order was confirmed by the vendor.
        /// </summary>
        [JsonPropertyName("dateConfirmed")]
        public DateTime? ConfirmedDate { get; set; }

        /// <summary>
        /// Timestamp of when the order was last revised.
        /// </summary>
        [JsonPropertyName("dateRevision")]
        public DateTime? RevisionDate { get; set; }

        /// <summary>
        /// Timestamp of when the order was scheduled.
        /// </summary>
        [JsonPropertyName("dateScheduled")]
        public DateTime? FulfillmentDate { get; set; }

        /// <summary>
        /// Timestamp of when the order was completed.
        /// </summary>
        [JsonPropertyName("dateCompleted")]
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// The tax rate on the order. This object is ignored for companies in the United States.
        /// </summary>
        [JsonPropertyName("taxRate")] 
        public TaxRate TaxRate { get; } = new TaxRate();

        /// <summary>
        /// The total amount of tax on the order.
        /// </summary>
        [JsonPropertyName("totalTax")]
        public decimal TotalTax { get; set; }

        /// <summary>
        /// Indicates if the order total includes tax.
        /// </summary>
        [JsonPropertyName("totalIncludesTax")]
        public bool TotalIncludesTax { get; set; }

        /// <summary>
        /// The location group the order belongs to.
        /// </summary>
        [JsonPropertyName("locationGroup")] 
        public LocationGroup LocationGroup { get; } = new LocationGroup();

        /// <summary>
        /// The order's note field.
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; }

        /// <summary>
        /// The url link on the order.
        /// </summary>
        [JsonPropertyName("url ")]
        public string Url { get; set; }


        /// <summary>
        /// The currency used on the order.
        /// </summary>
        [JsonPropertyName("currency")] 
        public Currency Currency { get; } = new Currency();

        /// <summary>
        /// The vendor's email address for the order.
        /// </summary>
        [JsonPropertyName("email")]
        public string VendorEmail { get; set; }

        /// <summary>
        /// The vendor's phone number for the order.
        /// </summary>
        [JsonPropertyName("phone")]
        public string VendorPhone { get; set; }



        /// <summary>
        /// The shipping address on the order.
        /// </summary>
        [JsonPropertyName("shipToAddress")] 
        public Address ShipToAddress { get; } = new Address();
        
        /// <summary>
        /// The remit to address on the order.
        /// </summary>
        [JsonPropertyName("remitToAddress")] 
        public Address RemitToAddress { get; } = new Address();
        
        

        [JsonPropertyName("carrierServiceId")]
        public int? CarrierServiceId { get; set; }

        [JsonPropertyName("carrierServiceName")]
        public string CarrierServiceName { get; set; }



        /// <summary>
        /// A list of the purchase order items.
        /// </summary>
        [JsonPropertyName("poItems")]
        public List<PurchaseOrderItem> Items { get; } = new List<PurchaseOrderItem>();

        /// <summary>
        /// A list of custom fields associated with the order.
        /// </summary>
        [JsonPropertyName("customFields")]
        public List<CustomField> CustomFields { get; } = new List<CustomField>();
    }
}
