using System.ComponentModel.DataAnnotations;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// Indicates the type of purchase order item
    /// </summary>
    /// <remarks> 'Purchase' | 'Misc. Purchase' | 'Credit Return' | 'Misc. Credit' | 'Out Sourced' | 'Shipping'</remarks>
    public enum PurchaseOrderItemType
    {
        [Display(Name = "Purchase")] 
        Purchase = 10,

        [Display(Name = "Misc. Purchase")] 
        MiscPurchase = 11,

        [Display(Name = "Credit Return")] 
        CreditReturn = 20,

        [Display(Name = "Misc. Credit")] 
        MiscCredit = 21,

        [Display(Name = "Out Sourced")] 
        OutSourced = 30,

        [Display(Name = "Shipping")] 
        Shipping = 40
    }
}
