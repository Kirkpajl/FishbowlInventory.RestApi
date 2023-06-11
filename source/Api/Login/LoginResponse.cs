using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FishbowlInventory.Api.Common;
using FishbowlInventory.Models;

namespace FishbowlInventory.Api.Login
{
    class LoginResponse : IFishbowlResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("user")]
        public UserInformation User { get; set; }
    }

    public class UserInformation
    {
        [JsonPropertyName("userFullName")]
        public string FullName { get; set; }

        [JsonPropertyName("moduleAccessList")]
        public string[] AllowedModules { get; set; }

        [JsonPropertyName("serverVersion")]
        public string ServerVersion { get; set; }
    }
}
