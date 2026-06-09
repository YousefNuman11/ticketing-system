namespace TicketingSystem.Repository.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Email{ get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public DateTime DateOfBirth{ get; set; }
        public UserRole Role { get; set; }
        public string PasswordHash{ get; set; } = string.Empty;
        public string Address{ get; set; } = string.Empty;
        public bool IsActive{ get; set; }

        public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
        public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
        public ICollection<TicketsComment> TicketsComments { get; set; } = new List<TicketsComment>();
    }
}