using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
	/// <summary>
	/// Stores information about an item found in a work order.
	/// </summary>
	public class WorkOrderItem
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("moItemId")]
		public int ManufacturingOrderItemId { get; set; }

        [JsonPropertyName("partId")]
        public int PartId { get; set; }

        [JsonPropertyName("typeId")]
		public int TypeId { get; set; }

        [JsonPropertyName("uomId")]
        public int UomId { get; set; }

        [JsonPropertyName("woId")]
        public int WorkOrderId { get; set; }

  //      [JsonPropertyName("part")]
		//public Part Part { get; set; }

		[JsonPropertyName("partNumber")]
		public string PartNumber { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("cost")]
		public string Cost { get; set; }

        [JsonPropertyName("standardCost")]
        public string StandardCost { get; set; }

  //      [JsonPropertyName("dateScheduled")]
		//public DateTime? DateScheduled { get; set; }

		//[JsonPropertyName("instructionNote")]
		//public string InstructionNote { get; set; }

		//[JsonPropertyName("instructionURL")]
		//public string InstructionURL { get; set; }

		[JsonPropertyName("qtyScrapped")]
		public int QuantityScrapped { get; set; }

		[JsonPropertyName("qtyTarget")]
		public int QuantityTarget { get; set; }

		[JsonPropertyName("qtyUsed")]
		public int QuantityUsed { get; set; }

        [JsonPropertyName("sortId")]
        public int SortId { get; set; }

        //      [JsonPropertyName("destLocation")]
        //public Location DestinationLocation { get; set; }

        //[JsonPropertyName("uom")]
        //public UnitOfMeasure UnitOfMeasure { get; set; }

        [JsonPropertyName("code")]
        public string UOM { get; set; }

        //[JsonPropertyName("tracking")]
        //public string Tracking { get; set; }
    }
}
