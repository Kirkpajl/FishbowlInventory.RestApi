using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FishbowlInventory.Exceptions
{
    public abstract class FishbowlInventoryException : Exception
    {
        protected FishbowlInventoryException() { }

        protected FishbowlInventoryException(string message) : base(message) { }

        protected FishbowlInventoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected FishbowlInventoryException(string message, Exception innerException) : base(message, innerException) { }

        protected FishbowlInventoryException(string message, string apiPath, HttpStatusCode statusCode, string content) : base(message)
        {
            ApiPath = apiPath;
            StatusCode = statusCode;
            Content = content;
        }

        protected FishbowlInventoryException(string message, Exception innerException, string apiPath, HttpStatusCode statusCode, string content) : base(message, innerException)
        {
            ApiPath = apiPath;
            StatusCode = statusCode;
            Content = content;
        }



        public string ApiPath { get; }
        public HttpStatusCode StatusCode { get; }
        public string Content { get; }
    }
}
