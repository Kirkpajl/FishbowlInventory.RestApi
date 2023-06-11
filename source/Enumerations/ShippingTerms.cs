using System.ComponentModel.DataAnnotations;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// The shipping terms.
    /// </summary>
    /// <remarks>
    /// 'Prepaid & Billed' | 'Prepaid' | 'Freight Collect'
    /// </remarks>
    public enum ShippingTerms
    {
        [Display(Name = "Prepaid & Billed")] PrepaidBilled,
        [Display(Name = "Prepaid")] Prepaid,
        [Display(Name = "Freight Collect")] FreightCollect
    }
}
