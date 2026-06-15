namespace TicketingSystem.Services.Features.TicketMediator.Contracts
{
    public class UpdateTicketDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
    }
}
