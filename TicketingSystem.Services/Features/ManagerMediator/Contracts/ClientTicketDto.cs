namespace TicketingSystem.Services.Features.ManagerMediator.Contracts
{
    public class ClientTicketDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
