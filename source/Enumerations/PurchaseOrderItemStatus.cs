using FishbowlInventory.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// The order item status.
    /// </summary>
    /// <remarks>
    /// 'All Open' | 'Entered' | 'Picking' | 'Partial' | 'Picked' | 'Shipped' | 'Fulfilled' | 'Closed Short' | 'Void' | 'Historical'
    /// </remarks>
    [JsonConverter(typeof(EnumDisplayConverter))]
    public enum PurchaseOrderItemStatus
    {
        [Display(Name = "All Open")] AllOpen = 5,
        [Display(Name = "Entered")] Entered = 10,
        [Display(Name = "Picking")] Picking = 20,
        [Display(Name = "Partial")] Partial = 30,
        [Display(Name = "Picked")] Picked = 40,
        [Display(Name = "Shipped")] Shipped = 45,
        [Display(Name = "Fulfilled")] Fulfilled = 50,
        [Display(Name = "Closed Short")] ClosedShort = 60,
        [Display(Name = "Void")] Void = 70,
        [Display(Name = "Historical")] Historical = 80
    }
}
