using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// These endpoints are used to modify Fishbowl plugin information. In order to access the plugin's information, you will need authorization from the plugin.
    /// </summary>
    public class PluginInformation
    {
        /// <summary>
        /// The plugin information's unique identification number.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The name of the plugin.
        /// </summary>
        [JsonPropertyName("plugin")]
        public string Name { get; set; }

        /// <summary>
        /// The name of the database table.
        /// </summary>
        [JsonPropertyName("table")]
        public string Table { get; set; }

        /// <summary>
        /// The unique identifier for a record in Fishbowl associated with the table.
        /// </summary>
        [JsonPropertyName("recordId")]
        public int RecordId { get; set; }

        /// <summary>
        /// The user specified identifier used to separate types of plugins you are using.
        /// </summary>
        [JsonPropertyName("groupId")]
        public int GroupId { get; set; }

        /// <summary>
        /// The unique identifier for the object being linked externally.
        /// </summary>
        [JsonPropertyName("channelId")]
        public string ChannelId { get; set; }

        /// <summary>
        /// Any additional information.
        /// </summary>
        [JsonPropertyName("info")]
        public dynamic Info { get; set; }
    }
}
