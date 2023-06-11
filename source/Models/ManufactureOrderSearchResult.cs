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
    /// Manufacture orders are used to organize work orders and allows for items to be manufactured, disassembled, and repaired.
    /// </summary>
    public class ManufactureOrderSearchResult
    {
        /// <summary>
        /// The manufacture order's unique identification number.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The manufacture order number.
        /// </summary>
        [JsonPropertyName("number")]
        public string Number { get; set; }

        /// <summary>
        /// The number of the associated Bill of Material.
        /// </summary>
        [JsonPropertyName("bomNumber")]
        public string BillOfMaterialNumber { get; set; }

        /// <summary>
        /// The description of the associated Bill of Material.
        /// </summary>
        [JsonPropertyName("bomDescription")]
        public string BillOfMaterialDescription { get; set; }

        /// <summary>
        /// The associated sales order number.
        /// </summary>
        [JsonPropertyName("soNumber")]
        public string SalesOrderNumber { get; set; }

        /// <summary>
        /// Timestamp of when the order was scheduled.
        /// </summary>
        [JsonPropertyName("dateScheduled")]  //'yyyy-MM-dd'
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? DateScheduled { get; set; }

        /// <summary>
        /// The order status.
        /// </summary>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(EnumDescriptionConverter))]
        public ManufactureOrderStatus Status { get; set; }

        /// <summary>
        /// The location group the order belongs to.
        /// </summary>
        [JsonPropertyName("locationGroup")]
        public string LocationGroupName { get; set; }
    }
}
