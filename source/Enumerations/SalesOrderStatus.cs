using System.ComponentModel.DataAnnotations;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// The order status.
    /// </summary>
    /// <remarks>
    /// 'All' | 'All Open' | 'Bid Request' | 'Pending Approval' | 'Issued' | 'Picking' | 'Partial' | 'Picked' | 'Shipped' | 'Fulfilled' | 'Closed Short' | 'Void' | 'Historical'
    /// </remarks>
    public enum SalesOrderStatus
    {
        [Display(Name = "Estimate")] Estimate = 10,
        [Display(Name = "Issued")] Issued = 20,
        [Display(Name = "Cancelled")] Cancelled = 85,
        [Display(Name = "Historical")] Historical = 95
    }
}
