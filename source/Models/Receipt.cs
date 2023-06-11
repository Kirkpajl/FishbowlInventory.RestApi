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
    public class Receipt
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("statusID")]
        public int StatusId { get; set; }

        [JsonPropertyName("typeID")]
        public int TypeId { get; set; }

        [JsonPropertyName("orderTypeID")]
        public int OrderTypeId { get; set; }

        [JsonPropertyName("soID")]
        public int SalesOrderId { get; set; }

        [JsonPropertyName("poID")]
        public int PurchaseOrderId { get; set; }

        [JsonPropertyName("xoID")]
        public int TransferOrderId { get; set; }

        [JsonPropertyName("userID")]
        public int UserId { get; set; }

        [JsonPropertyName("locationGroupID")]
        public int LocationGroupId { get; set; }

        [JsonPropertyName("receiptItems")]
        public ReceiveItem[] ReceiptItems { get; set; }
    }
}
