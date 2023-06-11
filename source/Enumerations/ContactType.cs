using System.ComponentModel.DataAnnotations;

namespace FishbowlInventory.Enumerations
{
    /// <summary>
    /// The basic type of the contact
    /// </summary>
    /// <remarks> 'Home' | 'Work' | 'Mobile' | 'Fax' | 'Main' | 'Email' | 'Pager' | 'Other' | 'Web'</remarks>
    public enum ContactType
    {
        [Display(Name = "Home")] Home = 10,
        [Display(Name = "Work")] Work = 20,
        [Display(Name = "Mobile")] Mobile = 30,
        [Display(Name = "Fax")] Fax = 40,
        [Display(Name = "Main")] Main = 50,
        [Display(Name = "Email")] Email = 60,
        [Display(Name = "Pager")] Pager = 70,
        [Display(Name = "Other")] Other = 80,
        [Display(Name = "Web")] Web = 90
    }
}
