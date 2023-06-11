using FishbowlInventory.Api.Inventory;
using FishbowlInventory.Api.Login;
using FishbowlInventory.Api.Products;
using FishbowlInventory.Converters;
using FishbowlInventory.Enumerations;
using FishbowlInventory.Exceptions;
using FishbowlInventory.Extensions;
using FishbowlInventory.Helpers;
using FishbowlInventory.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace FishbowlInventory
{
    public sealed class FishbowlInventoryApiClient : IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FishbowlInventory.FishbowlInventoryApiClient class.
        /// </summary>
        /// <param name="baseAddress">The base URL of the API endpoints (i.e., "http://localhost:60464/api/").</param>
        public FishbowlInventoryApiClient(string baseAddress, string applicationName = default, string applicationDescription = default, int applicationId = default, 
            string userName = default, string userPassword = default, bool allowInvalidServerCertificates = false)
        {
            // Correct any BaseAddress formatting issues
            baseAddress = baseAddress.RemoveFromEnd("/").RemoveFromEnd("/api");

            // Create a handler to ignore SSL certificate validation errors (if directed)
            HttpClientHandler httpClientHandler = allowInvalidServerCertificates
                ? new HttpClientHandler { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator }
                : new HttpClientHandler();

            // Create an instance of HttpClient and assign the base address of the Web API.
            _httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(baseAddress)
            };
            //_httpClient.DefaultRequestHeaders.Clear();
            //_httpClient.DefaultRequestHeaders.ConnectionClose = true;

            // Initialize the global JSON serialization options
            _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            _jsonOptions.Converters.Add(new JsonStringEnumConverter());
            _jsonOptions.Converters.Add(new DataTableJsonConverter());
            _jsonOptions.Converters.Add(new FishbowlDateTimeConverter());
            _jsonOptions.Converters.Add(new DateOnlyConverter());
            _jsonOptions.Converters.Add(new EnumDescriptionConverter());
            _jsonOptions.Converters.Add(new EnumDisplayConverter());

            // Retain any Fishbowl configuration values
            _applicationName = applicationName;
            _applicationDescription = applicationDescription;
            _applicationId = applicationId;
            _userName = userName;
            _userPassword = userPassword;
        }

        ~FishbowlInventoryApiClient()
        {
            Dispose(false);
        }

        #endregion Constructors

        #region Private Fields

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private bool _disposed;

        private string _token, _applicationName, _applicationDescription, _userName, _userPassword;
        private int _applicationId;
        private readonly object _syncLock = new object();

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Authorization Token for the current session
        /// </summary>
        public string Token => _token;



        /// <summary>
        /// Fishbowl User Account Name
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// Fishbowl User Account Password
        /// </summary>
        public string UserPassword
        {
            get { return _userPassword; }
            set { _userPassword = value; }
        }



        /// <summary>
        /// Application ID; can be any number, but must be unique in Fishbowl
        /// </summary>
        public int ApplicationId
        {
            get { return _applicationId; }
            set { _applicationId = value; }
        }

        /// <summary>
        /// Application Name, must be unique in Fishbowl
        /// </summary>
        public string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        /// <summary>
        /// Application Description, must be unique in Fishbowl
        /// </summary>
        public string ApplicationDescription
        {
            get { return _applicationDescription; }
            set { _applicationDescription = value; }
        }

        #endregion Public Properties

        
        
        #region Login/Logout

        public Task<UserInformation> LoginAsync(string appName, string appDescription, int appId, string username, string password, CancellationToken cancellationToken = default)
        {
            // Update the Fishbowl connection properties
            _applicationName = appName;
            _applicationDescription = appDescription;
            _applicationId = appId;
            _userName = username;
            _userPassword = password;

            // Execute the login request
            return LoginAsync(cancellationToken);
        }

        public async Task<UserInformation> LoginAsync(CancellationToken cancellationToken = default)
        {
            // Execute the login request
            var request = BuildLoginRequest();
            var response = await PostAsync<LoginRequest, LoginResponse>("/api/login", request, cancellationToken);

            // Cache the token
            SetToken(response.Token);

            // Return the user information
            return response.User;
        }



        public Task LogoutAsync(string token, CancellationToken cancellationToken = default)
        {
            // Overwrite the cached token with the supplied value
            if (!string.IsNullOrWhiteSpace(token))
            {
                SetToken(token);
            }

            // Terminate the specified session
            return LogoutAsync(cancellationToken);
        }

        public async Task LogoutAsync(CancellationToken cancellationToken = default)
        {
            // End the current Fishbowl session
            await PostAsync("/api/logout", cancellationToken);

            // Clear the cached token
            SetToken(null);
        }




        private LoginRequest BuildLoginRequest() => new LoginRequest { AppName = _applicationName, AppDescription = _applicationDescription, AppId = _applicationId, Username = _userName, Password = _userPassword };

        /// <summary>
        /// Update the private <see cref="_token"/> field in a thread-safe manner, as well as modifying the 'Authorization' header.
        /// </summary>
        /// <param name="token"></param>
        private void SetToken(string token)
        {
            // Set the token field value
            lock (_syncLock)
            {
                _token = token;
            }

            // Add/Remove the 'Authorization' header
            if (token == null)
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                //_httpClient.DefaultRequestHeaders.Authorization = null;
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Token);
            }
        }

        #endregion Login

        #region Imports/Exports

        /// <summary>
        /// Import CSV data.
        /// </summary>
        /// <param name="name">The name parameter should be replaced by the case-sensitive default name of the CSV import/export with "-" replacing any spaces.  (i.e.: Sales Order Details -> Sales-Order-Details)</param>
        /// <param name="lines"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ImportAsync(string name, string[] lines, CancellationToken cancellationToken = default)
        {
            // Build the request URL
            name = name.Replace(" ", "-");
            string requestUri = $"/api/import/{name}";

            // Set content type of the POST request to text/csv
            var content = new StringContent(string.Join("\r\n", lines), Encoding.UTF8, "text/csv");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute),
                Content = content
            };

            // HTTP POST
            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw await HandleRequestFailureAsync(requestUri, response, "Failed to complete HTTP POST request.", cancellationToken);
            }
        }



        /// <summary>
        /// Execute a data query against the database using SQL.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteQueryAsync(string sqlQuery, CancellationToken cancellationToken = default)
        {
            const string requestUri = "/api/data-query";

            // Set content type of the GET request to application/sql
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute),
                Content = new StringContent(sqlQuery, Encoding.UTF8, "application/sql")
            };

            // HTTP GET
            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(json);
                return doc.RootElement.ToDataTable();
            }
            else
            {
                throw await HandleRequestFailureAsync(requestUri, response, "Failed to complete HTTP GET request.", cancellationToken);
            }
        }

        /// <summary>
        /// Execute a data query against the database using SQL.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T[]> ExecuteQueryAsync<T>(string sqlQuery, CancellationToken cancellationToken = default)
        {
            const string requestUri = "/api/data-query";

            // Set content type of the GET request to application/sql
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute),
                Content = new StringContent(sqlQuery, Encoding.UTF8, "application/sql")
            };

            // HTTP GET
            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                //string json = await response.Content.ReadAsStringAsync(cancellationToken);
                return await response.Content.ReadFromJsonAsync<T[]>(_jsonOptions, cancellationToken);
            }
            else
            {
                throw await HandleRequestFailureAsync(requestUri, response, "Failed to complete HTTP GET request.", cancellationToken);
            }
        }

        /// <summary>
        /// Execute a scalar data query against the database using SQL.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResult> ExecuteScalarQueryAsync<TResult>(string sqlQuery, CancellationToken cancellationToken = default)
        {
            // Execute the data query
            TResult[] results = await ExecuteQueryAsync<TResult>(sqlQuery, cancellationToken);

            // Return the first/only record
            return results.FirstOrDefault();
        }

        #endregion Imports/Exports

        #region Integrations

        /// <summary>
        /// Search for plugin information.
        /// </summary>
        /// <param name="plugin">The name of the plugin.</param>
        /// <param name="authorization">The user specified code for authorization.</param>
        /// <param name="table">The table name in the database.</param>
        /// <param name="recordId">The unique identifier for a record in Fishbowl associated with the table.</param>
        /// <param name="groupId">The user specified identifier used to separate types of plugins you are using.</param>
        /// <param name="channelId">The unique identifier for the object being linked externally.</param>
        /// <returns></returns>
        public Task<PluginInformation[]> SearchPluginInformationAsync(string plugin, string authorization, string table = null, int? recordId = null, 
            int? groupId = null, string channelId = null, CancellationToken cancellationToken = default)
        {
            // Ensure that the required parameters were provided
            if (string.IsNullOrWhiteSpace(plugin)) throw new ArgumentNullException(nameof(plugin));
            if (string.IsNullOrWhiteSpace(authorization)) throw new ArgumentNullException(nameof(authorization));

            // Assemble the Request Uri
            var queryParameters = new Dictionary<string, string>
            {
                { "plugin", plugin },
                { "authorization", authorization }
            };

            queryParameters.AddParameterIfNotNull("table", table);
            queryParameters.AddParameterIfNotNull("recordId", recordId);
            queryParameters.AddParameterIfNotNull("groupId", groupId);
            queryParameters.AddParameterIfNotNull("channelId", channelId);

            string requestUri = QueryHelpers.AddQueryString("/api/integrations/plugin-info", queryParameters);

            // Process the request
            return GetAsync<PluginInformation[]>(requestUri, cancellationToken);
        }

        /// <summary>
        /// Create a plugin information
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        public Task<PluginInformation[]> CreatePluginInformationAsync(PluginInformation plugin, CancellationToken cancellationToken = default) 
            => PostAsync<PluginInformation, PluginInformation[]>("/api/integrations/plugin-info", plugin, cancellationToken);

        /// <summary>
        /// Update a plugin information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="plugin"></param>
        /// <returns></returns>
        public Task<PluginInformation[]> UpdatePluginInformationAsync(int id, PluginInformation plugin, CancellationToken cancellationToken = default)
            => PostAsync<PluginInformation, PluginInformation[]>($"/api/integrations/plugin-info/{id}", plugin, cancellationToken);

        /// <summary>
        /// Deletes the plugin information with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="plugin"></param>
        /// <returns></returns>
        public Task DeletePluginInformationAsync(int id, CancellationToken cancellationToken = default) 
            => DeleteAsync($"/api/integrations/plugin-info/{id}", cancellationToken);

        #endregion Integrations

        #region Inventory

        /// <summary>
        /// Searches for inventory.
        /// </summary>
        /// <param name="pageNumber">The current page of the results.</param>
        /// <param name="pageSize">The number of returned results per page. (Default 100)</param>
        /// <param name="number">The part number.</param>
        /// <param name="description">The part description.</param>
        /// <param name="locationId">The unique identifier for the location.</param>
        /// <param name="locationGroupId">The unique identifier for the location group.</param>
        /// <param name="abc">The ABC code for the part.  ('A' | 'B' | 'C')</param>
        /// <param name="includeZeroQuantities">Indicates whether to include parts with no on hand inventory.</param>
        /// <param name="active">Indicates whether to only include active (or inactive) parts.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PagedResultSet<PartInventory>> SearchInventoryPartsAsync(int pageNumber, int pageSize = 100, string number = null, string description = null, 
            int? locationId = null, int? locationGroupId = null, string abc = null, bool includeZeroQuantities = false, bool? active = true, CancellationToken cancellationToken = default)
        {
            // Assemble the Request Uri
            var queryParameters = new Dictionary<string, string>
            {
                { "pageNumber", pageNumber.ToString() },
                { "pageSize", pageSize.ToString() },
                { "includeZeroQuantities", includeZeroQuantities.ToString() }
            };

            queryParameters.AddParameterIfNotNull("number", number);
            queryParameters.AddParameterIfNotNull("description", description);
            queryParameters.AddParameterIfNotNull("locationId", locationId);
            queryParameters.AddParameterIfNotNull("locationGroupId", locationGroupId);
            queryParameters.AddParameterIfNotNull("abc", abc);
            queryParameters.AddParameterIfNotNull("active", active);

            //if (number != null) queryParameters.Add("number", number);
            //if (description != null) queryParameters.Add("description", description);
            //if (locationId != null) queryParameters.Add("locationId", locationId?.ToString());
            //if (locationGroupId != null) queryParameters.Add("locationGroupId", locationGroupId?.ToString());
            //if (abc != null) queryParameters.Add("abc", abc);
            //if (active != null) queryParameters.Add("active", active?.ToString());

            string requestUri = QueryHelpers.AddQueryString("/api/parts/inventory", queryParameters);

            // Process the request
            return GetAsync<PagedResultSet<PartInventory>>(requestUri, cancellationToken);  //SearchInventoryResponse
        }
                
        /// <summary>
        /// Adds inventory.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task AddInventoryAsync(int id, AddInventoryRequest request, CancellationToken cancellationToken = default)
            => PostAsync($"/api/parts/{id}/inventory/add", request, cancellationToken);

        /// <summary>
        /// Cycles inventory.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task CycleInventoryAsync(int id, CycleInventoryRequest request, CancellationToken cancellationToken = default)
            => PostAsync($"/api/parts/{id}/inventory/cycle", request, cancellationToken);

        /// <summary>
        /// Moves inventory from one location to another.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task MoveInventoryAsync(int id, MoveInventoryRequest request, CancellationToken cancellationToken = default)
            => PostAsync($"/api/parts/{id}/inventory/move", request, cancellationToken);

        /// <summary>
        /// Scraps inventory. Parts that have tracking only require the primary tracking value.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task ScrapInventoryAsync(int id, ScrapInventoryRequest request, CancellationToken cancellationToken = default)
            => PostAsync($"/api/parts/{id}/inventory/scrap", request, cancellationToken);

        #endregion Inventory

        #region Location Groups

        /// <summary>
        /// Searches for location groups.
        /// </summary>
        /// <param name="pageNumber">The current page of the results.</param>
        /// <param name="pageSize">The number of returned results per page. (Default 100)</param>
        /// <param name="name">The location group name.</param>
        /// <param name="active">The active status of the location groups.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PagedResultSet<LocationGroup>> SearchLocationGroupsAsync(int pageNumber, int pageSize = 100, string name = null, bool? active = null, CancellationToken cancellationToken = default)
        {
            // Assemble the Request Uri
            var queryParameters = new Dictionary<string, string>
            {
                { "pageNumber", pageNumber.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            queryParameters.AddParameterIfNotNull("name", name);
            queryParameters.AddParameterIfNotNull("active", active);

            //if (name != null) queryParameters.Add("name", name);
            //if (active != null) queryParameters.Add("active", active?.ToString());

            string requestUri = QueryHelpers.AddQueryString("/api/location-groups", queryParameters);

            // Process the request
            return GetAsync<PagedResultSet<LocationGroup>>(requestUri, cancellationToken);  //SearchLocationGroupResponse
        }

        #endregion Location Groups

        #region Manufacture Orders

        /// <summary>
        /// Searches for manufacture orders.
        /// </summary>
        /// <param name="pageNumber">The current page of the results.</param>
        /// <param name="pageSize">The number of returned results per page. (Default 100)</param>
        /// <param name="moNumber ">The manufacture order number.</param>
        /// <param name="status">The order status.</param>
        /// <param name="bomNumber ">A BOM number used in a manufacture order.</param>
        /// <param name="soNumber ">The linked Sales Order number for a manufacture order.</param>
        /// <param name="assignedUserId">The unique identifier of the user assigned to a manufacture order.</param>
        /// <param name="locationGroupId">The unique identifier of a manufacture order's location group.</param>
        /// <param name="woCategory">The work order category name.</param>
        /// <param name="issuedFrom">The start issued date cutoff for the search.</param>
        /// <param name="issuedTo">The end issued date cutoff for the search.</param>
        /// <param name="scheduledFrom">The start scheduled date cutoff for the search.</param>
        /// <param name="scheduledTo">The end scheduled date cutoff for the search.</param>
        /// <param name="fulfilledFrom">The start fulfilled date cutoff for the search.</param>
        /// <param name="fulfilledTo">The end fulfilled date cutoff for the search.</param>
        /// <param name="containingPartNumber">A part number contained within a manufacture order.</param>
        /// <param name="containingPartDescription">A part description contained within a manufacture order.</param>
        /// <param name="containingBomItemType">The type of a BOM item contained in the manufacture order.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PagedResultSet<ManufactureOrderSearchResult>> SearchManufactureOrdersAsync(int pageNumber, int pageSize = 100, string moNumber  = null, ManufactureOrderStatus? status = null,
            string bomNumber = null, string soNumber = null, int? assignedUserId = null, int? locationGroupId = null, string woCategory = null, 
            DateTime? issuedFrom = null, DateTime? issuedTo = null, DateTime? scheduledFrom = null, DateTime? scheduledTo = null, DateTime? fulfilledFrom = null, DateTime? fulfilledTo = null,
            string containingPartNumber = null, string containingPartDescription = null, BillOfMaterialItemType? containingBomItemType = null,
            CancellationToken cancellationToken = default)
        {
            // Assemble the Request Uri
            var queryParameters = new Dictionary<string, string>
            {
                { "pageNumber", pageNumber.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            queryParameters.AddParameterIfNotNull("moNumber", moNumber);
            queryParameters.AddParameterIfNotNull("status", status);
            queryParameters.AddParameterIfNotNull("bomNumber", bomNumber);
            queryParameters.AddParameterIfNotNull("soNumber", soNumber);
            queryParameters.AddParameterIfNotNull("assignedUserId", assignedUserId);
            queryParameters.AddParameterIfNotNull("locationGroupId", locationGroupId);
            queryParameters.AddParameterIfNotNull("woCategory", woCategory);
            queryParameters.AddParameterIfNotNull("issuedFrom", issuedFrom);
            queryParameters.AddParameterIfNotNull("issuedTo", issuedTo);
            queryParameters.AddParameterIfNotNull("scheduledFrom", scheduledFrom);
            queryParameters.AddParameterIfNotNull("scheduledTo", scheduledTo);
            queryParameters.AddParameterIfNotNull("fulfilledFrom", fulfilledFrom);
            queryParameters.AddParameterIfNotNull("fulfilledTo", fulfilledTo);
            queryParameters.AddParameterIfNotNull("containingPartNumber", containingPartNumber);
            queryParameters.AddParameterIfNotNull("containingPartDescription", containingPartDescription);
            queryParameters.AddParameterIfNotNull("containingBomItemType", containingBomItemType);

            string requestUri = QueryHelpers.AddQueryString("/api/manufacture-orders", queryParameters);

            // Process the request
            return GetAsync<PagedResultSet<ManufactureOrderSearchResult>>(requestUri, cancellationToken);
        }

        /// <summary>
        /// Retrieves the details of an existing manufacture order. You only need to provide the unique manufacture order ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ManufactureOrder> GetManufactureOrderAsync(int id, CancellationToken cancellationToken = default) 
            => GetAsync<ManufactureOrder>($"/api/manufacture-orders/{id}", cancellationToken);

        /// <summary>
        /// Create a manufacture order
        /// </summary>
        /// <param name="manufactureOrder"></param>
        /// <returns></returns>
        public Task<ManufactureOrder> CreateManufactureOrderAsync(ManufactureOrder manufactureOrder, CancellationToken cancellationToken = default) 
            => PostAsync<ManufactureOrder, ManufactureOrder>("/api/manufacture-orders", manufactureOrder, cancellationToken);

        /// <summary>
        /// Updates a manufacture order. Configurations will only be updated when the order status is 'Entered'. Manufacture orders linked to sales orders will not have their configurations updated.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="manufactureOrder"></param>
        /// <returns></returns>
        public Task<ManufactureOrder> UpdateManufactureOrderAsync(int id, ManufactureOrder manufactureOrder, CancellationToken cancellationToken = default) 
            => PostAsync<ManufactureOrder, ManufactureOrder>($"/api/manufacture-orders/{id}", manufactureOrder, cancellationToken);

        /// <summary>
        /// Issues the manufacture order with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ManufactureOrder> IssueManufactureOrderAsync(int id, CancellationToken cancellationToken = default)
            => PostAsync<ManufactureOrder>($"/api/manufacture-orders/{id}/issue", cancellationToken);

        /// <summary>
        /// Unissues the manufacture order with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ManufactureOrder> UnissueManufactureOrderAsync(int id, CancellationToken cancellationToken = default)
            => PostAsync<ManufactureOrder>($"/api/manufacture-orders/{id}/unissue", cancellationToken);

        /// <summary>
        /// Closes short the manufacture order with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ManufactureOrder> CloseShortManufactureOrderAsync(int id, CancellationToken cancellationToken = default)
            => PostAsync<ManufactureOrder>($"/api/manufacture-orders/{id}/close-short", cancellationToken);

        /// <summary>
        /// Deletes the manufacture order with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteManufactureOrderAsync(int id, CancellationToken cancellationToken = default)
            => DeleteAsync($"/api/manufacture-orders/{id}", cancellationToken);

        #endregion Manufacture Orders

        #region Memos

        /// <summary>
        /// Retrieves a list of memos for the specific object.
        /// </summary>
        /// <returns></returns>
        public Task<Memo[]> GetAllManufactureOrderMemosAsync(int id, CancellationToken cancellationToken = default)
            => GetAsync<Memo[]>($"/api/manufacture-orders/{id}/memos", cancellationToken);

        /// <summary>
        /// Retrieves the details of an existing memo. You only need to provide the unique memo ID.
        /// </summary>
        /// <returns></returns>
        public Task<Memo> GetManufactureOrderMemoAsync(int id, int memoId, CancellationToken cancellationToken = default)
            => GetAsync<Memo>($"/api/manufacture-orders/{id}/memos/{memoId}", cancellationToken);

        /// <summary>
        /// Creates a memo.
        /// </summary>
        /// <returns></returns>
        public Task<Memo> CreateManufactureOrderMemoAsync(int id, Memo memo, CancellationToken cancellationToken = default)
            => PostAsync<Memo, Memo>($"/api/manufacture-orders/{id}/memos", memo, cancellationToken);

        /// <summary>
        /// Updates a memo.
        /// </summary>
        /// <returns></returns>
        public Task<Memo> UpdateManufactureOrderMemoAsync(int id, int memoId, Memo memo, CancellationToken cancellationToken = default)
            => PostAsync<Memo, Memo>($"/api/manufacture-orders/{id}/memos/{memoId}", memo, cancellationToken);

        /// <summary>
        /// Deletes the memo with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteManufactureOrderMemoAsync(int id, int memoId, CancellationToken cancellationToken = default)
            => DeleteAsync($"/api/manufacture-orders/{id}/memos/{memoId}", cancellationToken);



        /// <summary>
        /// Retrieves a list of memos for the specific object.
        /// </summary>
        /// <returns></returns>
        public Task<Memo[]> GetAllPurchaseOrderMemosAsync(int id, CancellationToken cancellationToken = default)
            => GetAsync<Memo[]>($"/api/purchase-orders/{id}/memos", cancellationToken);

        /// <summary>
        /// Retrieves the details of an existing memo. You only need to provide the unique memo ID.
        /// </summary>
        /// <returns></returns>
        public Task<Memo> GetPurchaseOrderMemoAsync(int id, int memoId, CancellationToken cancellationToken = default)
            => GetAsync<Memo>($"/api/purchase-orders/{id}/memos/{memoId}", cancellationToken);

        /// <summary>
        /// Creates a memo.
        /// </summary>
        /// <returns></returns>
        public Task<Memo> CreatePurchaseOrderMemoAsync(int id, Memo memo, CancellationToken cancellationToken = default)
            => PostAsync<Memo, Memo>($"/api/purchase-orders/{id}/memos", memo, cancellationToken);

        /// <summary>
        /// Updates a memo.
        /// </summary>
        /// <returns></returns>
        public Task<Memo> UpdatePurchaseOrderMemoAsync(int id, int memoId, Memo memo, CancellationToken cancellationToken = default)
            => PostAsync<Memo, Memo>($"/api/manufacture-orders/{id}/memos/{memoId}", memo, cancellationToken);

        /// <summary>
        /// Deletes the memo with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeletePurchaseOrderMemoAsync(int id, int memoId, CancellationToken cancellationToken = default)
            => DeleteAsync($"/api/purchase-orders/{id}/memos/{memoId}", cancellationToken);

        #endregion Memos

        #region Parts

        //private const string PART_IMPORT_NAME = "ImportPart";
        //private const string PART_AVG_COST_IMPORT_NAME = "ImportPartCost";
        //private const string PART_STD_COST_IMPORT_NAME = "ImportPartStdCost";

        private const string PART_SELECT_QUERY =
            @"SELECT
                p.id,
                p.num AS number, 
                p.description, 
                p.upc,
                t.name AS type,
                p.abcCode AS abc,
                p.details, 
                IF(p.defaultBomId IS NULL or p.defaultBomId = '', false, true) as hasBom,
                p.activeFlag AS active,
                prod.num AS productNumber,
                prod.description AS productDescription,
                vp.vendorPartNumber,
                v.name AS vendorName,

                u.code AS uom, 
                p.weight, 
                uw.code AS weightUOM, 
                p.width, 
                p.height, 
                p.len AS length,
                us.code AS sizeUOM, 
                p.alertNote AS alertNote, 
                p.stdCost AS standardCost, 
                ch.avgCost AS averageCost,
                p.trackingFlag AS hasTracking,
                p.serializedFlag AS isSerialized
                
            FROM part p

            INNER JOIN parttype AS t ON t.id = p.typeId
            LEFT JOIN product AS prod ON prod.id = p.defaultProductId
            LEFT JOIN partcosthistory AS ch ON ch.partId = p.id
            LEFT JOIN vendorparts AS vp ON vp.partId = p.id
            LEFT JOIN vendor AS v ON v.id = vp.VendorId
            INNER JOIN uom AS u ON u.id = p.uomId
            INNER JOIN uom AS uw ON uw.id = p.weightUomId
            INNER JOIN uom AS us ON us.id = p.sizeUomId";



        /// <summary>
        /// Get all Parts
        /// </summary>
        public Task<Part[]> GetPartsAsync(PartType? partType = null, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{PART_SELECT_QUERY} {(partType.HasValue ? $"WHERE p.typeId = '{(int)partType.Value}'" : "")} ORDER BY p.num";

            // Execute the SELECT query
            return ExecuteQueryAsync<Part>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Searches for parts.
        /// </summary>
        /// <param name="pageNumber">The current page of the results.</param>
        /// <param name="pageSize">The number of returned results per page. (Default 100)</param>
        /// <param name="number">The part number.</param>
        /// <param name="description">The part description.</param>
        /// <param name="upc">The UPC code for the part.</param>
        /// <param name="type">The basic type of the part.</param>
        /// <param name="abc">The ABC code for the part.</param>
        /// <param name="details">The part details.</param>
        /// <param name="hasBom">
        /// Indicates if the part has an associated bill of materials. True returns parts with an 
        /// associated bill of materials, false does not filter the results.
        /// </param>
        /// <param name="active">The active status of the UOM.</param>
        /// <param name="productNumber">The associated product number.</param>
        /// <param name="productDescription">The product description.</param>
        /// <param name="vendorPartNumber">The vendor part number.</param>
        /// <param name="vendorName">The name of the associated vendor.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PagedResultSet<Part>> SearchPartsAsync(int pageNumber, int pageSize = 100, string number = null, string description = null, string upc = null, PartType? type = null, string abc = null,
            string details = null, bool? hasBom = null, bool? active = null, string productNumber = null, string productDescription = null, string vendorPartNumber = null, string vendorName = null, 
            CancellationToken cancellationToken = default)
        {
            // Assemble the Request Uri
            var queryParameters = new Dictionary<string, string>
            {
                { "pageNumber", pageNumber.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            queryParameters.AddParameterIfNotNull("number", number);
            queryParameters.AddParameterIfNotNull("description", description);
            queryParameters.AddParameterIfNotNull("upc", upc);
            queryParameters.AddParameterIfNotNull("type", type);
            queryParameters.AddParameterIfNotNull("abc", abc);
            queryParameters.AddParameterIfNotNull("details", details);
            queryParameters.AddParameterIfNotNull("hasBom", hasBom);
            queryParameters.AddParameterIfNotNull("active", active);
            queryParameters.AddParameterIfNotNull("productNumber", productNumber);
            queryParameters.AddParameterIfNotNull("productDescription", productDescription);
            queryParameters.AddParameterIfNotNull("vendorPartNumber", vendorPartNumber);
            queryParameters.AddParameterIfNotNull("vendorName", vendorName);

            string requestUri = QueryHelpers.AddQueryString("/api/parts", queryParameters);

            // Process the request
            return GetAsync<PagedResultSet<Part>>(requestUri, cancellationToken);  //SearchPartResponse
        }

        /// <summary>
        /// Retrieves the best cost for a specific part. The cost will be in the currency of the vendor.
        /// </summary>
        /// <param name="id">The unique identifier for the part.</param>
        /// <param name="vendorId">The unique identifier for the vendor associated with the part.</param>
        /// <param name="quantity">The part quantity associated with the cost.</param>
        /// <param name="uomId">The unique identifier for the unit of measure.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<VendorPartCost> GetBestPartCostAsync(int id, int vendorId, double quantity, int uomId, CancellationToken cancellationToken = default)
        {
            // Ensure that the parameters are valid
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity), "The quantity must be greater than zero.");

            // Assemble the Request Uri
            var queryParameters = new Dictionary<string, string>();

            queryParameters.AddParameterIfNotNull("vendorId", vendorId);
            queryParameters.AddParameterIfNotNull("quantity", quantity);
            queryParameters.AddParameterIfNotNull("uomId", uomId);

            string requestUri = QueryHelpers.AddQueryString($"/api/parts/{id}/best-cost", queryParameters);

            // Process the request
            return GetAsync<VendorPartCost>(requestUri, cancellationToken);
        }

        /// <summary>
        /// Retrieves the details of an existing part. You only need to provide the unique part ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Part> GetPartAsync(int id, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{PART_SELECT_QUERY} WHERE p.id = '{id}'";

            // Execute the SELECT query
            var parts = await ExecuteQueryAsync<Part>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Return the first/only record
            return parts.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the details of an existing part. You only need to provide the unique part number.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task<Part> GetPartByNumberAsync(string number, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{PART_SELECT_QUERY} WHERE p.num = '{number}'";

            // Execute the SELECT query
            var parts = await ExecuteQueryAsync<Part>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Return the first/only record
            return parts.FirstOrDefault();
        }

        #endregion Parts

        #region Products

        /// <summary>
        /// Retrieves the best unit price for the specific product.
        /// </summary>
        /// <param name="id">The unique identifier for the product.</param>
        /// <param name="customerId">The unique identifier for the customer associated with the product.</param>
        /// <param name="quantity">The product quantity associated with the cost.</param>
        /// <param name="date">The date on which the best price will be calculated.</param>
        /// <param name="uomId">The unique identifier for the unit of measure.</param>
        /// <param name="includePricingRules">Indicates whether the list of pricing rules should be returned.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<GetBestProductPriceResponse> GetBestProductPriceAsync(int id, int customerId, double quantity, DateTime? date = null, int? uomId = null, 
            bool? includePricingRules = null, CancellationToken cancellationToken = default)
        {
            // Ensure that the parameters are valid
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity), "The quantity must be greater than zero.");

            // Assemble the Request Uri
            var queryParameters = new Dictionary<string, string>();

            queryParameters.AddParameterIfNotNull("customerId", customerId);
            queryParameters.AddParameterIfNotNull("quantity", quantity);
            queryParameters.AddParameterIfNotNull("date", date);
            queryParameters.AddParameterIfNotNull("uomId", uomId);
            queryParameters.AddParameterIfNotNull("includePricingRules", includePricingRules);

            string requestUri = QueryHelpers.AddQueryString($"/api/products/{id}/best-price", queryParameters);

            // Process the request
            return GetAsync<GetBestProductPriceResponse>(requestUri, cancellationToken);
        }

        #endregion Products

        #region Purchase Orders

        private const string PURCHASEORDER_IMPORT_NAME = "PurchaseOrder";
        private const string PURCHASEORDER_SELECT_QUERY =
            @"SELECT
            po.id AS Id,
            po.buyer AS BuyerUserName,
            po.buyerId AS BuyerId,
            po.carrierId AS CarrierId,
            po.currencyId AS CurrencyId,
            po.currencyRate AS CurrencyRate,
            po.customerSO AS CustomerSalesOrderNumber,
            po.dateCompleted AS CompletedDate,
            po.dateConfirmed AS ConfirmedDate,
            po.dateCreated AS CreatedDate,
            po.dateFirstShip AS FulfillmentDate,
            po.dateIssued AS IssuedDate,
            po.dateLastModified AS LastModifiedDate,
            po.dateRevision AS RevisionDate,
            po.deliverTo AS DeliverToName,
            po.email AS Email,
            po.fobPointId AS FobPointId,
            po.locationGroupId AS LocationGroupId,
            po.note AS Note,
            po.num AS Number,
            po.paymentTermsId AS PaymentTermsId,
            po.phone AS Phone,
            po.qbClassId AS QuickBooksClassId,
            po.remitAddress AS RemitToStreetAddress,
            po.remitCity AS RemitToCity,
            po.remitToName AS RemitToName,
            po.remitZip AS RemitToZip,
            po.revisionNum AS RevisionNumber,
            po.shipTermsId AS ShippingTermsId,
            po.shipToAddress AS ShipToStreetAddress,
            po.shipToCity AS ShipToCity,
            po.shipToName AS ShipToName,
            po.shipToZip AS ShipToZip,
            po.statusId AS Status,
            po.taxRateName AS TaxRateName,
            po.totalIncludesTax TotalIncludesTax,
            po.totalTax AS TotalTax,
            po.typeId AS Type,
            po.url AS URL,
            po.username AS Username,
            po.vendorContact AS VendorContact,
            po.vendorId AS VendorId,
            po.vendorSO AS VendorSalesOrderNumber,

            buyer.firstName AS BuyerFirstName,
            buyer.lastName AS BuyerLastName,
            carrier.name AS CarrierName,
            carsrv.name AS CarrierServiceName,
            curr.name AS CurrencyName,
            fp.name AS FobPointName,
            loc.name AS LocationGroupName,
            pterm.name AS PaymentTermsName,
            qb.name AS QuickBooksClassName,
            rctry.name AS RemitToCountry,
            rstate.name AS RemitToState,
            sctry.name AS ShipToCountry,
            sstate.name AS ShipToState,
            stat.name AS StatusName,
            sterm.name AS ShippingTermsName,
            type.name AS TypeName,
            vendor.name AS VendorName
                
            FROM po

            INNER JOIN sysuser AS buyer ON buyer.id = po.buyerId
            INNER JOIN carrier ON carrier.id = po.carrierId
            LEFT JOIN carrierservice AS carsrv ON carsrv.id = po.carrierServiceId
            LEFT JOIN currency AS curr ON curr.id = po.currencyId
            INNER JOIN fobpoint AS fp ON fp.id = po.fobPointId
            INNER JOIN locationgroup AS loc ON loc.id = po.locationGroupId
            INNER JOIN paymentterms AS pterm ON pterm.id = po.paymentTermsId
            LEFT JOIN qbclass AS qb ON qb.id = po.qbClassId
            LEFT JOIN countryconst AS rctry ON rctry.id = po.remitCountryId
            LEFT JOIN stateconst AS rstate ON rstate.id = po.remitStateId
            LEFT JOIN countryconst AS sctry ON sctry.id = po.shipToCountryId
            LEFT JOIN stateconst AS sstate ON sstate.id = po.shipToStateId
            INNER JOIN postatus AS stat ON stat.id = po.statusId
            INNER JOIN shipterms AS sterm ON sterm.id = po.shipTermsId
            INNER JOIN potype AS type ON type.id = po.typeId
            INNER JOIN vendor ON vendor.id = po.vendorId";
        /*private const string PURCHASEORDER_SELECT_QUERY =
            @"SELECT
            po.id AS Id,
            po.num AS number,
            po.statusId,
            stat.name AS status,
            po.qbClassId AS classId,
            qb.name AS className,
            po.carrierId,
            carrier.name AS carrierName,
            po.fobPointId AS FobPointId,
            fp.name AS fobPointName,
            po.paymentTermsId,
            pterm.name AS paymentTermsName,
            po.shipTermsId,
            sterm.name AS shipTermsName,
            po.typeId,
            type.name AS type,
            po.vendorId,
            vendor.name AS vendorName,
            po.vendorContact,
            po.vendorSO AS vendorSoNumber,
            po.customerSO AS customerSoNumber,
            po.buyerId,
            po.buyer AS buyerUserName,
            buyer.firstName AS buyerFirstName,
            buyer.lastName AS buyerLastName,
            po.deliverTo AS deliverTo,
            po.revisionNum AS revisionNumber,
            po.dateLastModified,
            po.dateIssued,
            po.dateCreated,
            po.username AS createdByUsername,
            po.dateConfirmed,
            po.dateRevision,
            po.dateFirstShip AS dateScheduled,
            po.dateCompleted,
            po.taxRateId,
            po.taxRateName,
            po.totalTax,
            po.totalIncludesTax,
            po.locationGroupId,
            loc.name AS locationGroupName,
            po.note,
            po.url,
            po.currencyId,
            po.currencyRate,
            curr.name AS currencyName,
            po.email,
            po.phone,

            po.shipToName,
            po.shipToAddress AS shipToStreetAddress,
            po.shipToCity,
            sstate.name AS shipToState,
            po.shipToZip,
            sctry.name AS shipToCountry,

            po.remitToName,
            po.remitAddress AS remitToStreetAddress,
            po.remitCity AS remitToCity,
            rstate.name AS remitToState,
            po.remitZip,
            rctry.name AS remitToCountry,

            po.carrierServiceId,
            carsrv.name AS carrierServiceName            
                
            FROM po

            INNER JOIN sysuser AS buyer ON buyer.id = po.buyerId
            INNER JOIN carrier ON carrier.id = po.carrierId
            LEFT JOIN carrierservice AS carsrv ON carsrv.id = po.carrierServiceId
            LEFT JOIN currency AS curr ON curr.id = po.currencyId
            INNER JOIN fobpoint AS fp ON fp.id = po.fobPointId
            INNER JOIN locationgroup AS loc ON loc.id = po.locationGroupId
            INNER JOIN paymentterms AS pterm ON pterm.id = po.paymentTermsId
            LEFT JOIN qbclass AS qb ON qb.id = po.qbClassId
            LEFT JOIN countryconst AS rctry ON rctry.id = po.remitCountryId
            LEFT JOIN stateconst AS rstate ON rstate.id = po.remitStateId
            LEFT JOIN countryconst AS sctry ON sctry.id = po.shipToCountryId
            LEFT JOIN stateconst AS sstate ON sstate.id = po.shipToStateId
            INNER JOIN postatus AS stat ON stat.id = po.statusId
            INNER JOIN shipterms AS sterm ON sterm.id = po.shipTermsId
            INNER JOIN potype AS type ON type.id = po.typeId
            INNER JOIN vendor ON vendor.id = po.vendorId";*/



        /// <summary>
        /// Return all Purchase Orders in Fishbowl
        /// </summary>
        /// <returns></returns>
        public async Task<PurchaseOrder[]> GetPurchaseOrdersAsync(CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{PURCHASEORDER_SELECT_QUERY} ORDER BY po.num";

            // Execute the SELECT query and cast to object collection
            var purchaseOrdersTable = await ExecuteQueryAsync(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Parse the rows into PurchaseOrder/PurchaseOrderItem objects
            return ToPurchaseOrders(purchaseOrdersTable);
        }

        /*public Task<PurchaseOrder[]> GetPurchaseOrdersAsync(CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{PURCHASEORDER_SELECT_QUERY} ORDER BY po.num";

            // Execute the SELECT query and cast to object collection
            return ExecuteQueryAsync<PurchaseOrder>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);
        }*/

        /// <summary>
        /// Searches for purchase orders.
        /// </summary>
        /// <param name="pageNumber">The current page of the results.</param>
        /// <param name="pageSize">The number of returned results per page. (Default 100)</param>
        /// <param name="poNumber">The purchase order number.</param>
        /// <param name="poType">Indicates whether the order is a standard or drop ship purchase order.</param>
        /// <param name="vendorSoNumber">The vendor sales order number.</param>
        /// <param name="issuedFrom">The start issued date cutoff for the search.</param>
        /// <param name="issuedTo">The end issued date cutoff for the search.</param>
        /// <param name="fulfilledFrom">The start fulfilled date cutoff for the search.</param>
        /// <param name="fulfilledTo">The end fulfilled date cutoff for the search.</param>
        /// <param name="vendorName">The name of the vendor.</param>
        /// <param name="partNumber">The part number.</param>
        /// <param name="partDescription">The description on the part.</param>
        /// <param name="partDetails">The details for the part.</param>
        /// <param name="status">The order status.</param>
        /// <param name="customerSoNumber">The customer sales order number.</param>
        /// <param name="locationGroupId">The unique identifier for the location group on the order.</param>
        /// <param name="vendorPartNumber">The number for the vendor part.</param>
        /// <param name="buyerId">The unique identifier for the salesperson on the order.</param>
        /// <param name="remitTo">The vendor address on the order.</param>
        /// <param name="shipTo">The ship to address on the order.</param>
        /// <param name="customerName">The name of the customer or job on the order.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PagedResultSet<PurchaseOrder>> SearchPurchaseOrdersAsync(int pageNumber, int pageSize = 100, string poNumber = null, PurchaseOrderType? poType = null,
            string vendorSoNumber = null, DateTime? issuedFrom = null, DateTime? issuedTo = null, DateTime? fulfilledFrom = null, DateTime? fulfilledTo = null,
            string vendorName = null, string partNumber = null, string partDescription = null, string partDetails = null, PurchaseOrderStatus? status = null,
            string customerSoNumber = null, int? locationGroupId = null, string vendorPartNumber = null, int? buyerId = null,
            string remitTo = null, string shipTo = null, string customerName = null, CancellationToken cancellationToken = default)
        {
            // Assemble the Request Uri
            var queryParameters = new Dictionary<string, string>
            {
                { "pageNumber", pageNumber.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            queryParameters.AddParameterIfNotNull("poNumber", poNumber);
            queryParameters.AddParameterIfNotNull("poType", poType);
            queryParameters.AddParameterIfNotNull("vendorSoNumber", vendorSoNumber);
            queryParameters.AddParameterIfNotNull("issuedFrom", issuedFrom);
            queryParameters.AddParameterIfNotNull("issuedTo", issuedTo);
            queryParameters.AddParameterIfNotNull("fulfilledFrom", fulfilledFrom);
            queryParameters.AddParameterIfNotNull("fulfilledTo", fulfilledTo);
            queryParameters.AddParameterIfNotNull("vendorName", vendorName);
            queryParameters.AddParameterIfNotNull("partNumber", partNumber);
            queryParameters.AddParameterIfNotNull("partDescription", partDescription);
            queryParameters.AddParameterIfNotNull("partDetails", partDetails);
            queryParameters.AddParameterIfNotNull("status", status);
            queryParameters.AddParameterIfNotNull("customSoNumber", customerSoNumber);
            queryParameters.AddParameterIfNotNull("locationGroupId", locationGroupId);
            queryParameters.AddParameterIfNotNull("vendorPartNumber", vendorPartNumber);
            queryParameters.AddParameterIfNotNull("buyerId", buyerId);
            queryParameters.AddParameterIfNotNull("remitTo", remitTo);
            queryParameters.AddParameterIfNotNull("shipTo", shipTo);
            queryParameters.AddParameterIfNotNull("customerName", customerName);

            string requestUri = QueryHelpers.AddQueryString("/api/purchase-orders", queryParameters);

            // Process the request
            return GetAsync<PagedResultSet<PurchaseOrder>>(requestUri, cancellationToken);  //SearchPurchaseOrderResponse
        }

        /// <summary>
        /// Retrieves the details of an existing purchase order. You only need to provide the unique purchase order ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PurchaseOrder> GetPurchaseOrderAsync(int id, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{PURCHASEORDER_SELECT_QUERY} WHERE po.id = '{id}'";

            // Execute the SELECT query
            var purchaseOrdersTable = await ExecuteQueryAsync(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Parse the rows into PurchaseOrder/PurchaseOrderItem objects
            return ToPurchaseOrders(purchaseOrdersTable).FirstOrDefault();
        }
        /*public Task<PurchaseOrder> GetPurchaseOrderAsync(int id, CancellationToken cancellationToken = default)
            => GetAsync<PurchaseOrder>($"/api/purchase-orders/{id}", cancellationToken);*/



        /// <summary>
        /// Retrieves the details of an existing purchase order. You only need to provide the unique purchase order number.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task<PurchaseOrder> GetPurchaseOrderByNumberAsync(string number, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{PURCHASEORDER_SELECT_QUERY} WHERE po.num = '{number}'";

            // Execute the SELECT query
            var purchaseOrdersTable = await ExecuteQueryAsync(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Parse the rows into PurchaseOrder/PurchaseOrderItem objects
            return ToPurchaseOrders(purchaseOrdersTable).FirstOrDefault();
        }

        /// <summary>
        /// Return all active Purchase Order Items in Fishbowl belonging to the specified Purchase Order
        /// </summary>
        /// <param name="poNumber">The unique number of the Purchase Order</param>
        /// <returns></returns>
        private async Task<PurchaseOrderItem[]> GetPurchaseOrderItemsAsync(string poNumber, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $@"
                SELECT
                poitem.id AS Id,
                poitem.customerId AS CustomerId,
                poitem.dateLastFulfillment AS LastFulfillmentDate,
                poitem.dateScheduledFulfillment AS FulfillmentDate,
                poitem.description AS Description,
                poitem.mcTotalCost AS McTotalCost,
                poitem.note AS Note,
                poitem.partNum AS PartNumber,
                poitem.poLineItem AS LineItem,
                poitem.qtyFulfilled AS FulfilledQuantity,
                poitem.qtyPicked AS PickedQuantity,
                poitem.qtyToFulfill AS PartQuantity,
                poitem.repairFlag AS Repair,
                poitem.revLevel AS RevisionLevel,
                poitem.statusId AS StatusId,
                poitem.taxRate AS TaxRateValue,
                poitem.tbdCostFlag AS CostToBeDetermined,
                poitem.totalCost AS TotalCost,
                poitem.typeId AS TypeId,
                poitem.unitCost AS PartPrice,
                poitem.uomId AS UnitOfMeasureId,
                poitem.vendorPartNum AS VendorPartNumber,
                poitem.outsourcedPartNumber AS OutsourcedPartNumber,
                poitem.outsourcedPartDescription AS OutsourcedPartDescription,

                customer.name AS CustomerName,
                qb.name AS QuickBooksClassName,
                status.name AS StatusName,
                taxrate.name AS TaxRateName,
                type.name AS TypeName,
                uom.code AS UOM

                FROM poitem

                LEFT JOIN customer ON customer.id = poitem.customerId
                LEFT JOIN part ON part.id = poitem.partId
                LEFT JOIN part AS outsourcedPart ON outsourcedPart.id = poitem.outsourcedPartId
                INNER JOIN po ON po.id = poitem.poId
                INNER JOIN poitemtype AS type ON type.id = poitem.typeId
                INNER JOIN poitemstatus AS status ON status.id = poitem.statusId
                LEFT JOIN qbclass AS qb ON qb.id = poitem.qbClassId
                INNER JOIN taxrate ON taxrate.id = poitem.taxId
                LEFT JOIN uom ON uom.id = poitem.uomId

                WHERE po.num = '{poNumber}'";

            // Execute the SELECT query
            var poItemsTable = await ExecuteQueryAsync(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Parse the rows into PurchaseOrderItem objects
            return ToPurchaseOrderItems(poItemsTable);
        }

        /// <summary>
        /// Create a purchase order
        /// </summary>
        /// <param name="purchaseOrder"></param>
        /// <returns></returns>
        public async Task<PurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
        {
            //string purchaseOrderJson = JsonSerializer.Serialize(purchaseOrder);
            //return PostAsync<PurchaseOrder, PurchaseOrder>("/api/purchase-orders", purchaseOrder, cancellationToken);



            // Ensure that the required fields have values
            if (purchaseOrder.Status == 0) purchaseOrder.Status = PurchaseOrderStatus.Historical;

            if (string.IsNullOrWhiteSpace(purchaseOrder.Vendor.Name)) purchaseOrder.Vendor.Name = "[UNIDENTIFIED VENDOR]";
            //if (string.IsNullOrWhiteSpace(purchaseOrder.VendorContactName)) purchaseOrder.VendorContactName = "[VENDOR CONTACT NAME]";

            if (string.IsNullOrWhiteSpace(purchaseOrder.RemitToAddress.Name)) purchaseOrder.RemitToAddress.Name = "[REMIT TO NAME]";
            if (string.IsNullOrWhiteSpace(purchaseOrder.RemitToAddress.StreetAddress)) purchaseOrder.RemitToAddress.StreetAddress = "[REMIT TO ADDRESS]";
            if (string.IsNullOrWhiteSpace(purchaseOrder.RemitToAddress.City)) purchaseOrder.RemitToAddress.City = "[REMIT TO CITY]";
            if (string.IsNullOrWhiteSpace(purchaseOrder.RemitToAddress.State)) purchaseOrder.RemitToAddress.State = "[REMIT TO STATE]";
            if (string.IsNullOrWhiteSpace(purchaseOrder.RemitToAddress.PostalCode)) purchaseOrder.RemitToAddress.PostalCode = "[REMIT TO ZIP]";
            if (string.IsNullOrWhiteSpace(purchaseOrder.RemitToAddress.Country)) purchaseOrder.RemitToAddress.Country = "USA";

            if (string.IsNullOrWhiteSpace(purchaseOrder.ShipToAddress.Name)) purchaseOrder.ShipToAddress.Name = "[SHIP TO NAME]";
            if (string.IsNullOrWhiteSpace(purchaseOrder.DeliverTo)) purchaseOrder.DeliverTo = "[DELIVER TO NAME]";
            if (string.IsNullOrWhiteSpace(purchaseOrder.ShipToAddress.StreetAddress)) purchaseOrder.ShipToAddress.StreetAddress = "[SHIP TO ADDRESS]";
            if (string.IsNullOrWhiteSpace(purchaseOrder.ShipToAddress.City)) purchaseOrder.ShipToAddress.City = "[SHIP TO CITY]";
            if (string.IsNullOrWhiteSpace(purchaseOrder.ShipToAddress.State)) purchaseOrder.ShipToAddress.State = "[SHIP TO STATE]";
            if (string.IsNullOrWhiteSpace(purchaseOrder.ShipToAddress.PostalCode)) purchaseOrder.ShipToAddress.PostalCode = "[SHIP TO ZIP]";
            if (string.IsNullOrWhiteSpace(purchaseOrder.ShipToAddress.Country)) purchaseOrder.ShipToAddress.Country = "USA";

            if (string.IsNullOrWhiteSpace(purchaseOrder.Carrier.Name)) purchaseOrder.Carrier.Name = "Will Call";
            //if (string.IsNullOrWhiteSpace(purchaseOrder.CarrierServiceName)) purchaseOrder.CarrierServiceName = "Ground";

            //if (string.IsNullOrWhiteSpace(purchaseOrder.ShippingTermsName)) purchaseOrder.ShippingTermsName = "Prepaid & Billed";
            //if (string.IsNullOrWhiteSpace(purchaseOrder.PaymentTermsName)) purchaseOrder.PaymentTermsName = "COD";

            // Compile the CSV rows for the specified Purchase Order objects
            string[] purchaseOrderLines = ToCsv(purchaseOrder);

            //Debug.WriteLine($"Purchase Order #{purchaseOrder.Number}");
            //foreach (string line in purchaseOrderLines) Debug.WriteLine(line);

            // Attempt to import the CSV rows into Fishbowl
            await ImportAsync(PURCHASEORDER_IMPORT_NAME, purchaseOrderLines, cancellationToken);

            // Return the new PO record
            return await GetPurchaseOrderByNumberAsync(purchaseOrder.Number, cancellationToken);
        }

        /// <summary>
        /// Updates a purchase order. Optional parameters that are not passed in will be reset to their default values. Best practice is to send the complete object you would like to save.
        /// </summary>
        /// <param name="purchaseOrder"></param>
        /// <returns></returns>
        public Task<PurchaseOrder> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default) => CreatePurchaseOrderAsync(purchaseOrder, cancellationToken);

        /// <summary>
        /// Updates a purchase order. Optional parameters that are not passed in will be reset to their default values. Best practice is to send the complete object you would like to save.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="purchaseOrder"></param>
        /// <returns></returns>
        public Task<PurchaseOrder> UpdatePurchaseOrderAsync(int id, PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
            => PostAsync<PurchaseOrder, PurchaseOrder>($"/api/purchase-orders/{id}", purchaseOrder, cancellationToken);

        /// <summary>
        /// Issues the purchase order with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PurchaseOrder> IssuePurchaseOrderAsync(int id, CancellationToken cancellationToken = default)
            => PostAsync<PurchaseOrder>($"/api/purchase-orders/{id}/issue", cancellationToken);

        /// <summary>
        /// Unissues the purchase order with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PurchaseOrder> UnissuePurchaseOrderAsync(int id, CancellationToken cancellationToken = default)
            => PostAsync<PurchaseOrder>($"/api/purchase-orders/{id}/unissue", cancellationToken);

        /// <summary>
        /// Closes short the purchase order with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PurchaseOrder> CloseShortPurchaseOrderAsync(int id, CancellationToken cancellationToken = default)
            => PostAsync<PurchaseOrder>($"/api/purchase-orders/{id}/close-short", cancellationToken);

        /// <summary>
        /// Close shorts the purchase order item with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="poItemId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PurchaseOrder> CloseShortPurchaseOrderItemAsync(int id, int poItemId, CancellationToken cancellationToken = default)
            => PostAsync<PurchaseOrder>($"/api/purchase-orders/{id}/close-short/{poItemId}", cancellationToken);

        /// <summary>
        /// Voids the purchase order with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PurchaseOrder> VoidPurchaseOrderAsync(int id, CancellationToken cancellationToken = default)
            => PostAsync<PurchaseOrder>($"/api/purchase-orders/{id}/void", cancellationToken);

        /// <summary>
        /// Deletes the purchase order with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeletePurchaseOrderAsync(int id, CancellationToken cancellationToken = default)
            => DeleteAsync($"/api/purchase-orders/{id}", cancellationToken);




        /// <summary>
        /// Deserialize CSV data to PurchaseOrder/PurchaseOrderItem DTOs
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private PurchaseOrder[] ToPurchaseOrders(DataTable table)
        {
            // Ensure that a DataTable was provided
            if (table == null) return Array.Empty<PurchaseOrder>();

            // Initialize the collection
            var items = new List<PurchaseOrder>();

            // Iterate through the data rows
            foreach (DataRow row in table.Rows)
            {
                // Parse the top-level properties
                var item = row.ToObject<PurchaseOrder>();

                item.Buyer.Id = row.Field<int>("BuyerId");
                item.Buyer.UserName = row.Field<string>("BuyerUserName");
                item.Buyer.FirstName = row.Field<string>("BuyerFirstName");
                item.Buyer.LastName = row.Field<string>("BuyerLastName");

                item.Class.Id = row.Field<int>("QuickBooksClassId");
                item.Class.Name = row.Field<string>("QuickBooksClassName");

                item.Carrier.Id = row.Field<int>("CarrierId");
                item.Carrier.Name = row.Field<string>("CarrierName");

                var createdDate = row.DateField("CreatedDate");
                if (createdDate.HasValue) item.Created.DateLastModified = createdDate.Value;

                var issuedDate = row.DateField("IssuedDate");
                if (issuedDate.HasValue) item.Issued.DateLastModified = issuedDate.Value;

                var lastModifiedDate = row.DateField("LastModifiedDate");
                if (lastModifiedDate.HasValue) item.LastModified.DateLastModified = lastModifiedDate.Value;

                item.PaymentTerms.Id = row.Field<int>("PaymentTermsId");
                item.PaymentTerms.Name = row.Field<string>("PaymentTermsName");

                // Populate the RemitTo address
                item.RemitToAddress.Name = row.Field<string>("RemitToName");
                item.RemitToAddress.StreetAddress = row.Field<string>("RemitToStreetAddress");
                item.RemitToAddress.City = row.Field<string>("RemitToCity");
                item.RemitToAddress.State = row.Field<string>("RemitToState");
                item.RemitToAddress.PostalCode = row.Field<string>("RemitToZip");
                item.RemitToAddress.Country = row.Field<string>("RemitToCountry");

                // Populate the ShipTo address
                item.ShipToAddress.Name = row.Field<string>("ShipToName");
                item.ShipToAddress.StreetAddress = row.Field<string>("ShipToStreetAddress");
                item.ShipToAddress.City = row.Field<string>("ShipToCity");
                item.ShipToAddress.State = row.Field<string>("ShipToState");
                item.ShipToAddress.PostalCode = row.Field<string>("ShipToZip");
                item.ShipToAddress.Country = row.Field<string>("ShipToCountry");

                //item.TaxRate.Id = row.Field<int>("TaxRateId");
                item.TaxRate.Name = row.Field<string>("TaxRateName");

                item.Vendor.Id = row.Field<int>("VendorId");
                item.Vendor.Name = row.Field<string>("VendorName");

                // Populate the Items collection
                item.Items.AddRange(GetPurchaseOrderItemsAsync(item.Number).GetAwaiter().GetResult());

                // Add the item to the collection
                items.Add(item);
            }

            // Return the completed collection
            return items.ToArray();
        }

        /// <summary>
        /// Deserialize CSV data to PurchaseOrderItem DTOs
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static PurchaseOrderItem[] ToPurchaseOrderItems(DataTable table)
        {
            // Ensure that a DataTable was provided
            if (table == null) return Array.Empty<PurchaseOrderItem>();

            // Initialize the collection
            var items = new List<PurchaseOrderItem>();

            // Iterate through the data rows
            foreach (DataRow row in table.Rows)
            {
                // Parse the top-level properties
                var item = row.ToObject<PurchaseOrderItem>();

                item.Class.Name = row.Field<string>("QuickBooksClassName");

                item.Part.Number = row.Field<string>("PartNumber");
                item.Part.Description = row.Field<string>("Description");
                
                int? customerId = row.Field<int?>("CustomerId");
                if (customerId.HasValue) item.Customer.CustomerId = customerId.Value;
                item.Customer.Name = row.Field<string>("CustomerName");

                item.DateLastFulfilled = row.DateField("LastFulfillmentDate");
                item.DateScheduled = row.DateField("FulfillmentDate");

                item.LineNumber = row.Field<int>("LineItem");

                item.OutsourcedPart.Number = row.Field<string>("OutsourcedPartNumber");
                item.OutsourcedPart.Description = row.Field<string>("OutsourcedPartDescription");

                item.QuantityFulfilled = row.DecimalField("FulfilledQuantity");
                item.QuantityPicked = row.DecimalField("PickedQuantity");
                item.Quantity = row.DecimalField("PartQuantity");

                item.Revision = row.Field<string>("RevisionLevel");

                item.Status = (PurchaseOrderItemStatus)row.Field<int>("StatusId");

                item.TaxRate.Rate = row.Field<decimal>("TaxRateValue");
                item.TaxRate.Name = row.Field<string>("TaxRateName");

                item.Type.Id = row.Field<int>("TypeId");
                item.Type.Name = row.Field<string>("TypeName");

                item.UnitCost = row.DecimalField("PartPrice");

                item.UnitOfMeasure.Id = row.Field<int>("UnitOfMeasureId");
                item.UnitOfMeasure.Abbreviation = row.Field<string>("UOM");

                //Repair
                //CostToBeDetermined
                //StatusName

                // Add the item to the collection
                items.Add(item);
            }

            // Return the completed collection
            return items.ToArray();
        }



        /// <summary>
        /// Serialize the PurchaseOrder/PurchaseOrderItem DTOs to CSV data.
        /// </summary>
        /// <param name="purchaseOrder"></param>
        /// <returns></returns>
        private static string[] ToCsv(PurchaseOrder purchaseOrder)
        {
            // Initialize CSV line collection
            var lines = new List<string>
            {
                "\"Flag\",\"PONum\",\"Status\",\"VendorName\",\"VendorContact\",\"RemitToName\",\"RemitToAddress\",\"RemitToCity\",\"RemitToState\",\"RemitToZip\",\"RemitToCountry\",\"ShipToName\",\"DeliverToName\",\"ShipToAddress\",\"ShipToCity\",\"ShipToState\",\"ShipToZip\",\"ShipToCountry\",\"CarrierName\",\"CarrierService\",\"VendorSONum\",\"CustomerSONum\",\"CreatedDate\",\"CompletedDate\",\"ConfirmedDate\",\"FulfillmentDate\",\"IssuedDate\",\"Buyer\",\"ShippingTerms\",\"PaymentTerms\",\"FOB\",\"Note\",\"QuickBooksClassName\",\"LocationGroupName\",\"URL\",\"Phone\",\"Email\"",
                "\"Flag\",\"POItemTypeID\",\"PartNumber\",\"VendorPartNumber\",\"PartQuantity\",\"FulfilledQuantity\",\"PickedQuantity\",\"UOM\",\"PartPrice\",\"FulfillmentDate\",\"LastFulfillmentDate\",\"RevisionLevel\",\"Note\",\"QuickBooksClassName\",\"CustomerJob\""
            };

            // Append the CSV line for the PO
            var csv = new CsvBuilder();

            csv.Add("PO");  // "Flag"
            csv.Add(purchaseOrder.Number);  // "PONum"
            csv.Add(((int)purchaseOrder.Status).ToString());  // "Status"
            csv.Add(purchaseOrder.Vendor.Name);  // "VendorName"
            csv.Add(purchaseOrder.RemitToAddress.Attention);  // "VendorContact"
            csv.Add(purchaseOrder.RemitToAddress.Name);  // "RemitToName"
            csv.Add(purchaseOrder.RemitToAddress.StreetAddress);  // "RemitToAddress"
            csv.Add(purchaseOrder.RemitToAddress.City);  // "RemitToCity"
            csv.Add(purchaseOrder.RemitToAddress.State);  // "RemitToState"
            csv.Add(purchaseOrder.RemitToAddress.PostalCode);  // "RemitToZip"
            csv.Add(purchaseOrder.RemitToAddress.Country);  // "RemitToCountry"
            csv.Add(purchaseOrder.ShipToAddress.Name);  // "ShipToName"
            csv.Add(purchaseOrder.DeliverTo);  // "DeliverToName"
            csv.Add(purchaseOrder.ShipToAddress.StreetAddress);  // "ShipToAddress"
            csv.Add(purchaseOrder.ShipToAddress.City);  // "ShipToCity"
            csv.Add(purchaseOrder.ShipToAddress.State);  // "ShipToState"
            csv.Add(purchaseOrder.ShipToAddress.PostalCode);  // "ShipToZip"
            csv.Add(purchaseOrder.ShipToAddress.Country);  // "ShipToCountry"
            csv.Add(purchaseOrder.Carrier.Name);  // "CarrierName"
            csv.Add(purchaseOrder.CarrierServiceName);  // "CarrierService"
            csv.Add(purchaseOrder.VendorSalesOrderNumber);  // "VendorSONum"
            csv.Add(purchaseOrder.CustomerSalesOrderNumber);  // "CustomerSONum"
            csv.Add(purchaseOrder.Created.DateLastModified.ToShortDateString());  // "CreatedDate"
            csv.Add(purchaseOrder.CompletedDate?.ToShortDateString());  // "CompletedDate"
            csv.Add(purchaseOrder.ConfirmedDate?.ToShortDateString());  // "ConfirmedDate"
            csv.Add(purchaseOrder.FulfillmentDate?.ToShortDateString());  // "FulfillmentDate"
            csv.Add(purchaseOrder.LastModified.DateLastModified.ToShortDateString());  // "IssuedDate"
            csv.Add(purchaseOrder.Buyer.UserName);  // "Buyer"
            csv.Add(purchaseOrder.ShippingTermsName);  // "ShippingTerms"
            csv.Add(purchaseOrder.PaymentTerms.Name);  // "PaymentTerms"
            csv.Add(purchaseOrder.FobPointName);  // "FOB"
            csv.Add(purchaseOrder.Note);  // "Note"
            csv.Add(purchaseOrder.Class.Name);  // "QuickBooksClassName"
            csv.Add(purchaseOrder.LocationGroup.Name);  // "LocationGroupName"
            csv.Add(purchaseOrder.Url);  // "URL"
            csv.Add(purchaseOrder.VendorPhone);  // "Phone"
            csv.Add(purchaseOrder.VendorEmail);  // "Email"

            lines.Add(csv.ToString());

            // Add the CSV lines for the PO Items
            if (purchaseOrder.Items != null)
            {
                foreach (var item in purchaseOrder.Items) lines.Add(ToCsv(item));
            }

            // Return the CSV lines
            return lines.ToArray();
        }

        /// <summary>
        /// Serialize the PurchaseOrderItem DTO to CSV data.
        /// </summary>
        /// <param name="purchaseOrder"></param>
        /// <returns></returns>
        private static string ToCsv(PurchaseOrderItem purchaseOrderItem)
        {
            //"Flag","POItemTypeID","PartNumber","VendorPartNumber","PartQuantity","FulfilledQuantity","PickedQuantity","UOM","PartPrice","FulfillmentDate","LastFulfillmentDate","RevisionLevel","Note","QuickBooksClassName","CustomerJob"

            // Build the CSV line for the PO Item
            var csv = new CsvBuilder();

            csv.Add("Item");  // "Flag"
            csv.Add(purchaseOrderItem.Type?.Id.ToString());  // "POItemTypeID"  // csv.Add(purchaseOrderItem.Type.GetDisplayName());
            csv.Add(purchaseOrderItem.Part?.Number);  // "PartNumber"  // csv.Add(purchaseOrderItem.PartNumber);
            csv.Add(purchaseOrderItem.VendorPartNumber);  // "VendorPartNumber"
            csv.Add(purchaseOrderItem.Quantity.ToString());  // "PartQuantity"
            csv.Add(purchaseOrderItem.QuantityFulfilled.ToString());  // "FulfilledQuantity"
            csv.Add(purchaseOrderItem.QuantityPicked.ToString());  // "PickedQuantity"
            csv.Add(purchaseOrderItem.UnitOfMeasure?.Abbreviation);  // "UOM"
            csv.Add(purchaseOrderItem.UnitCost.ToString());  // "PartPrice"
            csv.Add(purchaseOrderItem.DateScheduled?.ToFishbowlDateString());  // "FulfillmentDate"
            csv.Add(purchaseOrderItem.DateLastFulfilled?.ToFishbowlDateString());  // "LastFulfillmentDate"
            csv.Add(purchaseOrderItem.Revision);  // "RevisionLevel"
            csv.Add(purchaseOrderItem.Note);  // "Note"
            csv.Add(purchaseOrderItem.Class?.Name);  // "QuickBooksClassName"  //QuickBooksClassName
            csv.Add(purchaseOrderItem.Customer?.Name);  // "CustomerJob"  //CustomerName

            // Return the CSV lines
            return csv.ToString();
        }

        #endregion Purchase Orders

        #region Unit of Measure

        /*
        /// <summary>
        /// Get all Units of Measure that match the search criteria
        /// </summary>
        /// <param name="name">The UOM name.</param>
        /// <param name="abbreviation">The UOM abbreviation.</param>
        /// <param name="description">The UOM description.</param>
        /// <param name="type">The basic type of the UOM.</param>
        /// <param name="active">The active status of the UOM.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UnitOfMeasure[]> GetUnitOfMeasuresAsync(string name = null, string abbreviation = null, string description = null,
            UnitOfMeasureType? type = null, bool? active = null, CancellationToken cancellationToken = default)
        {
            // Query Fishbowl to determine how many Unit Of Measure records meet the criteria
            var sizeResult = await SearchUnitOfMeasuresAsync(1, 1, name, abbreviation, description, type, active, cancellationToken);
            int pageSize = sizeResult.TotalCount;

            // Perform the search
            var uomResult = await SearchUnitOfMeasuresAsync(1, pageSize, name, abbreviation, description, type, active, cancellationToken);

            // Return the results
            return uomResult.Results;
        }
        */

        /// <summary>
        /// Search for units of measure.
        /// </summary>
        /// <param name="pageNumber">The current page of the results.</param>
        /// <param name="pageSize">The number of returned results per page. (Default 100)</param>
        /// <param name="name">The UOM name.</param>
        /// <param name="abbreviation">The UOM abbreviation.</param>
        /// <param name="description">The UOM description.</param>
        /// <param name="type">The basic type of the UOM.</param>
        /// <param name="active">The active status of the UOM.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PagedResultSet<UnitOfMeasure>> SearchUnitOfMeasuresAsync(int pageNumber, int pageSize = 100, string name = null, string abbreviation = null, string description = null, 
            UnitOfMeasureType? type = null, bool? active = null, CancellationToken cancellationToken = default)
        {
            // Assemble the Request Uri
            var queryParameters = new Dictionary<string, string>
            {
                { "pageNumber", pageNumber.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            queryParameters.AddParameterIfNotNull("name", name);
            queryParameters.AddParameterIfNotNull("abbreviation", abbreviation);
            queryParameters.AddParameterIfNotNull("description", description);
            queryParameters.AddParameterIfNotNull("type", type);
            queryParameters.AddParameterIfNotNull("active", active);

            string requestUri = QueryHelpers.AddQueryString("/api/uoms", queryParameters);

            // Process the request
            return GetAsync<PagedResultSet<UnitOfMeasure>>(requestUri, cancellationToken);  //SearchUnitOfMeasureResponse
        }

        /// <summary>
        /// Retrieves the details of a unit of measure. You only need to provide the unique UOM ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<UnitOfMeasure> GetUnitOfMeasureAsync(int id, CancellationToken cancellationToken = default)
            => GetAsync<UnitOfMeasure>($"/api/uoms/{id}", cancellationToken);

        /// <summary>
        /// Create a unit of measure.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="unitOfMeasure"></param>
        /// <returns></returns>
        public Task<UnitOfMeasure> CreateUnitOfMeasureAsync(UnitOfMeasure unitOfMeasure, CancellationToken cancellationToken = default)
            => PostAsync<UnitOfMeasure, UnitOfMeasure>("/api/uoms", unitOfMeasure, cancellationToken);

        /// <summary>
        /// Update a unit of measure.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="unitOfMeasure"></param>
        /// <returns></returns>
        public Task<UnitOfMeasure> UpdateUnitOfMeasureAsync(int id, UnitOfMeasure unitOfMeasure, CancellationToken cancellationToken = default)
            => PostAsync<UnitOfMeasure, UnitOfMeasure>($"/api/uoms/{id}", unitOfMeasure, cancellationToken);

        /// <summary>
        /// Deletes the unit of measure with the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteUnitOfMeasureAsync(int id, CancellationToken cancellationToken = default)
            => DeleteAsync($"/api/uoms/{id}", cancellationToken);

        #endregion Unit of Measure

        #region Users

        private const string USER_IMPORT_NAME = "Users";

        private const string USER_SELECT_QUERY =
            @"SELECT
                id,
                userName AS username, 
                firstName, 
                lastName, 
                initials, 
                activeFlag AS active,
                email, 
                phone
                
            FROM sysuser AS user";
        private const string GROUP_SELECT_QUERY =
            @"SELECT
                user.id,
                user.userName, 
                user.firstName, 
                user.lastName, 
                user.initials, 
                user.activeFlag AS active,
                user.email, 
                user.phone
                
            FROM usergroup AS grp

            INNER JOIN usergrouprel AS rel ON rel.groupId = grp.id
            INNER JOIN sysuser AS user ON user.id = rel.userId";



        /// <summary>
        /// Get all Users
        /// </summary>
        public Task<User[]> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{USER_SELECT_QUERY} ORDER BY lastName,firstName";

            // Execute the SELECT query
            return ExecuteQueryAsync<User>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Search for users.
        /// </summary>
        /// <param name="pageNumber">The current page of the results.</param>
        /// <param name="pageSize">The number of returned results per page. (Default 100)</param>
        /// <param name="username">The username.</param>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="initials">The initials of the user's name.</param>
        /// <param name="active">The active status of the user.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PagedResultSet<User>> SearchUsersAsync(int pageNumber, int pageSize = 100, string username = null, string firstName = null, string lastName = null,
            string initials = null, bool? active = null, CancellationToken cancellationToken = default)
        {
            // Assemble the Request Uri
            var queryParameters = new Dictionary<string, string>
            {
                { "pageNumber", pageNumber.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            queryParameters.AddParameterIfNotNull("username", username);
            queryParameters.AddParameterIfNotNull("firstName", firstName);
            queryParameters.AddParameterIfNotNull("lastName", lastName);
            queryParameters.AddParameterIfNotNull("initials", initials);
            queryParameters.AddParameterIfNotNull("active", active);

            string requestUri = QueryHelpers.AddQueryString("/api/users", queryParameters);

            // Process the request
            return GetAsync<PagedResultSet<User>>(requestUri, cancellationToken);  //SearchUserResponse
        }

        /// <summary>
        /// Retrieves the details of an existing purchase order. You only need to provide the unique purchase order number.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public Task<User[]> GetUsersInGroupAsync(string groupName, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{GROUP_SELECT_QUERY} WHERE grp.name = '{groupName}' ORDER BY user.lastName, user.firstName";

            // Execute the SELECT query
            return ExecuteQueryAsync<User>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Retrieves the details of an existing user. You only need to provide the unique user ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(int id, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{USER_SELECT_QUERY} WHERE user.id = '{id}'";

            // Execute the SELECT query
            var users = await ExecuteQueryAsync<User>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Return the first/only record
            return users.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the details of an existing user. You only need to provide the unique <paramref name="userName"/>.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<User> GetUserByNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{USER_SELECT_QUERY} WHERE user.userName = '{userName}'";

            // Execute the SELECT query
            var users = await ExecuteQueryAsync<User>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Return the first/only record
            return users.FirstOrDefault();
        }

        /// <summary>
        /// Create a User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
        {
            // Ensure that the required fields have values
            //if (user.Status == 0) user.Status = PurchaseOrderStatus.Historical;

            //if (string.IsNullOrWhiteSpace(user.VendorName)) user.VendorName = "[UNIDENTIFIED VENDOR]";
            //if (string.IsNullOrWhiteSpace(purchaseOrder.VendorContactName)) purchaseOrder.VendorContactName = "[VENDOR CONTACT NAME]";

            // Compile the CSV rows for the specified Purchase Order objects
            string[] userLines = ToCsv(user);

            // Attempt to import the CSV rows into Fishbowl
            await ImportAsync(USER_IMPORT_NAME, userLines, cancellationToken);

            // Return the new PO record
            return await GetUserByNameAsync(user.UserName, cancellationToken);
        }

        /// <summary>
        /// Updates an User. Optional parameters that are not passed in will be reset to their default values. Best practice is to send the complete object you would like to save.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default) => CreateUserAsync(user, cancellationToken);



        /// <summary>
        /// Serialize the User DTOs to CSV data.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private static string[] ToCsv(User user)
        {
            // Initialize CSV line collection
            var lines = new List<string>
            {
                "\"UserName\",\"FirstName\",\"LastName\",\"Initials\",\"Active\",\"UserGroups\",\"DefaultLocGroup\",\"LocGroups\",\"Email\",\"Phone\""
            };

            // Append the CSV line for the PO
            var csv = new CsvBuilder();

            csv.Add(user.UserName);  // "UserName"
            csv.Add(user.FirstName);  // "FirstName"
            csv.Add(user.LastName);  // "LastName"
            csv.Add(user.Initials);  // "Initials"
            csv.Add(user.IsActive.ToString());  // "Active"
            csv.Add(user.UserGroups);  // "UserGroups"
            csv.Add(user.DefaultLocGroup);  // "DefaultLocGroup"
            csv.Add(user.LocGroups);  // "LocGroups"
            csv.Add(user.Email);  // "Email"
            csv.Add(user.Phone);  // "Phone"

            lines.Add(csv.ToString());

            // Return the CSV lines
            return lines.ToArray();
        }

        #endregion Users

        #region Vendors

        private const string VENDOR_IMPORT_NAME = "Vendors";
        private const string VENDOR_SELECT_QUERY =
            @"SELECT 
                vendor.id,
                vendor.name, 
                addr.addressName, 
                cont.contactName, 
                addr.typeID AS addressTypeId, 
                addrtype.name AS addressType, 
                addr.defaultFlag AS isDefault,
                addr.address AS streetAddress, 
                addr.city AS city, 
                state.code AS state, 
                addr.zip, 
                country.name AS country, 
                addr.residentialFlag AS isResidential, 
                mainContact.datus AS main,
                homeContact.datus AS home, 
                workContact.datus AS work, 
                mobileContact.datus AS mobile, 
                faxContact.datus AS fax, 
                emailContact.datus AS email, 
                pagerContact.datus AS pager,
                webContact.datus AS web, 
                otherContact.datus AS other, 
                status.name AS status, 
                vendor.accountNum AS accountNumber, 
                vendor.activeFlag AS active, 
                carr.name AS defaultCarrier, 
                ship.name AS defaultShippingTerms, 
                vendor.minOrderAmount,
                vendor.note AS note,
                vendor.url AS Url, 
                carsrv.name AS defaultCarrierService, 
                pymt.name AS defaultPaymentTerms,
                ship.name AS defaultShippingTerms, 
                vendor.creditLimit,
                vendor.dateEntered, 
                vendor.dateLastModified, 
                vendor.lastChangedUser, 
                vendor.leadTime,
                tax.name AS taxRateName,
                curr.name AS currencyName,
                vendor.currencyRate,
                vendor.leadTime

                FROM vendor

                LEFT JOIN address AS addr ON addr.accountId = vendor.accountId
                LEFT JOIN addresstype AS addrtype ON addrtype.Id = addr.typeID
                LEFT JOIN contact AS cont ON (cont.addressId = addr.id AND cont.accountId = vendor.accountId)
                LEFT JOIN stateconst AS state ON state.id = addr.stateId
                LEFT JOIN countryconst AS country ON country.id = addr.countryId
                LEFT JOIN contact AS homeContact ON (homeContact.accountId = vendor.accountId AND homeContact.typeID = '10')
                LEFT JOIN contact AS workContact ON (workContact.accountId = vendor.accountId AND workContact.typeID = '20')
                LEFT JOIN contact AS mobileContact ON (mobileContact.accountId = vendor.accountId AND mobileContact.typeID = '30')
                LEFT JOIN contact AS faxContact ON (faxContact.accountId = vendor.accountId AND faxContact.typeID = '40')
                LEFT JOIN contact AS mainContact ON (mainContact.accountId = vendor.accountId AND mainContact.typeID = '50')
                LEFT JOIN contact AS emailContact ON (emailContact.accountId = vendor.accountId AND emailContact.typeID = '60')
                LEFT JOIN contact AS pagerContact ON (pagerContact.accountId = vendor.accountId AND pagerContact.typeID = '70')
                LEFT JOIN contact AS otherContact ON (otherContact.accountId = vendor.accountId AND otherContact.typeID = '80')
                LEFT JOIN contact AS webContact ON (webContact.accountId = vendor.accountId AND webContact.typeID = '90')
                LEFT JOIN carrier AS carr ON carr.id = vendor.defaultCarrierId
                LEFT JOIN vendorstatus AS status ON status.id = vendor.statusId
                LEFT JOIN shipterms AS ship ON ship.id = vendor.defaultShipTermsId
                LEFT JOIN carrierservice AS carsrv ON carsrv.id = vendor.defaultCarrierServiceId
                LEFT JOIN taxrate AS tax ON tax.id = vendor.taxRateId
                LEFT JOIN account AS acct ON acct.id = vendor.accountId
                LEFT JOIN paymentterms AS pymt ON pymt.id = vendor.defaultPaymentTermsId
                LEFT JOIN currency AS curr ON curr.id = vendor.currencyId";



        /*
                addr.city,
                state.code AS state,
                country.abbreviation AS country,
        

                LEFT JOIN address AS addr ON addr.accountId = vendor.accountId AND addr.defaultFlag = true AND addr.typeID = 50
                LEFT JOIN stateconst AS state ON state.id = addr.stateId
                LEFT JOIN countryconst AS country ON country.id = addr.countryId
        */

        /*private const string VENDOR_SELECT_QUERY =
            @"SELECT 
                vendor.id,
                vendor.name, 
                addr.addressName, 
                cont.contactName, 
                addr.typeID AS addressType, 
                addr.defaultFlag AS isDefault,
                addr.address AS streetAddress, 
                addr.city AS city, 
                state.code AS state, 
                addr.zip, 
                country.name AS country, 
                addr.residentialFlag AS isResidential, 
                mainContact.datus AS main,
                homeContact.datus AS home, 
                workContact.datus AS work, 
                mobileContact.datus AS mobile, 
                faxContact.datus AS fax, 
                emailContact.datus AS email, 
                pagerContact.datus AS pager,
                webContact.datus AS web, 
                otherContact.datus AS other, 
                status.name AS status, 
                vendor.accountNum AS accountNumber, 
                vendor.activeFlag AS active, 
                carr.name AS defaultCarrier, 
                ship.name AS defaultShippingTerms, 
                vendor.minOrderAmount,
                vendor.note AS note,
                vendor.url AS Url, 
                carsrv.name AS defaultCarrierService, 
                pymt.name AS defaultTerms, 
                vendor.creditLimit,
                vendor.dateEntered, 
                vendor.dateLastModified, 
                vendor.lastChangedUser, 
                vendor.leadTime,
                tax.name AS taxRateName

                FROM vendor

                LEFT JOIN address AS addr ON addr.accountId = vendor.accountId
                LEFT JOIN contact AS cont ON (cont.addressId = addr.id AND cont.accountId = vendor.accountId)
                LEFT JOIN stateconst AS state ON state.id = addr.stateId
                LEFT JOIN countryconst AS country ON country.id = addr.countryId
                LEFT JOIN contact AS homeContact ON (homeContact.accountId = vendor.accountId AND homeContact.typeID = '10')
                LEFT JOIN contact AS workContact ON (workContact.accountId = vendor.accountId AND workContact.typeID = '20')
                LEFT JOIN contact AS mobileContact ON (mobileContact.accountId = vendor.accountId AND mobileContact.typeID = '30')
                LEFT JOIN contact AS faxContact ON (faxContact.accountId = vendor.accountId AND faxContact.typeID = '40')
                LEFT JOIN contact AS mainContact ON (mainContact.accountId = vendor.accountId AND mainContact.typeID = '50')
                LEFT JOIN contact AS emailContact ON (emailContact.accountId = vendor.accountId AND emailContact.typeID = '60')
                LEFT JOIN contact AS pagerContact ON (pagerContact.accountId = vendor.accountId AND pagerContact.typeID = '70')
                LEFT JOIN contact AS otherContact ON (otherContact.accountId = vendor.accountId AND otherContact.typeID = '80')
                LEFT JOIN contact AS webContact ON (webContact.accountId = vendor.accountId AND webContact.typeID = '90')
                LEFT JOIN carrier AS carr ON carr.id = vendor.defaultCarrierId
                LEFT JOIN vendorstatus AS status ON status.id = vendor.statusId
                LEFT JOIN shipterms AS ship ON ship.id = vendor.defaultShipTermsId
                LEFT JOIN carrierservice AS carsrv ON carsrv.id = vendor.defaultCarrierServiceId
                LEFT JOIN taxrate AS tax ON tax.id = vendor.taxRateId
                LEFT JOIN account AS acct ON acct.id = vendor.accountId
                LEFT JOIN paymentterms AS pymt ON pymt.id = vendor.defaultPaymentTermsId
                LEFT JOIN currency AS curr ON curr.id = vendor.currencyId";*/



        /// <summary>
        /// Get all Vendors that match the search criteria
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Vendor[]> GetVendorsAsync(CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{VENDOR_SELECT_QUERY} GROUP BY addr.id ORDER BY vendor.name";

            // Execute the SELECT query
            var vendorTable =  await ExecuteQueryAsync(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Parse the rows into Vendor/Address/Contact objects
            return ToVendors(vendorTable);
        }

        /// <summary>
        /// Search for vendors.
        /// </summary>
        /// <param name="pageNumber">The current page of the results.</param>
        /// <param name="pageSize">The number of returned results per page. (Default 100)</param>
        /// <param name="name">The vendor name.</param>
        /// <param name="accountNumber">The vendor account number.</param>
        /// <param name="city">The vendor address city.</param>
        /// <param name="state">The vendor address state.</param>
        /// <param name="country">The vendor address country</param>
        /// <param name="active">Indicates if the vendor is active.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<PagedResultSet<Vendor>> SearchVendorsAsync(int pageNumber, int pageSize = 100, string name = null, string accountNumber = null, string city = null,
            string state = null, string country = null, bool? active = null, CancellationToken cancellationToken = default)
        {
            // Assemble the Request Uri
            var queryParameters = new Dictionary<string, string>
            {
                { "pageNumber", pageNumber.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            queryParameters.AddParameterIfNotNull("name", name);
            queryParameters.AddParameterIfNotNull("accountNumber", accountNumber);
            queryParameters.AddParameterIfNotNull("city", city);
            queryParameters.AddParameterIfNotNull("state", state);
            queryParameters.AddParameterIfNotNull("country", country);
            queryParameters.AddParameterIfNotNull("active", active);

            string requestUri = QueryHelpers.AddQueryString("/api/vendors", queryParameters);

            // Process the request
            return GetAsync<PagedResultSet<Vendor>>(requestUri, cancellationToken);  //SearchVendorResponse
        }

        /// <summary>
        /// Return the Vendor containing the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Vendor> GetVendorAsync(int id, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{VENDOR_SELECT_QUERY} WHERE (vendor.id = {id}) AND (addr.defaultFlag = TRUE) GROUP BY addr.id";

            // Execute the SELECT query
            var vendorTable = await ExecuteQueryAsync(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Parse the rows into Vendor/Address/Contact objects
            return ToVendors(vendorTable).FirstOrDefault();
        }

        /// <summary>
        /// Return the Vendor containing the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Vendor> GetVendorByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{VENDOR_SELECT_QUERY} WHERE (LOWER(vendor.name) LIKE \"{name.ToLower()}\") AND (addr.defaultFlag = TRUE) GROUP BY addr.id";

            // Execute the SELECT query
            var vendorTable = await ExecuteQueryAsync(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Parse the rows into Vendor/Address/Contact objects
            return ToVendors(vendorTable).FirstOrDefault();
        }

        /// <summary>
        /// Create a vendor
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        public async Task<Vendor> CreateVendorAsync(Vendor vendor, CancellationToken cancellationToken = default)
        {
            // Compile the CSV rows for the specified Vendor objects
            string[] csvLines = ToCsv(vendor);

            // Attempt to import the CSV rows into Fishbowl
            await ImportAsync(VENDOR_IMPORT_NAME, csvLines, cancellationToken);

            // Return the new Vendor record
            return await GetVendorByNameAsync(vendor.Name, cancellationToken);
        }

        /// <summary>
        /// Updates a vendor. Optional parameters that are not passed in will be reset to their default values. Best practice is to send the complete object you would like to save.
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        public Task<Vendor> UpdateVendorAsync(Vendor vendor, CancellationToken cancellationToken = default) => CreateVendorAsync(vendor, cancellationToken);



        /// <summary>
        /// Deserialize CSV data to Vendor/Address/Contact DTOs
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static Vendor[] ToVendors(DataTable table)
        {
            // Ensure that a valid DataTable was provided
            if (table == null) return Array.Empty<Vendor>();

            // Initialize CSV dto collection
            var vendors = new List<Vendor>();

            // Iterate through the VendorCSV rows
            foreach (DataRow row in table.Rows)
            {
                // Create/Retrieve the Vendor DTO by the csv's Name
                var vendor = vendors.SingleOrDefault(c => c.Name == row.Field<string>("name"));
                if (vendor == null)
                {
                    _ = Enum.TryParse(row.Field<string>("status"), out VendorStatus vendorStatus);

                    vendor = new Vendor
                    {
                        Id = row.Field<int>("id"),
                        Name = row.Field<string>("name"),
                        CurrencyName = row.Field<string>("currencyName"),
                        CurrencyRate = row.Field<decimal>("currencyRate"),
                        DefaultPaymentTerms = row.Field<string>("defaultPaymentTerms"),
                        DefaultCarrier = row.Field<string>("defaultCarrier"),
                        DefaultShippingTerms = row.Field<string>("defaultShippingTerms"),
                        Status = vendorStatus,
                        AccountNumber = row.Field<string>("accountNumber"),
                        IsActive = row.Field<bool>("active"),
                        //MinOrderAmount = row.Field<string>("MinOrderAmount"),
                        Note = row.Field<string>("note"),
                        Url = row.Field<string>("url"),
                        DefaultCarrierService = row.Field<string>("defaultCarrierService")
                    };

                    vendors.Add(vendor);
                }

                // Create/Retrieve the Address DTO by the csv's AddressName
                var address = vendor.Addresses.SingleOrDefault(a => a.Name == row.Field<string>("addressName"));
                if (address == null)
                {
                    _ = Enum.TryParse(row.Field<string>("addressType"), out AddressType addressType);

                    address = new Address
                    {
                        Name = row.Field<string>("addressName"),
                        Attention = row.Field<string>("contactName"),
                        Type = addressType,
                        IsDefault = row.Field<bool>("isDefault"),
                        StreetAddress = row.Field<string>("streetAddress"),
                        City = row.Field<string>("city"),
                        State = row.Field<string>("state"),
                        PostalCode = row.Field<string>("zip"),
                        Country = row.Field<string>("country")
                    };

                    vendor.Addresses.Add(address);
                }

                // Create/Retrieve the Contact records
                var homeContact = AddUpdateContact(address.Contacts, row.Field<string>("contactName"), ContactType.Home, row.Field<string>("home"));
                var workContact = AddUpdateContact(address.Contacts, row.Field<string>("contactName"), ContactType.Work, row.Field<string>("work"));
                var mobileContact = AddUpdateContact(address.Contacts, row.Field<string>("contactName"), ContactType.Mobile, row.Field<string>("mobile"));
                var faxContact = AddUpdateContact(address.Contacts, row.Field<string>("contactName"), ContactType.Fax, row.Field<string>("fax"));
                var mainContact = AddUpdateContact(address.Contacts, row.Field<string>("contactName"), ContactType.Main, row.Field<string>("main"));
                var emailContact = AddUpdateContact(address.Contacts, row.Field<string>("contactName"), ContactType.Email, row.Field<string>("email"));
                var pagerContact = AddUpdateContact(address.Contacts, row.Field<string>("contactName"), ContactType.Pager, row.Field<string>("pager"));
                var otherContact = AddUpdateContact(address.Contacts, row.Field<string>("contactName"), ContactType.Other, row.Field<string>("other"));
                var webContact = AddUpdateContact(address.Contacts, row.Field<string>("contactName"), ContactType.Web, row.Field<string>("web"));
            }


            //Name	            AddressName	        AddressContact	AddressType	IsDefault	Address	                    City	    State	Zip	    Country	Residential	Main	                Home	Work	Mobile	Fax	Email	                        Pager	Web	Other	Group	CreditLimit	Status	Active	TaxRate	    Salesman	DefaultPriority	Number	    PaymentTerms	TaxExempt	TaxExemptNumber	URL	CarrierName	CarrierService	ShippingTerms	AlertNotes	QuickBooksClassName	ToBeEmailed	ToBePrinted	IssuableStatus
            //Copelands, Inc.   Main Address        Copelands, Inc. 50          TRUE        PO Box 787                  Ooltewah    TN      37363           FALSE       (423) 238 - 5621 ext219                             wcopeland@copelandsinc.net                                  0           Normal  TRUE                                            COPELANDS   NET 30          FALSE                                                       Prepaid                     None                TRUE        TRUE
            //Copelands, Inc.   Billing Address     Copelands, Inc. 20          TRUE        PO Box 787                  Ooltewah    TN      37363           FALSE       (423) 238 - 5621 ext219                             wcopeland@copelandsinc.net                                  0           Normal  TRUE                                            COPELANDS   NET 30          FALSE                                                       Prepaid                     None                TRUE        TRUE
            //Copelands, Inc.   Shipping Address    Copelands, Inc. 10          TRUE        9616 Ooltewah Industrial Dr Ooltewah    TN      37363           FALSE       (423) 238 - 5621 ext219                             wcopeland@copelandsinc.net                                  0           Normal  TRUE                                            COPELANDS   NET 30          FALSE                                                       Prepaid                     None                TRUE        TRUE

            // Return the populated collection
            return vendors.ToArray();
        }

        /// <summary>
        /// Return the Contact record matching the Type and Datum (if it exists).  Otherwise, a new Contact record is created.
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="datum"></param>
        /// <returns></returns>
        private static ContactInformation AddUpdateContact(ICollection<ContactInformation> contacts, string name, ContactType type, string datum)
        {
            // Search for a Contact record matching the specified Type and Datum
            var contact = contacts.SingleOrDefault(c => c.Type == type && c.Datum == datum);

            // Create a new Contact record (if necessary)
            if (contact == null)
            {
                contact = new ContactInformation
                {
                    Name = name,
                    Type = type,
                    Datum = datum,
                    IsDefault = !contacts.Any(c => c.Type == type)
                };

                contacts.Add(contact);
            }

            // Return the Contact record
            return contact;
        }



        /// <summary>
        /// Serialize Vendor/Address/Contact DTOs to CSV data
        /// </summary>
        /// <param name="vendors"></param>
        /// <returns></returns>
        private static string[] ToCsv(Vendor vendor, bool includeHeaderRow = true)
        {
            // Ensure that the Vendor record is valid
            if (vendor == null) throw new ArgumentNullException(nameof(vendor));
            if (vendor.Addresses == null || vendor.Addresses.Count == 0) throw new ArgumentException("The Vendor record must have at least one address!", nameof(vendor));

            // Initialize CSV lines collection
            var lines = new List<string>();

            if (includeHeaderRow)
                lines.Add("Name,AddressName,AddressContact,AddressType,IsDefault,Address,City,State,Zip,Country,Main,Home,Work,Mobile,Fax,Email,Pager,Web,Other,CurrencyName,CurrencyRate,DefaultTerms,DefaultCarrier,DefaultShippingTerms,Status,AccountNumber,Active,MinOrderAmount,AlertNotes,URL,DefaultCarrierService");
            //lines.Add("Name,AddressName,AddressContact,AddressType,IsDefault,Address,City,State,Zip,Country,Residential,Main,Home,Work,Mobile,Fax,Email,Pager,Web,Other,Group,CreditLimit,Status,Active,TaxRate,Salesman,DefaultPriority,Number,PaymentTerms,TaxExempt,TaxExemptNumber,URL,CarrierName,CarrierService,ShippingTerms,AlertNotes,QuickBooksClassName,ToBeEmailed,ToBePrinted,IssuableStatus");
                        
            // Iterate through the Address DTOs
            foreach (var address in vendor.Addresses)
            {
                // Create pointers to address contacts
                var homeContact = address.Contacts.SingleOrDefault(c => c.Type == ContactType.Home);
                var workContact = address.Contacts.SingleOrDefault(c => c.Type == ContactType.Work);
                var mobileContact = address.Contacts.SingleOrDefault(c => c.Type == ContactType.Mobile);
                var faxContact = address.Contacts.SingleOrDefault(c => c.Type == ContactType.Fax);
                var mainContact = address.Contacts.SingleOrDefault(c => c.Type == ContactType.Main);
                var emailContact = address.Contacts.SingleOrDefault(c => c.Type == ContactType.Email);
                var pagerContact = address.Contacts.SingleOrDefault(c => c.Type == ContactType.Pager);
                var otherContact = address.Contacts.SingleOrDefault(c => c.Type == ContactType.Other);
                var webContact = address.Contacts.SingleOrDefault(c => c.Type == ContactType.Web);

                // Create a CSV record for this Vendor-Address-Contact row
                var csv = new CsvBuilder();

                csv.Add(vendor.Name);  // Name
                csv.Add(address.Name);  // AddressName
                csv.Add(address.Attention);  // AddressContact
                csv.Add(((int)address.Type).ToString());  // AddressType
                csv.Add(address.IsDefault.ToString());  // IsDefault
                csv.Add(address.StreetAddress);  // Address
                csv.Add(address.City);  // City
                csv.Add(address.State);  // State
                csv.Add(address.PostalCode);  // Zip
                csv.Add(address.Country);  // Country
                //csv.Add(address.IsResidential.ToString());  // Residential
                csv.Add(mainContact?.Datum);  // Main
                csv.Add(homeContact?.Datum);  // Home
                csv.Add(workContact?.Datum);  // Work
                csv.Add(mobileContact?.Datum);  // Mobile
                csv.Add(faxContact?.Datum);  // Fax
                csv.Add(emailContact?.Datum);  // Email
                csv.Add(pagerContact?.Datum);  // Pager
                csv.Add(webContact?.Datum);  // Web
                csv.Add(otherContact?.Datum);  // Other
                csv.Add(vendor.CurrencyName);  // CurrencyName
                csv.Add(vendor.CurrencyRate.ToString());  // CurrencyRate
                csv.Add(vendor.DefaultPaymentTerms);  // DefaultTerms
                csv.Add(vendor.DefaultCarrier);  // DefaultCarrier
                csv.Add(vendor.DefaultShippingTerms);  // DefaultShippingTerms
                csv.Add(vendor.Status.ToString());  // Status
                csv.Add(vendor.AccountNumber);  // AccountNumber
                csv.Add(vendor.IsActive.ToString());  // Active
                csv.Add(vendor.MinimumOrderAmount.ToString());  // MinOrderAmount
                csv.Add(vendor.Note);  // AlertNotes
                csv.Add(vendor.Url);  // URL
                csv.Add(vendor.DefaultCarrierService);  // DefaultCarrierService

                // Add the completed CSV line to the collection
                string csvLine = csv.ToString();

                lines.Add(csvLine);
            }

            // Return the populated collection
            return lines.ToArray();



            //Name	            AddressName	        AddressContact	AddressType	IsDefault	Address	                    City	    State	Zip	    Country	Residential	Main	                Home	Work	Mobile	Fax	Email	                        Pager	Web	Other	Group	CreditLimit	Status	Active	TaxRate	    Salesman	DefaultPriority	Number	    PaymentTerms	TaxExempt	TaxExemptNumber	URL	CarrierName	CarrierService	ShippingTerms	AlertNotes	QuickBooksClassName	ToBeEmailed	ToBePrinted	IssuableStatus
            //Copelands, Inc.   Main Address        Copelands, Inc. 50          TRUE        PO Box 787                  Ooltewah    TN      37363           FALSE       (423) 238 - 5621 ext219                             wcopeland@copelandsinc.net                                  0           Normal  TRUE                                            COPELANDS   NET 30          FALSE                                                       Prepaid                     None                TRUE        TRUE
            //Copelands, Inc.   Billing Address     Copelands, Inc. 20          TRUE        PO Box 787                  Ooltewah    TN      37363           FALSE       (423) 238 - 5621 ext219                             wcopeland@copelandsinc.net                                  0           Normal  TRUE                                            COPELANDS   NET 30          FALSE                                                       Prepaid                     None                TRUE        TRUE
            //Copelands, Inc.   Shipping Address    Copelands, Inc. 10          TRUE        9616 Ooltewah Industrial Dr Ooltewah    TN      37363           FALSE       (423) 238 - 5621 ext219                             wcopeland@copelandsinc.net                                  0           Normal  TRUE                                            COPELANDS   NET 30          FALSE                                                       Prepaid                     None                TRUE        TRUE
        }

        #endregion Vendors

        #region Work Orders

        private const string WORKORDER_SELECT_QUERY =
            @"SELECT
            wo.id, 
            wo.calCategoryId, 
            wo.cost, 
            wo.customerId,
            wo.dateCreated, 
            wo.dateFinished,
            wo.dateLastModified, 
            wo.dateScheduled,
            wo.dateScheduledToStart, 
            wo.dateStarted,
            wo.locationGroupId, 
            wo.locationId,
            wo.moItemId, 
            wo.note, 
            wo.num, 
            wo.priorityId, 
            wo.qbClassId, 
            wo.qtyOrdered,
            wo.qtyScrapped, 
            wo.qtyTarget,
            wo.statusId,
            stat.name AS status,
		    wo.userId, 
                
		    location.name AS locationName,
		    lg.name AS locationGroupName,
            mo.num AS manufacturingOrderNumber, 
            moitem.description,
		    qb.name AS quickBooksClassName,
		    stat.name AS statusName,
		    user.userName

            FROM wo

            LEFT JOIN calcategory AS calcat ON calcat.id = wo.calCategoryId
            LEFT JOIN customer ON customer.id = wo.customerId
            LEFT JOIN locationgroup AS lg ON lg.id = wo.locationGroupId
            LEFT JOIN location ON location.id = wo.locationId
            INNER JOIN moitem ON moitem.id = wo.moItemId
            LEFT JOIN mo ON moitem.moId = mo.id
            INNER JOIN priority ON priority.id = wo.priorityId
            LEFT JOIN qbclass AS qb ON qb.id = wo.qbClassId
            INNER JOIN sysuser AS user ON user.id = wo.userId
            INNER JOIN wostatus AS stat ON stat.id = wo.statusId";
        


        /// <summary>
        /// Return all Work Orders in Fishbowl
        /// </summary>
        /// <returns></returns>
        public Task<WorkOrder[]> GetWorkOrdersAsync(CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{WORKORDER_SELECT_QUERY} ORDER BY wo.num";

            // Execute the SELECT query
            return ExecuteQueryAsync<WorkOrder>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Return all Work Orders in Fishbowl belonging to the specified Manufacturing Order
        /// </summary>
        /// <returns></returns>
        public Task<WorkOrder[]> GetWorkOrdersAsync(string moNumber, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{WORKORDER_SELECT_QUERY} WHERE mo.num = '{moNumber}' ORDER BY wo.num";

            // Execute the SELECT query
            return ExecuteQueryAsync<WorkOrder>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Retrieves the details of an existing work order. You only need to provide the unique work order ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WorkOrder> GetWorkOrderAsync(int id, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{WORKORDER_SELECT_QUERY} WHERE wo.id = '{id}'";

            // Execute the SELECT query
            var workOrders = await ExecuteQueryAsync<WorkOrder>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Return the first (and presumably only) record
            return workOrders.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the details of an existing work order. You only need to provide the unique work order number.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task<WorkOrder> GetWorkOrderByNumberAsync(string number, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery = $"{WORKORDER_SELECT_QUERY} WHERE wo.num = '{number}'";

            // Execute the SELECT query
            var workOrders = await ExecuteQueryAsync<WorkOrder>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);

            // Return the first (and presumably only) record
            return workOrders.FirstOrDefault();
        }



        /// <summary>
        /// Return all Work Order Items in Fishbowl belonging to the specified Work Order
        /// </summary>
        /// <param name="woNumber">The unique number of the Work Order</param>
        /// <returns></returns>
        public Task<WorkOrderItem[]> GetWorkOrderItemsAsync(string woNumber, CancellationToken cancellationToken = default)
        {
            // Build the MySQL SELECT query
            string sqlQuery =
                @$"SELECT
                woitem.id AS Id, 
                woitem.moItemId AS ManufacturingOrderItemId, 
                woitem.partId AS PartId,
                woitem.typeId AS TypeId, 
                woitem.uomId AS UomId, 
                woitem.woId AS WorkOrderId, 
                woitem.cost AS Cost,
                woitem.standardCost AS StandardCost, 
                woitem.description AS Description, 
                woitem.qtyScrapped AS QuantityScrapped,
                woitem.qtyTarget AS QuantityTarget, 
                woitem.qtyUsed AS QuantityUsed, 
                woitem.sortId AS SortId, 
                woitem.oneTimeItem AS OneTimeItem,

                part.num AS PartNumber,
                uom.code AS UOM

                FROM woitem

                LEFT JOIN part ON woitem.partId = part.id
                LEFT JOIN moitem ON woitem.moItemId = moitem.id
                LEFT JOIN uom ON woitem.uomId = uom.id
                LEFT JOIN wo ON woitem.woId = wo.id

                WHERE wo.num = '{woNumber}'";

            // Execute the SELECT query
            return ExecuteQueryAsync<WorkOrderItem>(sqlQuery: sqlQuery, cancellationToken: cancellationToken);
        }

        #endregion Work Orders



        #region API Helper Methods

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive
        /// notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The requestUri must be an absolute URI or System.Net.Http.HttpClient.BaseAddress
        /// must be set.
        /// </exception>
        /// <exception cref="System.Net.Http.HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <exception cref="System.Threading.Tasks.TaskCanceledException">
        /// .NET Core and .NET 5.0 and later only: The request failed due to timeout.
        /// </exception>
        private async Task<TResponse> GetAsync<TResponse>(string requestUri, CancellationToken cancellationToken = default)
        {
            // HTTP GET
            var response = await _httpClient.GetAsync(requestUri, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, cancellationToken);
            }
            else
            {
                throw await HandleRequestFailureAsync(requestUri, response, "Failed to complete HTTP GET request.", cancellationToken);
            }
        }



        /// <summary>
        /// Send a POST request with a cancellation token as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive
        /// notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The requestUri must be an absolute URI or System.Net.Http.HttpClient.BaseAddress
        /// must be set.
        /// </exception>
        /// <exception cref="System.Net.Http.HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <exception cref="System.Threading.Tasks.TaskCanceledException">
        /// .NET Core and .NET 5.0 and later only: The request failed due to timeout.
        /// </exception>
        private async Task PostAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            // HTTP POST
            var response = await _httpClient.PostAsync(requestUri, null, cancellationToken: cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                throw await HandleRequestFailureAsync(requestUri, response, "Failed to complete HTTP POST request.", cancellationToken);
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri containing the value serialized as JSON
        /// in the request body.
        /// </summary>
        /// <typeparam name="TRequest">The type of the value to serialize.</typeparam>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="requestData">The value to serialize.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive
        /// notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The requestUri must be an absolute URI or System.Net.Http.HttpClient.BaseAddress
        /// must be set.
        /// </exception>
        /// <exception cref="System.Net.Http.HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <exception cref="System.Threading.Tasks.TaskCanceledException">
        /// .NET Core and .NET 5.0 and later only: The request failed due to timeout.
        /// </exception>
        private async Task PostAsync<TRequest>(string requestUri, TRequest requestData, CancellationToken cancellationToken = default)
        {
            // HTTP POST
            var response = await _httpClient.PostAsJsonAsync(requestUri, requestData, cancellationToken: cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                throw await HandleRequestFailureAsync(requestUri, response, "Failed to complete HTTP POST request.", cancellationToken);
            }
        }

        /// <summary>
        /// Send a POST request with a cancellation token as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TRequest">The type of the value to serialize.</typeparam>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="requestData">The value to serialize.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive
        /// notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The requestUri must be an absolute URI or System.Net.Http.HttpClient.BaseAddress
        /// must be set.
        /// </exception>
        /// <exception cref="System.Net.Http.HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <exception cref="System.Threading.Tasks.TaskCanceledException">
        /// .NET Core and .NET 5.0 and later only: The request failed due to timeout.
        /// </exception>
        private async Task<TResponse> PostAsync<TResponse>(string requestUri, CancellationToken cancellationToken = default)
        {
            // HTTP POST
            var response = await _httpClient.PostAsync(requestUri, null, cancellationToken: cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, cancellationToken);
            }
            else
            {
                throw await HandleRequestFailureAsync(requestUri, response, "Failed to complete HTTP POST request.", cancellationToken);
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri containing the value serialized as JSON
        /// in the request body.
        /// </summary>
        /// <typeparam name="TRequest">The type of the value to serialize.</typeparam>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="requestData">The value to serialize.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive
        /// notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The requestUri must be an absolute URI or System.Net.Http.HttpClient.BaseAddress
        /// must be set.
        /// </exception>
        /// <exception cref="System.Net.Http.HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <exception cref="System.Threading.Tasks.TaskCanceledException">
        /// .NET Core and .NET 5.0 and later only: The request failed due to timeout.
        /// </exception>
        private async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest requestData, CancellationToken cancellationToken = default)
        {
            // HTTP POST
            var response = await _httpClient.PostAsJsonAsync(requestUri, requestData, cancellationToken: cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, cancellationToken);
            }
            else
            {
                throw await HandleRequestFailureAsync(requestUri, response, "Failed to complete HTTP POST request.", cancellationToken);
            }
        }


        /*
        /// <summary>
        /// Send a PUT request with a cancellation token as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive
        /// notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The requestUri must be an absolute URI or System.Net.Http.HttpClient.BaseAddress
        /// must be set.
        /// </exception>
        /// <exception cref="System.Net.Http.HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <exception cref="System.Threading.Tasks.TaskCanceledException">
        /// .NET Core and .NET 5.0 and later only: The request failed due to timeout.
        /// </exception>
        private async Task PutAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            // HTTP PUT
            var response = await _httpClient.PutAsync(requestUri, null, cancellationToken: cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                //_ = response.Content.ReadFromJsonAsync<TValue>(_jsonOptions, cancellationToken);
                return;
            }
            else
            {
                throw await HandleRequestFailureAsync(requestUri, response, "Failed to complete HTTP PUT request.", cancellationToken);
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri containing the value serialized as JSON
        /// in the request body.
        /// </summary>
        /// <typeparam name="TRequest">The type of the value to serialize.</typeparam>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="requestData">The value to serialize.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive
        /// notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The requestUri must be an absolute URI or System.Net.Http.HttpClient.BaseAddress
        /// must be set.
        /// </exception>
        /// <exception cref="System.Net.Http.HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <exception cref="System.Threading.Tasks.TaskCanceledException">
        /// .NET Core and .NET 5.0 and later only: The request failed due to timeout.
        /// </exception>
        private async Task PutAsync<TRequest>(string requestUri, TRequest requestData, CancellationToken cancellationToken = default)
        {
            // HTTP PUT
            var response = await _httpClient.PutAsJsonAsync(requestUri, requestData, cancellationToken: cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                //_ = response.Content.ReadFromJsonAsync<TValue>(_jsonOptions, cancellationToken);
                return;
            }
            else
            {
                throw await HandleRequestFailureAsync(requestUri, response, "Failed to complete HTTP PUT request.", cancellationToken);
            }
        }
        */


        /// <summary>
        /// Send a DELETE request with a cancellation token as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive
        /// notice of cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The requestUri must be an absolute URI or System.Net.Http.HttpClient.BaseAddress
        /// must be set.
        /// </exception>
        /// <exception cref="System.Net.Http.HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <exception cref="System.Threading.Tasks.TaskCanceledException">
        /// .NET Core and .NET 5.0 and later only: The request failed due to timeout.
        /// </exception>
        private async Task DeleteAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            // HTTP DELETE
            var response = await _httpClient.DeleteAsync(requestUri, cancellationToken: cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                //_ = response.Content.ReadFromJsonAsync<TValue>(_jsonOptions, cancellationToken);
                return;
            }
            else
            {
                throw await HandleRequestFailureAsync(requestUri, response, "Failed to complete HTTP DELETE request.", cancellationToken);
            }
        }



        /// <summary>
        /// Encapsulate the HttpResponseMessage in a FishbowlInventoryOperationException object.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="httpResponse"></param>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="FishbowlInventoryOperationException"></exception>
        private async Task<Exception> HandleRequestFailureAsync(string requestUri, HttpResponseMessage httpResponse, string message, CancellationToken cancellationToken = default)
        {
            // Extract the response's content as a string
            var content = await httpResponse.Content.ReadAsStringAsync();
            var fishbowlError = await httpResponse.Content.ReadFromJsonAsync<FishbowlErrorResponse>(_jsonOptions, cancellationToken);
            string errorMessage = fishbowlError?.Message;

            //{"timeStamp":"2022-10-21T15:40:18.531-0400","status":401,"message":"Invalid authorization token.","path":"/api/integrations/plugin-info"}

            // Dispose of the content object (if necessary)
            httpResponse.Content?.Dispose();

            // Throw the appropriate exception
            return httpResponse.StatusCode switch
            {
                HttpStatusCode.BadRequest => new FishbowlInventoryOperationException(errorMessage ?? "Could not accept the request. Possibly a missing required parameter.", requestUri, httpResponse.StatusCode, content),
                HttpStatusCode.Unauthorized => new FishbowlInventoryAuthenticationException(errorMessage ?? "Invalid session key or login credentials.", requestUri, httpResponse.StatusCode, content),
                HttpStatusCode.PaymentRequired => new FishbowlInventoryOperationException(errorMessage ?? "The parameters were valid but the request failed.", requestUri, httpResponse.StatusCode, content),
                HttpStatusCode.Forbidden => new FishbowlInventoryAuthorizationException(errorMessage ?? "The current user did not have the required access rights.", requestUri, httpResponse.StatusCode, content),
                HttpStatusCode.NotFound => new FishbowlInventoryOperationException(errorMessage ?? "The path does not exist.", requestUri, httpResponse.StatusCode, content),
                HttpStatusCode.InternalServerError => new FishbowlInventoryOperationException(errorMessage ?? "An error occurred on Fishbowl's end.", requestUri, httpResponse.StatusCode, content),
                _ => new FishbowlInventoryOperationException(message + " - " + errorMessage, requestUri, httpResponse.StatusCode, content),
            };
        }

        #endregion API Helper Methods

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Terminate the session (if logged in)
                if (!string.IsNullOrWhiteSpace(Token))
                    Task.Run(() => LogoutAsync()).GetAwaiter().GetResult();

                // Dispose managed objects
                _httpClient.Dispose();
            }

            // Dispose unmanaged objects
            _disposed = true;
        }

        #endregion IDisposable Members
    }
}