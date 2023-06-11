using FishbowlInventory.Converters;
using FishbowlInventory.Enumerations;
using FishbowlInventory.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
	/// <summary>
	/// Contains all the vendor information and details.
	/// </summary>
	public class Vendor
	{
		/// <summary>
		/// The unique identifier.
		/// </summary>
		[JsonPropertyName("id")]
		public int Id { get; set; }

		/// <summary>
		/// The vendor name.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>
		/// The vendor account number.
		/// </summary>
		[JsonPropertyName("accountNumber")]
		public string AccountNumber { get; set; }

        /// <summary>
        /// The vendor address city.
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// The vendor address state.
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }

        /// <summary>
        /// The vendor address country.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// Indicates if the vendor is active.
        /// </summary>
        [JsonPropertyName("active")]
		public bool IsActive { get; set; }

		/// <summary>
		/// A list of the vendor's custom fields.
		/// </summary>
		[JsonPropertyName("customFields")]
		public List<CustomField> CustomFields { get; set; }



        [JsonConverter(typeof(EnumDisplayConverter))]
        [JsonPropertyName("status")]
        public VendorStatus Status { get; set; } = VendorStatus.Normal;  //string

        [JsonPropertyName("defaultPaymentTerms")]
        public string DefaultPaymentTerms { get; set; }

        [JsonPropertyName("defaultShippingTerms")]
        public string DefaultShippingTerms { get; set; }

        [JsonPropertyName("taxRate")]
        public string TaxRate { get; set; }

        [JsonPropertyName("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [JsonPropertyName("dateModified")]
        public DateTime? DateModified { get; set; }

        [JsonPropertyName("lastChangedUser")]
        public string LastChangedUser { get; set; }

        [JsonPropertyName("creditLimit")]
        public decimal CreditLimit { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("sysUserId")]
        public int SystemUserId { get; set; }

        [JsonPropertyName("accountingId")]
        public int AccountingId { get; set; }

        [JsonPropertyName("accountingHash")]
        public string AccountingHash { get; set; }

        [JsonPropertyName("currencyName")]
        public string CurrencyName { get; set; }

        [JsonPropertyName("currencyRate")]
        public decimal CurrencyRate { get; set; }

        [JsonPropertyName("leadTime")]
        public int LeadTime { get; set; }



        [JsonPropertyName("defCarrier")]
        public string DefaultCarrier { get; set; }

        [JsonPropertyName("defCarrierService")]
        public string DefaultCarrierService { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("minOrderAmount")]
        public decimal MinimumOrderAmount { get; set; }



        //[JsonPropertyName("addresses")]
        public ICollection<Address> Addresses { get; } = new HashSet<Address>();
    }
}
