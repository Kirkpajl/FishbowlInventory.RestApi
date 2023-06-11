using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Stores the details about a Customer. You must include the JobDepth field. There are no other 
    /// fields that are essential for the use of a Customer object. Use only those fields that you need.
    /// </summary>
    public class Customer
    {
        [JsonPropertyName("customerID")]
        public int CustomerId { get; set; }

        [JsonPropertyName("accountID")]
        public int AccountId { get; set; }

        [JsonPropertyName("defPaymentTerms")]
        public string DefaultPaymentTerms { get; set; }

        [JsonPropertyName("defShipTerms")]
        public string DefaultShipTerms { get; set; }

        [JsonPropertyName("taxRate")]
        public string TaxRate { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("dateLastModified")]
        public DateTime DateLastModified { get; set; }

        [JsonPropertyName("lastChangedUser")]
        public string LastChangedUser { get; set; }

        [JsonPropertyName("creditLimit")]
        public decimal CreditLimit { get; set; }

        [JsonPropertyName("taxExempt")]
        public bool IsTaxExempt { get; set; }

        [JsonPropertyName("taxExemptNumber")]
        public string TaxExemptNumber { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("activeFlag")]
        public bool IsActive { get; set; }

        [JsonPropertyName("accountingID")]
        public string AccountingId { get; set; }

        [JsonPropertyName("currencyName")]
        public string CurrencyName { get; set; }

        [JsonPropertyName("currencyRate")]
        public double CurrencyRate { get; set; }

        [JsonPropertyName("defaultSalesman")]
        public string DefaultSalesman { get; set; }

        [JsonPropertyName("defaultCarrier")]
        public string DefaultCarrier { get; set; }

        [JsonPropertyName("defaultShipService")]
        public string DefaultShipService { get; set; }

        [JsonPropertyName("jobDepth"), Required]
        public int JobDepth { get; set; }

        [JsonPropertyName("quickBooksClassName")]
        public string QuickBooksClassName { get; set; }

        [JsonPropertyName("parentID")]
        public int ParentID { get; set; }

        [JsonPropertyName("pipelineAccount")]
        public int PipelineAccount { get; set; }

        [JsonPropertyName("URL")]
        public string URL { get; set; }

        [JsonPropertyName("addresses")]
        public Address[] Addresses { get; set; }

        [JsonPropertyName("customFields")]
        public CustomField[] CustomFields { get; set; }
    }
}
