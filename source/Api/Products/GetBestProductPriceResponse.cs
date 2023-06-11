using FishbowlInventory.Api.Common;
using FishbowlInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Api.Products
{
    /// <summary>
    /// Retrieves the best unit price for the specific product.
    /// </summary>
    public class GetBestProductPriceResponse : IFishbowlResponse
    {
        [JsonPropertyName("bestUnitPrice")]
        public decimal BestUnitPrice { get; set; }

        [JsonPropertyName("pricingRules")]
        public PricingRule[] PricingRules { get; set; }
    }
}
