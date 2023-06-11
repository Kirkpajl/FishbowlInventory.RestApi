using System.Text.Json.Serialization;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// Contains information relevant to one of the financial accounts in the Fishbowl database.
    /// </summary>
    public class Account
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("accountingId")]
        public string AccountingId { get; set; }

        [JsonPropertyName("accountType")]
        public int AccountType { get; set; }

        [JsonPropertyName("balance")]
        public string Balance { get; set; }
    }
}
