using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FishbowlInventory.Models
{
    public class PagedResultSet<T>
    {
        /// <summary>
        /// The total number of records in the database.
        /// </summary>
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// The total number of pages based on the specified Page Size.
        /// </summary>
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// The current page of the results.
        /// </summary>
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// The number of returned results per page. (Default 100)
        /// </summary>
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// The search results
        /// </summary>
        [JsonPropertyName("results")]
        public T[] Results { get; set; }
    }
}
