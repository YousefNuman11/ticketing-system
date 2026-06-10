namespace TicketingSystem.Services.DTOs.TicketDtos
{
    public class TicketDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string? EmployeeName { get; set; }
    }
}
