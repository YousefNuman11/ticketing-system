namespace TicketingSystem.Services.DTOs.TicketDtos
{
    public class TicketFilterDto
    {
        public string? Status { get; set; }

        public Guid? EmployeeId { get; set; }

        public Guid? ClientId { get; set; }
    }
}