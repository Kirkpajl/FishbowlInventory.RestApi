using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory
{
    public class ApplicationConfig
    {
        [JsonPropertyName("baseUrl")]
        public string BaseUrl { get; set; } //= "https://fishbowl.fabsource.com:8080";  // "https://fsdbs01:8080"



        [JsonPropertyName("username")]
        public string Username { get; set; } //= "Kiosk";

        [JsonPropertyName("password")]
        public string Password { get; set; } //= "richrobin49";



        [JsonPropertyName("appName")]
        public string AppName { get; set; } //= "REST API Test Client";

        [JsonPropertyName("appDescription")]
        public string AppDescription { get; set; } //= "Tests the new REST API endpoints";

        [JsonPropertyName("appId")]
        public int AppId { get; set; } //= 1234;



        [JsonPropertyName("sessionToken")]
        public string SessionToken { get; set; }
    }
}
