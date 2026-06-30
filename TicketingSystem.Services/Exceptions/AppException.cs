namespace TicketingSystem.Services.Exceptions
{
    public abstract class AppException : Exception
    {
        public int StatusCode { get; }

        protected AppException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
