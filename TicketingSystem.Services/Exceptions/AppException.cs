namespace TicketingSystem.Services.Exceptions
{
    /// <summary>
    /// Base type for all application/domain exceptions that map to an HTTP status code.
    /// </summary>
    public abstract class AppException : Exception
    {
        public int StatusCode { get; }

        protected AppException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
