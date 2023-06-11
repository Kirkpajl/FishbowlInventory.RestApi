using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
	/// <summary>
	/// Stores sales order information.
	/// </summary>
	public class SalesOrder
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("note")]
		public string Note { get; set; }

		[JsonPropertyName("totalPrice")]
		public double TotalPrice { get; set; }

		[JsonPropertyName("totalTax")]
		public double TotalTax { get; set; }

		[JsonPropertyName("paymentTotal")]
		public double PaymentTotal { get; set; }

		[JsonPropertyName("itemTotal")]
		public double ItemTotal { get; set; }

		[JsonPropertyName("salesman")]
		public string Salesman { get; set; }

		[JsonPropertyName("number")]
		public string Number { get; set; }

		[JsonPropertyName("status")]
		public int Status { get; set; }

		[JsonPropertyName("carrier")]
		public string Carrier { get; set; }

		[JsonPropertyName("firstShipDate")]
		public DateTime? FirstShipDate { get; set; }

		[JsonPropertyName("createdDate")]
		public DateTime? CreatedDate { get; set; }

		[JsonPropertyName("issuedDate")]
		public DateTime? IssuedDate { get; set; }

		[JsonPropertyName("taxRatePercentage")]
		public double TaxRatePercentage { get; set; }

		[JsonPropertyName("taxRateName")]
		public string TaxRateName { get; set; }

		[JsonPropertyName("shippingCost")]
		public double ShippingCost { get; set; }

		[JsonPropertyName("shippingTerms")]
		public string ShippingTerms { get; set; }

		[JsonPropertyName("paymentTerms")]
		public string PaymentTerms { get; set; }

		[JsonPropertyName("customerContact")]
		public string CustomerContact { get; set; }

		[JsonPropertyName("customerName")]
		public string CustomerName { get; set; }

		[JsonPropertyName("customerID")]
		public int CustomerId { get; set; }

		[JsonPropertyName("fob")]
		public string FOB { get; set; }

		[JsonPropertyName("quickBooksClassName")]
		public string QuickBooksClassName { get; set; }

		[JsonPropertyName("locationGroup")]
		public string LocationGroup { get; set; }

		[JsonPropertyName("priorityId")]
		public int PriorityId { get; set; }

		[JsonPropertyName("currencyRate")]
		public double CurrencyRate { get; set; }

		[JsonPropertyName("currencyName")]
		public string CurrencyName { get; set; }

		[JsonPropertyName("priceIsInHomeCurrency")]
		public bool IsPriceInHomeCurrency { get; set; }

		[JsonPropertyName("billTo")]
		public SalesOrderContact BillTo { get; set; }

		[JsonPropertyName("ship")]
		public SalesOrderContact ShipTo { get; set; }

		[JsonPropertyName("issueFlag")]
		public bool IsIssued { get; set; }

		[JsonPropertyName("vendorPO")]
		public string VendorPurchaseOrder { get; set; }

		[JsonPropertyName("customerPO")]
		public string CustomerPurchaseOrder { get; set; }

		[JsonPropertyName("upsServiceID")]
		public int UPSServiceId { get; set; }

		[JsonPropertyName("totalIncludesTax")]
		public bool TotalIncludesTax { get; set; }

		[JsonPropertyName("typeID")]
		public int TypeId { get; set; }

		[JsonPropertyName("url")]
		public string URL { get; set; }

		[JsonPropertyName("cost")]
		public double Cost { get; set; }

		[JsonPropertyName("dateCompleted")]
		public DateTime? DateCompleted { get; set; }

		[JsonPropertyName("dateLastModified")]
		public DateTime? DateLastModified { get; set; }

		[JsonPropertyName("dateRevision")]
		public DateTime? DateRevision { get; set; }

		[JsonPropertyName("registerID")]
		public int RegisterId { get; set; }

		[JsonPropertyName("residentialFlag")]
		public bool IsResidential { get; set; }

		[JsonPropertyName("salesmanInitials")]
		public string SalesmanInitials { get; set; }

		[JsonPropertyName("customFields")]
		public CustomField[] CustomFields { get; set; }

		[JsonPropertyName("memos")]
		public Memo[] Memos { get; set; }

		[JsonPropertyName("items")]
		public SalesOrderItem[] Items { get; set; }
	}
}
