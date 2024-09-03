using System;

namespace tc.Utils.Exception
{
    internal class JsonParseException : ArgumentException
    {
        public JsonParseException(string message) : base(message) { }

        public JsonParseException()
        {
        }

        public JsonParseException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
