using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FishbowlInventory.Exceptions
{
    public class FishbowlInventoryAuthorizationException : FishbowlInventoryException
    {
        public FishbowlInventoryAuthorizationException() { }

        public FishbowlInventoryAuthorizationException(string message) : base(message) { }

        public FishbowlInventoryAuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public FishbowlInventoryAuthorizationException(string message, Exception innerException) : base(message, innerException) { }

        public FishbowlInventoryAuthorizationException(string message, string apiPath, HttpStatusCode statusCode, string content) : base(message, apiPath, statusCode, content) { }

        public FishbowlInventoryAuthorizationException(string message, Exception innerException, string apiPath, HttpStatusCode statusCode, string content) : base(message, innerException, apiPath, statusCode, content) { }
    }
}
