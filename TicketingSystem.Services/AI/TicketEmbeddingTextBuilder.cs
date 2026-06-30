using TicketingSystem.Repository.Models;

namespace TicketingSystem.Services.AI
{
    public class TicketEmbeddingTextBuilder
    {
        public static string Build(Ticket ticket)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Title: {ticket.Title}");
            sb.AppendLine($"Description: {ticket.Description}");

            if (ticket.AssignedEmployeeId.HasValue)
            {
                var lastEmployeeComment = ticket.TicketsComments
                    .Where(c => c.UserId == ticket.AssignedEmployeeId.Value)
                    .OrderByDescending(c => c.CreatedAt)
                    .FirstOrDefault();

                if (lastEmployeeComment != null)
                {
                    sb.AppendLine($"Resolution: {lastEmployeeComment.Text}");
                }
            }

            return sb.ToString();
        }
    }
}
