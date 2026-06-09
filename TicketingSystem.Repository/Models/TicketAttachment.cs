namespace TicketingSystem.Repository.Models
{
    public class TicketAttachment
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;
    }
}
