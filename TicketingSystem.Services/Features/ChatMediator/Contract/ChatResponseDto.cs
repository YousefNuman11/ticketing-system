namespace TicketingSystem.Services.Features.ChatMediator.Contract
{
    public class ChatResponseDto
    {
        public string Answer { get; set; } = string.Empty;
        public List<Guid> SourceTicketIds { get; set; } = new();
    }
}
