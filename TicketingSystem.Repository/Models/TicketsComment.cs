
namespace TicketingSystem.Repository.Models
{
    public class TicketsComment
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }

        public Ticket Ticket { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}