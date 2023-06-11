using System.ComponentModel.DataAnnotations;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// The status of the vendor.
    /// </summary>
    /// <remarks>
    /// "Normal", "Preferred", "Hold PO", "Hold Receipt", "Hold All"
    /// </remarks>
    public enum VendorStatus
    {
        [Display(Name = "Normal")] Normal = 10,
        [Display(Name = "Preferred")] Preferred = 20,
        [Display(Name = "Hold PO")] HoldPO = 30,
        [Display(Name = "Hold Receipt")] HoldReceipt = 40,
        [Display(Name = "Hold All")] HoldAll = 50
    }
}
