namespace TicketingSystem.Services.DTOs.CommentDtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
