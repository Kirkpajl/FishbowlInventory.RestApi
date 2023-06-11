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
    public class ManufactureOrder
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
        /// The order status.
        /// </summary>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(EnumDescriptionConverter))]
        public ManufactureOrderStatus Status { get; set; }

        /// <summary>
        /// The revision number.
        /// </summary>
        [JsonPropertyName("revisionNumber")]
        public int RevisionNumber { get; set; }

        /// <summary>
        /// The order's note field.
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; }

        /// <summary>
        /// The location group the order belongs to.
        /// </summary>
        [JsonPropertyName("locationGroup")]
        public LocationGroup LocationGroup { get; set; }

        /// <summary>
        /// The associated sales order number.
        /// </summary>
        [JsonPropertyName("salesOrder")]
        public SalesOrder SalesOrder { get; set; }

        /// <summary>
        /// Timestamp of when the order was created.
        /// </summary>
        [JsonPropertyName("dateCreated")]  //'yyyy-MM-dd'
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Timestamp of when the order was last modified and the user that made the modifications.
        /// </summary>
        [JsonPropertyName("lastModified")]  //'yyyy-MM-dd'
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Timestamp of when the order was issued.
        /// </summary>
        [JsonPropertyName("dateIssued")]  //'yyyy-MM-dd'
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? DateIssued { get; set; }

        /// <summary>
        /// Timestamp of when the order was scheduled.
        /// </summary>
        [JsonPropertyName("dateScheduled")]  //'yyyy-MM-dd'
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? DateScheduled { get; set; }

        /// <summary>
        /// Timestamp of when the order was completed.
        /// </summary>
        [JsonPropertyName("dateCompleted")]  //'yyyy-MM-dd'
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? DateCompleted { get; set; }

        /// <summary>
        /// The url link on the order.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// The percent of the order that is complete.
        /// </summary>
        [JsonPropertyName("percentComplete")]
        public string PercentComplete { get; set; }

        /// <summary>
        /// The class category.
        /// </summary>
        [JsonPropertyName("class")]
        public Category Class { get; set; }

        /// <summary>
        /// A list of the manufacture order configurations.
        /// </summary>
        [JsonPropertyName("configurations")]
        public List<ManufactureOrderConfiguration> Configurations { get; set; }

        /// <summary>
        /// A list of custom fields associated with the order.
        /// </summary>
        [JsonPropertyName("customFields")]
        public List<CustomField> CustomFields { get; set; }
    }
}
