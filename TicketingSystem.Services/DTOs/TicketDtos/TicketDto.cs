namespace TicketingSystem.Services.DTOs.TicketDtos
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid? AssignedEmployeeId { get; set; }
    }
}