using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// The type of sales order item
    /// </summary>
    /// <remarks> 'Purchase' | 'Misc. Purchase' | 'Credit Return' | 'Misc. Credit' | 'Out Sourced' | 'Shipping'</remarks>
    public enum SalesOrderItemType
    {
        [Display(Name = "Sale")]
        Sale = 10,

        [Display(Name = "Misc. Sale")]
        MiscSale = 11,

        [Display(Name = "Drop Ship")]
        DropShip = 12,

        [Display(Name = "Credit Return")]
        CreditReturn = 20,

        [Display(Name = "Misc. Credit")]
        MiscCredit = 21,

        [Display(Name = "Discount Percentage")]
        DiscountPercentage = 30,

        [Display(Name = "Discount Amount")]
        DiscountAmount = 31,

        [Display(Name = "Subtotal")]
        Subtotal = 40,

        [Display(Name = "Assoc. Price")]
        AssocPrice = 50,

        [Display(Name = "Shipping")]
        Shipping = 60,

        [Display(Name = "Tax")]
        Tax = 70,

        [Display(Name = "Kit")]
        Kit = 80,

        [Display(Name = "Note")]
        Note = 90,

        [Display(Name = "BOM Configuration Item")]
        BOMConfigurationItem = 31
    }
}
