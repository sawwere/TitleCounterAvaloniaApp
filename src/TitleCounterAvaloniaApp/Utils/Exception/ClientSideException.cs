using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace tc.Utils.Exception
{
    class ClientSideException : ApiResponseException
    {
        public ClientSideException()
        {
        }

        public ClientSideException(string? message) : base(message)
        {
        }

        public ClientSideException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}
