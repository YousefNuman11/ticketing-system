namespace TicketingSystem.Services.Exceptions
{
    public class ValidationException : AppException
    {
        public ValidationException(string message) : base(400, message) { }
    }
}
