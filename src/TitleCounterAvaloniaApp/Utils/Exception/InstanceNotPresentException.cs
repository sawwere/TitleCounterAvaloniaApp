using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tc.Utils.Exception
{
    public class InstanceNotPresentException : System.Exception
    {
        public InstanceNotPresentException(string message) : base(message)
        {
        }

        public InstanceNotPresentException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
