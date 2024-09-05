namespace tc.Utils.Exception
{
    public class ServiceUnavailableException : ApiResponseException
    {
        public ServiceUnavailableException()
        {
        }

        public ServiceUnavailableException(string? message) : base(message)
        {
        }
    }
}
