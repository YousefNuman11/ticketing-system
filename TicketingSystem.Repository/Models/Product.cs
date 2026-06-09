namespace TicketingSystem.Repository.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive{ get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}