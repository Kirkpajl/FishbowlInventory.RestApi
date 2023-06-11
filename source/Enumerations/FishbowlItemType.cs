using System.ComponentModel.DataAnnotations;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// The Fishbowl item type.
    /// </summary>
    /// <remarks>
    /// 'Part', 'Product', 'Customer', 'Vendor', 'SO', 'PO', 'TO', 'MO', 'RMA', 'BOM'
    /// </remarks>
    public enum FishbowlItemType
    {
        [Display(Name = "Part")] Part,
        [Display(Name = "Product")] Product,
        [Display(Name = "Customer")] Customer,
        [Display(Name = "Vendor")] Vendor,
        [Display(Name = "SO")] SalesOrder,
        [Display(Name = "PO")] PurchaseOrder,
        [Display(Name = "TO")] TransferOrder,
        [Display(Name = "MO")] ManufactureOrder,
        [Display(Name = "RMA")] ReturnManufacturerAuthorization,
        [Display(Name = "BOM")] BillOfMaterials
    }
}
