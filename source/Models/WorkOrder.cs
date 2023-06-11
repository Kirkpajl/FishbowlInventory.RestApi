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
	/// Contains all the work order information and details.
	/// </summary>
	public class WorkOrder
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
        
		[JsonPropertyName("calCategoryId")]
        public int? CalCategoryId { get; set; }

		[JsonPropertyName("cost")]
		public decimal Cost { get; set; }

        [JsonPropertyName("customerId")]
        public int? CustomerId { get; set; }

        [JsonPropertyName("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [JsonPropertyName("dateFinished")]
        public DateTime? DateFinished { get; set; }

        [JsonPropertyName("dateLastModified")]
        public DateTime? DateLastModified { get; set; }

        [JsonPropertyName("dateScheduled")]
        public DateTime? DateScheduled { get; set; }

        [JsonPropertyName("dateScheduledToStart")]
        public DateTime? DateScheduledToStart { get; set; }

        [JsonPropertyName("dateStarted")]
        public DateTime? DateStarted { get; set; }

        [JsonPropertyName("locationGroupId")]
        public int LocationGroupId { get; set; }

        [JsonPropertyName("locationId")]
        public int LocationId { get; set; }

        [JsonPropertyName("moItemId")]
        public int ManufacturingOrderItemId { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("num")]
        public string Number { get; set; }

        [JsonPropertyName("priorityId")]
        public int PriorityId { get; set; }

        [JsonPropertyName("qbClassId")]
        public int QuickBooksClassId { get; set; }

        [JsonPropertyName("qtyOrdered")]
        public int QuantityOrdered { get; set; }

        [JsonPropertyName("qtyScrapped")]
        public int? QuantityScrapped { get; set; }

        [JsonPropertyName("qtyTarget")]
        public int QuantityTarget { get; set; }

        [JsonPropertyName("statusId")]
        public int StatusId { get; set; }

        [JsonConverter(typeof(EnumDisplayConverter))]
        [JsonPropertyName("status")]
        public WorkOrderStatus Status { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }



        [JsonPropertyName("locationName")]
		public string LocationName { get; set; }

		[JsonPropertyName("locationGroupName")]
		public string LocationGroupName { get; set; }

        [JsonPropertyName("manufacturingOrderNumber")]
        public string ManufacturingOrderNumber { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

		[JsonPropertyName("quickBooksClassName")]
		public string QuickBooksClassName { get; set; }

        [JsonPropertyName("statusName")]
        public string StatusName { get; set; }

        [JsonPropertyName("userName")]
		public string UserName { get; set; }
    }
}
