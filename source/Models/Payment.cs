using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Contains payment information.
    /// </summary>
    public class Payment
    {
        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("salesOrderNumber")]
        public string SalesOrderNumber { get; set; }

        [JsonPropertyName("currencyRate")]
        public double CurrencyRate { get; set; }

        [JsonPropertyName("paymentDate")]
        public DateTime? PaymentDate { get; set; }

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("confirmation")]
        public string Confirmation { get; set; }

        [JsonPropertyName("expirationDate")]
        public DateTime? ExpirationDate { get; set; }

        [JsonPropertyName("depositAccountName")]
        public string DepositAccountName { get; set; }

        [JsonPropertyName("transactionID")]
        public string TransactionId { get; set; }

        [JsonPropertyName("authorizationCode")]
        public string AuthorizationCode { get; set; }

        [JsonPropertyName("merchantAccount")]
        public string MerchantAccount { get; set; }

        [JsonPropertyName("miscCreditCard")]
        public string MiscellanousCreditCard { get; set; }

        [JsonPropertyName("creditCard")]
        public CreditCard CreditCard { get; set; }
    }
}
