namespace TicketingSystem.Services.Features.AuthMediator.Contracts
{
    public class LoginDto
    {
        public string Identifier { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
