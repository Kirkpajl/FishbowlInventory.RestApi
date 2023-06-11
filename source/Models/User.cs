using FishbowlInventory.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    /// <summary>
    /// This is an object representing a Fishbowl user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user's unique identification number.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The username.
        /// </summary>
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        /// <summary>
        /// The first name of the user.
        /// </summary>
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// The initials of the user's name.
        /// </summary>
        [JsonPropertyName("initials")]
        public string Initials { get; set; }

        /// <summary>
        /// The active status of the user.
        /// </summary>
        [JsonPropertyName("active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// The email address of the User.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the User.
        /// </summary>
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// A list of the custom fields associated with the user.
        /// </summary>
        public List<CustomField> CustomFields { get; set; }


        
        /// <summary>
        /// A string separated with the pipe character ("|") listing the User Groups the User is a part of.
        /// </summary>
        /// <remarks>
        /// There must be no spaces between delimeters and the text.
        /// </remarks>
        [JsonPropertyName("userGroups")]
        public string UserGroups { get; set; }

        /// <summary>
        /// A string specifying the default Location Group.
        /// </summary>
        /// <remarks>
        /// This string must also appear in the <seealso cref="LocGroups"/> string below.
        /// </remarks>
        [JsonPropertyName("defaultLocGroup")]
        public string DefaultLocGroup { get; set; }

        /// <summary>
        /// A string separated with the pipe character ("|") listing the Location Groups the User is a part of.
        /// </summary>
        /// <remarks>
        /// There must be no spaces between delimeters and the text.
        /// </remarks>
        [JsonPropertyName("locGroups")]
        public string LocGroups { get; set; }
    }
}
