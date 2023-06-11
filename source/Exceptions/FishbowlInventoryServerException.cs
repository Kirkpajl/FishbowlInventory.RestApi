using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FishbowlInventory.Exceptions
{
    public sealed class FishbowlInventoryServerException : FishbowlInventoryException
    {
        public FishbowlInventoryServerException() { }

        public FishbowlInventoryServerException(string message) : base(message) { }

        public FishbowlInventoryServerException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public FishbowlInventoryServerException(string message, Exception innerException) : base(message, innerException) { }

        public FishbowlInventoryServerException(string message, string apiPath, HttpStatusCode statusCode, string content) : base(message, apiPath, statusCode, content) { }

        public FishbowlInventoryServerException(string message, Exception innerException, string apiPath, HttpStatusCode statusCode, string content) : base(message, innerException, apiPath, statusCode, content) { }
    }
}
