namespace TicketingSystem.Repository.Models
{
    public class TicketAttachment
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;

        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;

        public Guid UploadedBy { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}