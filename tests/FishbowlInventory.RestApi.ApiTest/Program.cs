using FishbowlInventory.Exceptions;
using FishbowlInventory.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory
{
    class Program
    {
        private static ApplicationConfig Configuration { get; } = LoadConfiguration();



        static async Task Main()
        {            
            // Output Fishbowl configuration
            Console.WriteLine("FishbowlInventory Test Client");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"AppName:  {Configuration.AppName}");
            Console.WriteLine($"AppDescription:  {Configuration.AppDescription}");
            Console.WriteLine($"AppId:  {Configuration.AppId}");
            Console.WriteLine($"Username:  {Configuration.Username}");
            Console.WriteLine($"Password:  {Configuration.Password}");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("");

            // Initialize the Fishbowl Inventory REST Api client
            using var fishbowl = new FishbowlInventoryApiClient(Configuration.BaseUrl, Configuration.AppName, Configuration.AppDescription, Configuration.AppId, Configuration.Username, Configuration.Password, allowInvalidServerCertificates: true);

            // Terminate the previous Fishbowl session (if token is present), then start a new session
            bool isLoggedIn = await InitializeSession(fishbowl);

            if (isLoggedIn)
            {
                // Plugins
                //await TestPlugins(fishbowl);  //TODO: Determine what authorization token to use for the Fishbowl Plugin Search

                // Inventory Parts
                //await TestInventoryParts(fishbowl);

                // Location Groups
                //await TestLocationGroups(fishbowl);

                // Manufacture Orders
                //await TestManufactureOrders(fishbowl);

                // Parts
                //await TestParts(fishbowl);

                // Purchase Orders
                await TestPurchaseOrders(fishbowl);

                // Units of Measure
                //await TestUnitsOfMeasure(fishbowl);

                // Users
                //await TestUsers(fishbowl);

                // Vendors
                //await TestVendors(fishbowl);

                // Work Orders
                //await TestWorkOrders(fishbowl);

                // Terminate the Fishbowl API session
                await TerminateSession(fishbowl);

                Console.WriteLine("All tests completed.  Press any key to exit...");
            }
            else
            {
                Console.WriteLine("Unable to authenticate with Fishbowl.  Testing has been cancelled.  Press any key to exit...");
            }

            // Pause execution
            Console.ReadKey();
        }



        private static async Task<bool> InitializeSession(FishbowlInventoryApiClient fishbowl)
        {
            // Ensure that the previous Fishbowl session was closed
            if (!string.IsNullOrWhiteSpace(Configuration.SessionToken))
            {
                Console.WriteLine("Terminating the previous user session...");
                
                // Attempt to terminate the previous session token
                try
                {
                    await fishbowl.LogoutAsync(Configuration.SessionToken);
                }
                catch (FishbowlInventoryAuthenticationException) { }  // Swallow 401 error if the server already terminated the token
                catch (Exception ex)
                {
                    OutputException(ex, $"An {ex.GetType().Name} occurred while terminating the previous user session!");
                }
                
                // Clear the token value
                Configuration.SessionToken = null;
                SaveConfig();


                Console.WriteLine($"Previous user session is terminated.");
                Console.WriteLine();
            }

            // Attempt to login
            try
            {
                Console.WriteLine("Initializing user session...");

                // Login to the Fishbowl Inventory server
                var loginUser = await fishbowl.LoginAsync();

                if (loginUser == null) return false;

                // Retain the new session token
                Configuration.SessionToken = fishbowl.Token;
                SaveConfig();

                // User details
                Console.WriteLine($"User Name:  {loginUser.FullName}");
                Console.WriteLine($"Allowed Modules ({loginUser.AllowedModules.Length}):");
                foreach (var module in loginUser.AllowedModules) Console.WriteLine($"  * {module}");
                Console.WriteLine($"Server Version:  {loginUser.ServerVersion}");
                Console.WriteLine();

                return true;
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while initializing the user session!");
                return false;
            }
        }

        /*private static async Task TestPlugins(FishbowlInventoryApiClient fishbowl)
        {
            try
            {
                Console.WriteLine($"Testing Plugins api...");

                var plugins = await fishbowl.SearchPluginInformationAsync("Zoho", "");

                Console.WriteLine($"Plugins ({plugins.Length}):");
                foreach (var plugin in plugins)
                {
                    Console.WriteLine($"  [{plugin.Id}]:  {plugin.Name}");
                }
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while testing the Plugins Api!");
            }
        }*/

        private static async Task TestInventoryParts(FishbowlInventoryApiClient fishbowl)
        {
            try
            {
                Console.WriteLine($"Testing Inventory Parts api...");

                var inventoryPartsResponse = await fishbowl.SearchInventoryPartsAsync(1);

                Console.WriteLine($"Inventory Parts (Page {inventoryPartsResponse.PageNumber} of {inventoryPartsResponse.TotalPages}):");
                foreach (var partInventory in inventoryPartsResponse.Results)
                {
                    Console.WriteLine($"  [{partInventory.Id}]:  {partInventory.PartNumber} - {partInventory.PartDescription}");
                }
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while testing the Inventory Parts Api!");
            }
        }

        private static async Task TestLocationGroups(FishbowlInventoryApiClient fishbowl)
        {
            try
            {
                Console.WriteLine($"Testing Location Groups api...");

                var locationGroupsResponse = await fishbowl.SearchLocationGroupsAsync(1);

                Console.WriteLine($"Location Groups (Page {locationGroupsResponse.PageNumber} of {locationGroupsResponse.TotalPages}):");
                foreach (var locationGroup in locationGroupsResponse.Results)
                {
                    Console.WriteLine($"  [{locationGroup.Id}]:  {locationGroup.Name}");
                }
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while testing the Location Groups Api!");
            }
        }

        private static async Task TestManufactureOrders(FishbowlInventoryApiClient fishbowl)
        {            
            try
            {
                Console.WriteLine($"Testing Manufacture Orders api...");

                var manufactureOrdersResponse = await fishbowl.SearchManufactureOrdersAsync(1);

                Console.WriteLine($"Manufacture Orders (Page {manufactureOrdersResponse.PageNumber} of {manufactureOrdersResponse.TotalPages}):");
                foreach (var manufactureOrder in manufactureOrdersResponse.Results)
                {
                    Console.WriteLine($"  [{manufactureOrder.Id}]:  {manufactureOrder.Number} - {manufactureOrder.BillOfMaterialDescription}");
                }
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while testing the Manufacture Orders Api!");
            }
        }

        private static async Task TestParts(FishbowlInventoryApiClient fishbowl)
        {
            try
            {
                Console.WriteLine($"Testing Parts api...");

                var partsResponse = await fishbowl.SearchPartsAsync(1);

                Console.WriteLine($"Parts (Page {partsResponse.PageNumber} of {partsResponse.TotalPages}):");
                foreach (var part in partsResponse.Results)
                {
                    Console.WriteLine($"  [{part.Id}]:  {part.Number} - {part.Description}");
                }
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while testing the Parts Api!");
            }
        }

        private static async Task TestPurchaseOrders(FishbowlInventoryApiClient fishbowl)
        {
            try
            {
                Console.WriteLine($"Testing Purchase Order api...");

                // Get all Purchase Orders
                var purchaseOrders = await fishbowl.GetPurchaseOrdersAsync();
                Console.WriteLine($"Purchase Orders ({purchaseOrders.Length}):");

                foreach (var purchaseOrder in purchaseOrders)
                {
                    Console.WriteLine($"  [{purchaseOrder.Id}]:  {purchaseOrder.Number} - {purchaseOrder.Vendor.Name} - {purchaseOrder.Items.Count:N0} items");
                }
                Console.WriteLine("");

                // Create a new Purchase Order
                var vendors = await fishbowl.GetVendorsAsync();
                var vendor = vendors.FirstOrDefault();
                const string newPurchaseOrderNumber = "012345-6789";

                Console.WriteLine($"Creating new Purchase Order record ({newPurchaseOrderNumber}):");
                var newPurchaseOrder = await fishbowl.CreatePurchaseOrderAsync(new PurchaseOrder
                {
                    Number = newPurchaseOrderNumber,
                    Status = Enumerations.PurchaseOrderStatus.BidRequest,
                    Vendor =
                    {
                        Id = vendor.Id,
                        Name = vendor.Name,                        
                    }
                });
                if (newPurchaseOrder == null)
                    Console.WriteLine("Unable to create record!");
                else
                    Console.WriteLine($"New record id:  {newPurchaseOrder.Id}");

                Console.WriteLine("");

                // Delete the new Purchase Order
                Console.WriteLine($"Deleting new Purchase Order record ({newPurchaseOrderNumber}):");
                await fishbowl.DeletePurchaseOrderAsync(newPurchaseOrder.Id);
                newPurchaseOrder = await fishbowl.GetPurchaseOrderAsync(newPurchaseOrder.Id);

                if (newPurchaseOrder == null)
                    Console.WriteLine("Record deleted!");
                else
                    Console.WriteLine($"Unable to delete PO record with id:  {newPurchaseOrder.Id}");

                Console.WriteLine("");






                /*
                var purchaseOrdersResponse = await fishbowl.SearchPurchaseOrdersAsync(1);

                Console.WriteLine($"Purchase Orders (Page {purchaseOrdersResponse.PageNumber} of {purchaseOrdersResponse.TotalPages}):");
                foreach (var purchaseOrder in purchaseOrdersResponse.Results)
                {
                    Console.WriteLine($"  [{purchaseOrder.Id}]:  {purchaseOrder.Number} - {purchaseOrder.VendorName}");
                }
                Console.WriteLine("");
                */
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while testing the Purchase Orders Api!");
            }
        }

        private static async Task TestUnitsOfMeasure(FishbowlInventoryApiClient fishbowl)
        {
            try
            {
                Console.WriteLine($"Testing Units of Measure api...");

                var uomResponse = await fishbowl.SearchUnitOfMeasuresAsync(1);

                Console.WriteLine($"Units of Measure (Page {uomResponse.PageNumber} of {uomResponse.TotalPages}):");
                foreach (var uom in uomResponse.Results)
                {
                    Console.WriteLine($"  [{uom.Id}]:  {uom.Name} ({uom.Abbreviation})");
                }
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while testing the Units Of Measure Api!");
            }
        }
        
        private static async Task TestUsers(FishbowlInventoryApiClient fishbowl)
        {
            try
            {
                Console.WriteLine($"Testing Users api...");

                // All Users
                var users = await fishbowl.GetUsersAsync();
                Console.WriteLine($"All Users ({users.Length}):");

                foreach (var user in users)
                {
                    Console.WriteLine($"  [{user.Id}]:  {user.LastName}, {user.FirstName} ({user.UserName})");
                }
                Console.WriteLine("");

                // Users in group "Purchasing"
                var purchasingUsers = await fishbowl.GetUsersInGroupAsync("Purchasing");
                Console.WriteLine($"Purchasing Users ({purchasingUsers.Length}):");

                foreach (var user in purchasingUsers)
                {
                    Console.WriteLine($"  [{user.Id}]:  {user.LastName}, {user.FirstName} ({user.UserName})");
                }
                Console.WriteLine("");


                /*
                // Get all Users' names/initials from Fishbowl
                var fishbowlPurchasingUsers = await client.GetUsersInGroupAsync("Purchasing", cancellationToken: token);
                */


                /*
                var userResponse = await fishbowl.SearchUsersAsync(1);

                Console.WriteLine($"Users (Page {userResponse.PageNumber} of {userResponse.TotalPages}):");
                foreach (var user in userResponse.Results)
                {
                    Console.WriteLine($"  [{user.Id}]:  {user.LastName}, {user.FirstName} ({user.UserName})");
                }
                Console.WriteLine("");
                */
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while testing the Users Api!");
            }
        }

        private static async Task TestVendors(FishbowlInventoryApiClient fishbowl)
        {
            try
            {
                Console.WriteLine($"Testing Vendors api...");

                // Get all Vendor records
                var vendors = await fishbowl.GetVendorsAsync();
                Console.WriteLine($"Vendors ({vendors.Length}):");

                foreach (var vendor in vendors)
                {
                    string description = string.IsNullOrWhiteSpace(vendor.AccountNumber) ? vendor.Name : $"{vendor.Name} ({vendor.AccountNumber})";

                    Console.WriteLine($"  [{vendor.Id}]:  {description}");
                }
                Console.WriteLine("");

                // Search for Vendor records
                /*
                var vendorsResponse = await fishbowl.SearchVendorsAsync(1);
                Console.WriteLine($"Vendors (Page {vendorsResponse.PageNumber} of {vendorsResponse.TotalPages}):");

                foreach (var vendor in vendorsResponse.Results)
                {
                    string description = string.IsNullOrWhiteSpace(vendor.AccountNumber) ? vendor.Name : $"{vendor.Name} ({vendor.AccountNumber})";

                    Console.WriteLine($"  [{vendor.Id}]:  {description}");
                }
                Console.WriteLine("");
                */

                // Create a new Vendor record
                const string newVendorName = "AAAbbbCCC";
                Console.WriteLine($"Creating new Vendor record ({newVendorName}):");
                var newVendor = await fishbowl.CreateVendorAsync(new Vendor
                {
                    Name = newVendorName,
                    IsActive = true,
                    Addresses =
                    {
                        new Address
                        {
                            Name = "MAIN",
                            Attention = "[ATTENTION]",
                            StreetAddress = "[STREET ADDRESS]",
                            City = "[CITY]",
                            State = "TN",
                            PostalCode = "12345",
                            IsDefault = true,
                            Type = Enumerations.AddressType.MainOffice
                        }
                    }
                });
                if (newVendor == null)
                    Console.WriteLine("Unable to create record!");
                else 
                    Console.WriteLine($"New record id:  {newVendor.Id}");

                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while testing the Vendors Api!");
            }
        }

        private static async Task TestWorkOrders(FishbowlInventoryApiClient fishbowl)
        {
            try
            {
                Console.WriteLine($"Testing Work Order api...");

                var workOrders = await fishbowl.GetWorkOrdersAsync();

                foreach (var workOrder in workOrders)
                {
                    string description = string.IsNullOrWhiteSpace(workOrder.Note) ? "TBD" : workOrder.Note;
                    description = description.Replace("\r", "; ");
                    description = description.Replace("\n", "; ");
                    description = description.Replace("\r\n", "; ");

                    Console.WriteLine($"  [{workOrder.Id}]:  {workOrder.Number} - {description}");
                }
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while testing the Work Orders Api!");
            }
        }

        private static async Task TerminateSession(FishbowlInventoryApiClient fishbowl)
        {
            try
            {
                Console.WriteLine($"Terminating user session...");

                // Logout from the Fishbowl Inventory server
                await fishbowl.LogoutAsync();

                // Clear the cached SessionToken
                Configuration.SessionToken = null;
                SaveConfig();

                Console.WriteLine($"User session is terminated.");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                OutputException(ex, $"An {ex.GetType().Name} occurred while terminating the user session!");
            }
        }



        #region Configuration Helper Methods

        //private static IConfigurationRoot LoadConfigurationRoot() =>
        //    new ConfigurationBuilder()
        //    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //    .AddJsonFile("appsettings.json", true, true)
        //    .Build();

        private static ApplicationConfig LoadConfiguration() =>
            new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", true, true)
            .Build()
            .Get<ApplicationConfig>();

        /// <summary>
        /// Serialize the config object and overwrite appsettings.json
        /// </summary>
        /// <param name="config"></param>
        private static void SaveConfig()
        {
            // Serialize the Config object
            var jsonWriteOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            jsonWriteOptions.Converters.Add(new JsonStringEnumConverter());

            var configJson = JsonSerializer.Serialize(Configuration, jsonWriteOptions);

            // Overwrite the appsettings.json file with the new JSON
            var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            File.WriteAllText(appSettingsPath, configJson);
        }

        #endregion Configuration Helper Methods



        /// <summary>
        /// Write the exception to the console with a custom format
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        private static void OutputException(Exception ex, string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                message = $"An {ex.GetType().Name} occurred while testing the Fishbowl Inventory REST Api!";

            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(message);
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();

            Console.ForegroundColor = currentColor;
        }
    }
}
