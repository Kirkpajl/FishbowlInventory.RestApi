using System.ComponentModel.DataAnnotations;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// The address type.
    /// </summary>
    /// <remarks> 'Ship To' | 'Bill To' | 'Remit To' | 'Home' | 'Main Office'</remarks>
    public enum AddressType
    {
        [Display(Name = "Ship To")] ShipTo = 10,
        [Display(Name = "Bill To")] BillTo = 20,
        [Display(Name = "Remit To")] RemitTo = 30,
        [Display(Name = "Home")] Home = 40,
        [Display(Name = "Main Office")] MainOffice = 50
    }
}
