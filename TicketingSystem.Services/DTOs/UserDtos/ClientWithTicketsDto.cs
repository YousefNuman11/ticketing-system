namespace TicketingSystem.Services.DTOs.UserDtos
{
    public class ClientWithTicketsDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<ClientTicketDto> Tickets { get; set; } = new();
    }
}
