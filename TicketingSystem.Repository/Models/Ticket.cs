namespace TicketingSystem.Repository.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }

        public Guid UserId { get; set; }
        public Guid? AssignedEmployeeId { get; set; }
        public Guid ProductId { get; set; }

        public User User { get; set; } = null!;
        public User? AssignedEmployee { get; set; }
        public Product Product { get; set; } = null!;
        public ICollection<TicketAttachment> TicketAttachments { get; set; } = new List<TicketAttachment>();
        public ICollection<TicketsComment> TicketsComments { get; set; } = new List<TicketsComment>();
    }
}