using System.ComponentModel.DataAnnotations;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// The order status.
    /// </summary>
    /// <remarks>
    /// 'Entered' | 'Started' | 'Fulfilled'
    /// </remarks>
    public enum WorkOrderStatus
    {
        [Display(Name = "Entered")] Entered = 10,
        [Display(Name = "Started")] Started = 30,
        [Display(Name = "Fulfilled")] Fulfilled = 40
    }
}
