using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace tc.Utils.Exception
{
    public class ApiResponseException : System.Exception
    {
        public ApiResponseException()
        {
        }

        public ApiResponseException(string? message) : base(message)
        {
        }

        public ApiResponseException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}
